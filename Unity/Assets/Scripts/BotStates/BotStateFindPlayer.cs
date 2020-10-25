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
      ChanceToSelect = 0.40f;
    }
    
    
    public override void UpdateChanceToSelect()
    {
      var t = (float)Player.Instance.GetComponent<CatPlacer>().CatsAttached / (float)GameManager.Instance.WinScore;
      ChanceToSelect = Mathf.Lerp(0.0f, 0.6f, t);
    }

    public override void Update()
    {
      Move = -(Control.Rigidbody.position - Control.Agent.nextPosition).normalized;
      Control.Agent.SetDestination(Player.Instance.transform.position);
    }
  }
}