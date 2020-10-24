using UnityEngine;
using UnityEngine.AI;

namespace BotStates
{
  public class BotStateWander : BotStateBase
  {
    private Vector3 _targetPosition;
    
    public BotStateWander(BallBotControl control) : base(control)
    {
      StateDuration = 3f;
      StateDurationRandom = 3f;
      ChanceToSelect = 0.1f;
    }

    public override void OnEnter()
    {
      base.OnEnter();
      
      var retries = 10;
      while (retries > 0)
      {
        var bounds = Level.Instance.LevelBounds;

        var from = new Vector2(bounds.min.x, bounds.min.z);
        var to = new Vector2(bounds.max.x, bounds.max.z);

        var result = new Vector3(Random.Range(from.x, to.x), 200, Random.Range(from.y, to.y));

        if (Physics.Raycast(new Ray(result, Vector3.down), out var rayHit, 400, int.MaxValue))
        {
          result.y = rayHit.point.y;
        }

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
      Move = -(Control.Rigidbody.position - _targetPosition).normalized;
      if (Vector3.Distance(_targetPosition, Control.Rigidbody.position) < 5f)
      {
        Finish = true;
      }
      else
      {
        Debug.DrawLine(Control.transform.position, _targetPosition, Color.green, 0f,false);
      }
    }
  }
}