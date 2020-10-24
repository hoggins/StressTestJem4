using UnityEngine;

namespace BotStates
{
  public class BotStateFindPlayer : BotStateBase
  {

    public BotStateFindPlayer(BallBotControl control)
      : base(control)
    {
      StateDuration = 10f;
      StateDurationRandom = 10;
    }

    public override void Update()
    {
      _move = -(Control.Rigidbody.position - Control.Agent.nextPosition).normalized;
      Control.Agent.SetDestination(Player.Instance.transform.position);
    }
  }
}