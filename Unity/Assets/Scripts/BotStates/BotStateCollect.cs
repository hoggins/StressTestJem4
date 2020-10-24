namespace BotStates
{
  public class BotStateCollect : BotStateBase
  {
    public BotStateCollect(BallBotControl control) : base(control)
    {
      StateDuration = 5f;
      StateDurationRandom = 10;
      ChanceToSelect = 0.8f;
    }
    
    public override void Update()
    {
      var closest = CatControl.GetClosest(Control.Rigidbody.position);
      if (closest == null)
      {
        Finish = true;
        return;
      }

      Move = -(Control.Rigidbody.position - Control.Agent.nextPosition).normalized;
      Control.Agent.SetDestination(closest.transform.position);
    }

    public override bool CanSelect()
    {
      return CatControl.AliveCats.Count > 0 && base.CanSelect();
    }
  }
}