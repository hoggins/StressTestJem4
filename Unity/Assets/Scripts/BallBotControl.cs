using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BotStates;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Vehicles.Ball;
using Random = UnityEngine.Random;

public class BallBotControl : MonoBehaviour
{
  [NonSerialized]
  public Ball Ball;
  
  [NonSerialized]
  public NavMeshAgent Agent;
  
  [NonSerialized]
  public Rigidbody Rigidbody;
  
  [NonSerialized]
  public CatPlacer CatPlacer;


  public float TimeBetweenMinJumps = 3.0f;
  public float TimeBetweenMaxJumps = 3.0f;
  public float ChanceToJump = .25f;

  public bool Jump;

  private float _stuckDuration;
  
  private Vector3 _lastPosition;
  private Vector3? _stuckStartPosition;

  private List<BotStateBase> _allStates = new List<BotStateBase>();
  private BotStateFindPlayer _findPlayer;
  private BotStateWander _wander;
  private BotStateKillPlayer _killPlayer;
  private BotStateCollect _collect;
  private BotStateTryWin _win;
  private BotStateBase _currentState;

  private float _stateTimeLeft;
  private float _jumpTimeLeft;
  private float _pressJumpTimer = 0;

  private void Awake()
  {
    _allStates = new List<BotStateBase>
    {
      (_findPlayer = new BotStateFindPlayer(this)),
      (_wander = new BotStateWander(this)),
      (_killPlayer = new BotStateKillPlayer(this)),
      (_collect = new BotStateCollect(this)),
      (_win = new BotStateTryWin(this)),
    };
    
    Ball = GetComponent<Ball>();
    Agent = GetComponent<NavMeshAgent>();
    Rigidbody = GetComponent<Rigidbody>();
    CatPlacer = GetComponent<CatPlacer>();
    Agent.updatePosition = false;
    Agent.updateRotation = false;
    Agent.updateUpAxis = false;
    _lastPosition = transform.position;

    SelectRandomState();
    ResetJumpTimer();
  }

  void Start()
  {
    // NavMesh.SamplePosition(transform.position, out var hit, 5.0f, int.MaxValue);
    // _agent.Warp(hit.position);
  }

  private void Update()
  {
    _currentState.Update();

    if (Vector3.Distance(transform.position, Player.Instance.transform.position) > 80)
    {
      Ball.m_botFarBonus = 3;
    }
    else
    {
      Ball.m_botFarBonus = 1;
    }

    foreach (var state in _allStates)
    {
      state.UpdateChanceToSelect();
    }
  }

  private void LateUpdate()
  {
    var max = Mathf.Max(Rigidbody.velocity.magnitude, 5);
    if (Vector3.Distance(Rigidbody.position, Agent.nextPosition) > max)
      Agent.speed = 0f;
    else
      Agent.speed = max;

    _currentState.LateUpdate();
    
    _stateTimeLeft -= Time.deltaTime;
    if (_stateTimeLeft <= 0)
    {
      SelectRandomState();
    }
  }
  
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("WinTrigger") && GetComponent<CatPlacer>().CatsAttached >= GameManager.Instance.WinScore)
    {
      GameManager.Instance.Lose();
    }
  }

  private void SelectRandomState()
  {
    var availableStates = new List<BotStateBase>();

    var tries = 15;
    while (availableStates.Count == 0 && tries-- > 0)
      availableStates = _allStates.Where(x => x.CanSelect()).ToList();

    if (tries <= 0)
    {
      SetState(_wander);
    }
    else
    {
      var index = Random.Range(0, availableStates.Count);
      SetState(availableStates[index]);
    }
  }

  private void FixedUpdate()
  {
    UpdateJump();
    _currentState.FixedUpdate();
    // Call the Move function of the ball controller
    UpdateStuckCheat();
  }

  private void UpdateJump()
  {
    Jump = false;
    _jumpTimeLeft -= Time.fixedDeltaTime;

    if (_jumpTimeLeft <= 0)
    {
      if (Random.Range(0, 1f) < ChanceToJump)
      {
        _pressJumpTimer = Random.Range(0.1f, 0.5f);
      }

      ResetJumpTimer();
    }

    _pressJumpTimer -= Time.fixedDeltaTime;
    if (_pressJumpTimer > 0)
    {
      Jump = true;
    }
  }

  private void ResetJumpTimer()
  {
    _jumpTimeLeft = Random.Range(TimeBetweenMinJumps, TimeBetweenMaxJumps);
  }

  private void UpdateStuckCheat()
  {
    const float stuckDistance = 2f;
    const float stuckDuration = 2f;
    if(!Agent.hasPath)
      ResetStuck();
    
    if (Vector3.Distance(_lastPosition, Rigidbody.position) < stuckDistance)
    {
      if (!_stuckStartPosition.HasValue)
      {
        _stuckStartPosition = _lastPosition;
      }

      if (Vector3.Distance(_stuckStartPosition.Value, Rigidbody.position) < stuckDistance)
      {
        _stuckDuration += Time.deltaTime;
      }
      else
      {
        ResetStuck();
      }
    }
    else
    {
      ResetStuck();
    }

    if (_stuckDuration > stuckDuration)
    {
      Agent.ResetPath();
      _stuckDuration = 0;
      Agent.Warp(Rigidbody.position);
      StartCoroutine(StuckBoost());
    }

    _lastPosition = Rigidbody.transform.position;
  }

  private void ResetStuck()
  {
    _stuckDuration = 0f;
    _stuckStartPosition = null;
    Ball.m_MovePowerBonus = 0f;
  }

  private IEnumerator StuckBoost()
  {
    Ball.m_MovePowerBonus = 5.0f; 
    yield return new WaitForSeconds(2.0f);

    ResetStuck();
  }

  private void SetState(BotStateBase state)
  {
    _currentState = state;
    _currentState.OnEnter();
    _stateTimeLeft = _currentState.StateDuration + Random.Range(0, _currentState.StateDurationRandom);
  }

  private void OnDrawGizmos()
  {
    if(_currentState == null)
      return;

#if UNITY_EDITOR
    Handles.Label(transform.position + new Vector3(1, 2, 0), _currentState.GetType().Name);
#endif
  }
}