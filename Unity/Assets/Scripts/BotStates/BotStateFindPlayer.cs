using UnityEngine;

namespace BotStates
{
  public class BotStateFindPlayer : BotStateBase
  {
    public BotStateFindPlayer(BallBotControl control)
      : base(control)
    {
      StateDuration = 10f;
      StateDurationRandom = 5f;
      ChanceToSelect = 0.35f;
    }

    public override void Update()
    {
      Move = -(Control.Rigidbody.position - Control.Agent.nextPosition).normalized;
      Control.Agent.SetDestination(Player.Instance.transform.position);
    }
  }
}