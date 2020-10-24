using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Vehicles.Ball;

public class BallBotControl : MonoBehaviour
{
  private Ball _ball;
  private NavMeshAgent _agent;
  private Rigidbody _rigidbody;

  private Vector3 _move;
  private bool _jump;

  private float _stuckDuration;
  private Vector3 _lastPosition;
  private Vector3? _stuckStartPosition;

  private void Awake()
  {
    _ball = GetComponent<Ball>();
    _agent = GetComponent<NavMeshAgent>();
    _rigidbody = GetComponent<Rigidbody>();
    _agent.updatePosition = false;
    _agent.updateRotation = false;
    _agent.updateUpAxis = false;
    _lastPosition = transform.position;
    // _agent.Warp(transform.position);
    // _agent.ResetPath();

  }

  void Start()
  {
    // NavMesh.SamplePosition(transform.position, out var hit, 5.0f, int.MaxValue);
    // _agent.Warp(hit.position);
  }

  private void Update()
  {
    _move = Vector3.forward;
    var direction = -(_rigidbody.position - _agent.nextPosition).normalized;
    _move = direction;

    _agent.SetDestination(Player.Instance.transform.position);
    // _jump = true;
    
  }

  private void LateUpdate()
  {
    var max = Mathf.Max(_rigidbody.velocity.magnitude, 5);
    if (Vector3.Distance(_rigidbody.position, _agent.nextPosition) > max)
      _agent.speed = 0f;
    else
     _agent.speed = max;
    
    // _agent.velocity = agentVelocity.magnitude < rigidbodyVelocity.magnitude
    // ? rigidbodyVelocity
    // : agentVelocity;
  }

  private void FixedUpdate()
  {
    // Call the Move function of the ball controller
    _ball.Move(_move, _jump);
    _jump = false;
    UpdateStuckCheat();
  }

  private void UpdateStuckCheat()
  {
    const float stuckDistance = 2f;
    const float stuckDuration = 2f;
    if (Vector3.Distance(_lastPosition, _rigidbody.position) < stuckDistance)
    {
      if (!_stuckStartPosition.HasValue)
      {
        _stuckStartPosition = _lastPosition;
      }

      if (Vector3.Distance(_stuckStartPosition.Value, _rigidbody.position) < stuckDistance)
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
      _agent.ResetPath();
      _stuckDuration = 0;
      _agent.Warp(_rigidbody.position);
      StartCoroutine(Boost());
    }

    _lastPosition = _rigidbody.transform.position;
  }

  private void ResetStuck()
  {
    _stuckDuration = 0f;
    _stuckStartPosition = null;
    _ball.m_MovePowerBonus = 0f;
  }

  private IEnumerator Boost()
  {
    _ball.m_MovePowerBonus = 5.0f; 
    yield return new WaitForSeconds(2.0f);

    ResetStuck();
  }
}