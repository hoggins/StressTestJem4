using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Vehicles.Ball;

public class BallBotControl : MonoBehaviour
{
  private Ball _ball;
  private NavMeshAgent _agent;

  private Vector3 _move;
  private bool _jump;

  private void Awake()
  {
    // Set up the reference.
    _ball = GetComponent<Ball>();
    _agent = GetComponent<NavMeshAgent>();
  }

  private void Update()
  {
    _move = new Vector3(0, 0, 1);
    _jump = true;
  }


  private void FixedUpdate()
  {
    // Call the Move function of the ball controller
    _ball.Move(_move, _jump);
    _jump = false;
  }
}