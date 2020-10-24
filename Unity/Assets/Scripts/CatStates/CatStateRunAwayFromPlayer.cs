using UnityEngine;

namespace DefaultNamespace.CatStates
{
  public class CatStateRunAwayFromPlayer : CatStateBase
  {
    private Vector3 _randomDirection;
    private Vector3 _randomPosOffset;

    private const float JumpDelayMin = 4f;
    private const float JumpDelayMax = 8f;
    
    private float _currentJumpDelay;
    private float _elapsed = 0f;
    
    public CatStateRunAwayFromPlayer(CatControl control) : base(control)
    {
      ChangeStateDurMin = 7;
      ChangeStateDurMax = 20;
    }

    public override void OnEnter()
    {
      base.OnEnter();

      Reset();
    }

    private void Reset()
    {
      var dir = Random.insideUnitCircle.normalized;
      _randomDirection = new Vector3(dir.x, 0, dir.y);
      _randomPosOffset = new Vector3(dir.x, 0, dir.y) * Random.Range(0, 10);
      _currentJumpDelay = Random.Range(JumpDelayMin, JumpDelayMax);
    }

    public override void Update()
    {
      base.Update();
      
      _elapsed += Time.deltaTime;
      if (_elapsed > _currentJumpDelay)
      {
        Reset();
        
        Control.Body.AddForce(Vector3.up * 400f + Random.insideUnitSphere*40);
        _elapsed = 0f;
      }
    }

    public override void FixedUpdate()
    {
      base.FixedUpdate();
      
      if(Vector3.Distance(Control.transform.position, Player.Instance.transform.position) > 60)
        return;

      var velocity = Control.Body.velocity;

      var directionToPlayer = (Player.Instance.Rigidbody.position - Control.Body.position + _randomPosOffset).normalized;
      var lerped = Vector3.Lerp(velocity, -directionToPlayer * 10 + _randomDirection, Time.deltaTime * 3f);
      lerped.y = velocity.y;

      Control.Body.velocity = lerped; 
      
      Control.Body.MoveRotation(Quaternion.Lerp(Control.Body.rotation, Quaternion.LookRotation(directionToPlayer),
        Time.deltaTime * 5f));
    }
  }
}