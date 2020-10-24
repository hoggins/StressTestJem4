using UnityEngine;

namespace BotStates
{
  public class BotStateKillPlayer : BotStateBase
  {
    private const float KillDistance = 50;
    private float _elapsed = 0f;
    private float _accuracy = 0f;
    
    public BotStateKillPlayer(BallBotControl control) : base(control)
    {
      StateDuration = 7f;
      StateDurationRandom = 4f;
      ChanceToSelect = 0.40f;
    }

    public override void OnEnter()
    {
      base.OnEnter();
      _elapsed = 0f;
      _accuracy = Random.Range(0, 1f);
      Control.Agent.ResetPath();
    }

    public override void Update()
    {
      _elapsed += Time.deltaTime;
      
      var targetPlayerPosition = Player.Instance.transform.position;
      
      Debug.DrawLine(Control.transform.position, targetPlayerPosition, Color.red, 0f, false);
      Move = -(Control.Rigidbody.position - targetPlayerPosition).normalized;


      if (Vector3.Distance(targetPlayerPosition, Control.transform.position) < 15f)
      {
        Control.GetComponent<BallSpeedUp>().Use(Move);
      }
    }

    public override bool CanSelect()
    {
      if (Player.Instance == null)
        return false;
      
      var playerPosition = GetTargetPositionWithPrediction();
      
      var distance = Vector3.Distance(Control.Rigidbody.position, playerPosition);
      var direction = -(Control.Rigidbody.position - playerPosition).normalized;

      if (distance > KillDistance)
        return false;
      
      if (!Physics.Raycast(new Ray(Control.transform.position, direction), out var hit, distance))
        return false;
      
      return hit.collider.gameObject.CompareTag("Player") && base.CanSelect();
    }
    
    
    private Vector3 GetTargetPositionWithPrediction()
    {
      var targetPosition = Player.Instance.transform.position;

      var ownerPosition = Control.transform.position;

      var predictionAmount = Mathf.Lerp(0.75f, 1.5f, (Mathf.Abs(Mathf.Sin(_elapsed))));

      var predictedPosition = GetPredictedPosition(targetPosition,
        ownerPosition,
        Player.Instance.Rigidbody.velocity * predictionAmount,
        Control.Rigidbody.velocity.magnitude);

      targetPosition = Vector3.Lerp(targetPosition, predictedPosition, _accuracy);

      return targetPosition;
    }
    
    
    private Vector3 GetPredictedPosition(Vector3 targetPosition,
      Vector3 shooterPosition,
      Vector3 targetVelocity,
      float projectileSpeed)
    {
      var displacement = targetPosition - shooterPosition;
      var targetMoveAngle = Vector3.Angle(-displacement, targetVelocity) * Mathf.Deg2Rad;
      if (targetVelocity.magnitude == 0 || targetVelocity.magnitude > projectileSpeed
          && Mathf.Sin(targetMoveAngle) / projectileSpeed > Mathf.Cos(targetMoveAngle) / targetVelocity.magnitude)
      {
        return targetPosition;
      }

      var shootAngle = Mathf.Asin(Mathf.Sin(targetMoveAngle) * targetVelocity.magnitude / projectileSpeed);
      return targetPosition + targetVelocity * displacement.magnitude
             / Mathf.Sin(Mathf.PI - targetMoveAngle - shootAngle) * Mathf.Sin(shootAngle) / targetVelocity.magnitude;
    }
  }
}