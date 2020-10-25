using UnityEngine;

namespace BotStates
{
  public class BotStateTryWin : BotStateBase
  {
    public BotStateTryWin(BallBotControl control)
      : base(control)
    {
      ChanceToSelect = 1f;
      StateDuration = 20f;
      StateDurationRandom = 10f;
    }

    public override void OnEnter()
    {
      base.OnEnter();
      Control.Agent.SetDestination(Level.Instance.WinTargetPoint.position);
    }

    public override void Update()
    {
      base.Update();
      
      if (!CanSelect())
      {
        Finish = true;
        return;
      }

      Move = -(Control.Rigidbody.position - Control.Agent.nextPosition).normalized;
    }

    public override bool CanSelect()
    {
      return Control.CatPlacer.CatsAttached >= GameManager.Instance.WinScore;
    }
  }
}