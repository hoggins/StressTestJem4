using UnityEngine;

namespace BotStates
{
  public class BotStateCollect : BotStateBase
  {
    private bool NearPlayer = false;
    public Vector3 PosOffset;
    private bool CloseToMe;

    public BotStateCollect(BallBotControl control) : base(control)
    {
      StateDuration = 6f;
      StateDurationRandom = 6f;
      ChanceToSelect = 0.8f;
    }

    public override void OnEnter()
    {
      base.OnEnter();
      NearPlayer = Random.Range(0, 1) > 0.2f;
      CloseToMe = Random.Range(0, 1) > 0.8f;
      PosOffset = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
    }


    public override void Update()
    {
      var rigidbodyPosition = Control.Rigidbody.position;
      var closest = CloseToMe
        ? CatControl.GetClosest(rigidbodyPosition)
        : CatControl.GetClosestNearPlayer(rigidbodyPosition, NearPlayer);
      if (closest == null)
      {
        Finish = true;
        return;
      }

      Move = -(Control.Rigidbody.position - Control.Agent.nextPosition).normalized;
      Control.Agent.SetDestination(closest.transform.position);
      Debug.DrawLine(Control.Rigidbody.position, closest.transform.position, Color.blue, 0, false);
    }

    public override bool CanSelect()
    {
      return CatControl.AliveCats.Count > 0 && base.CanSelect();
    }
  }
}