using UnityEngine;

namespace BotStates
{
  public class BotStateKillPlayer : BotStateBase
  {
    private const float KillDistance = 50;
    
    public BotStateKillPlayer(BallBotControl control) : base(control)
    {
      StateDuration = 7f;
      StateDurationRandom = 3f;
      ChanceToSelect = 0.25f;
    }

    public override bool CanSelect()
    {
      if (Player.Instance == null)
        return false;
      
      var playerPosition = Player.Instance.transform.position;
      var distance = Vector3.Distance(Control.Rigidbody.position, playerPosition);
      var direction = -(Control.Rigidbody.position - playerPosition).normalized;

      if (distance > KillDistance)
        return false;
      
      if (!Physics.Raycast(new Ray(Control.transform.position, direction), out var hit, distance))
        return false;
      
      return hit.collider.gameObject.CompareTag("Player") && base.CanSelect();
    }
  }
}