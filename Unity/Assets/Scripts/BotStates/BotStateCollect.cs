namespace BotStates
{
  public class BotStateCollect : BotStateBase
  {
    public BotStateCollect(BallBotControl control) : base(control)
    {
      StateDuration = 20f;
      StateDurationRandom = 10;
      ChanceToSelect = 1f;
    }
    
    public override void Update()
    {
      var closest = CatControl.GetClosest(Control.Rigidbody.position);
      
      _move = -(Control.Rigidbody.position - Control.Agent.nextPosition).normalized;
      Control.Agent.SetDestination(closest.transform.position);
    }

    public override bool CanSelect()
    {
      return CatControl.AliveCats.Count > 0 && base.CanSelect();
    }
  }
}