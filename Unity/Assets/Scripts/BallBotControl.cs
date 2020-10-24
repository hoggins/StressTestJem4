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

  private float _stuckDuration;
  private Vector3 _lastPosition;
  private Vector3? _stuckStartPosition;

  private List<BotStateBase> _allStates = new List<BotStateBase>();
  private BotStateFindPlayer _findPlayer;
  private BotStateWander _wander;
  private BotStateBase _currentState;

  private float _stateTimeLeft;

  private void Awake()
  {
    _allStates = new List<BotStateBase>
    {
      (_findPlayer = new BotStateFindPlayer(this)),
      (_wander = new BotStateWander(this)),
    };
    
    Ball = GetComponent<Ball>();
    Agent = GetComponent<NavMeshAgent>();
    Rigidbody = GetComponent<Rigidbody>();
    Agent.updatePosition = false;
    Agent.updateRotation = false;
    Agent.updateUpAxis = false;
    _lastPosition = transform.position;

    SelectRandomState();
  }

  void Start()
  {
    // NavMesh.SamplePosition(transform.position, out var hit, 5.0f, int.MaxValue);
    // _agent.Warp(hit.position);
  }

  private void Update()
  {
    _currentState.Update();
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

  private void SelectRandomState()
  {
    var availableStates = _allStates.Where(x => x.CanSelect()).ToList();
    var index = Random.Range(0, availableStates.Count);
    SetState(availableStates[index]);
  }

  private void FixedUpdate()
  {
    _currentState.FixedUpdate();
    // Call the Move function of the ball controller
    UpdateStuckCheat();
  }

  private void UpdateStuckCheat()
  {
    const float stuckDistance = 2f;
    const float stuckDuration = 2f;
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
      StartCoroutine(Boost());
    }

    _lastPosition = Rigidbody.transform.position;
  }

  private void ResetStuck()
  {
    _stuckDuration = 0f;
    _stuckStartPosition = null;
    Ball.m_MovePowerBonus = 0f;
  }

  private IEnumerator Boost()
  {
    Ball.m_MovePowerBonus = 5.0f; 
    yield return new WaitForSeconds(2.0f);

    ResetStuck();
  }

  private void SetState(BotStateBase findPlayer)
  {
    _currentState = findPlayer;
    _stateTimeLeft = _currentState.StateDuration + Random.Range(0, _currentState.StateDurationRandom);
  }

  private void OnDrawGizmos()
  {
#if UNITY_EDITOR
    Handles.Label(transform.position + new Vector3(1, 2, 0), _currentState.GetType().Name);
#endif
  }
}