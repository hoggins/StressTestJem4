using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

namespace BotStates
{
  public class BotStateWander : BotStateBase
  {
    private Vector3 _targetPosition;
    
    public BotStateWander(BallBotControl control) : base(control)
    {
      StateDuration = 10f;
      StateDurationRandom = 5f;
    }

    public override void OnEnter()
    {
      var retries = 10;
      while (retries > 0)
      {
        var bounds = Level.Instance.LevelBounds;

        var from = new Vector2(bounds.min.x, bounds.min.z);
        var to = new Vector2(bounds.max.x, bounds.max.z);

        var result = new Vector3(Random.Range(from.x, to.x), 0, Random.Range(from.y, to.y));


        if (NavMesh.SamplePosition(result, out var hit, 10, int.MaxValue))
        {
          _targetPosition = hit.position;
          Control.Agent.SetDestination(_targetPosition);
          break;
        }

        retries--;
      }
    }

    public override void Update()
    {
      _move = -(Control.Rigidbody.position - _targetPosition).normalized;
    }
  }
}