using UnityEngine;

namespace DefaultNamespace.CatStates
{
  public class CatStateRunAround : CatStateBase
  {
    private const float DirectionChangeTimeMin = 0.5f;
    private const float DirectionChangeTimeMax = 2.0f;
    private const float ForceMin = 0.5f;
    private const float ForceMax = 9f;

    private float _elapsed = 0f;
    private float _changetime;
    private Vector3 _direction;
    private float _currentForce;

    public CatStateRunAround(CatControl control) : base(control)
    {
      ChangeStateDurMin = 10;
      ChangeStateDurMax = 20;
    }

    public override void OnEnter()
    {
      base.OnEnter();
      Reset();
    }

    public override void Update()
    {
      base.Update();
      _elapsed += Time.deltaTime;

      if (_elapsed > _changetime)
      {
        Reset();
        OnEnter();
      }
    }

    private void Reset()
    {
      _changetime = Random.Range(DirectionChangeTimeMin, DirectionChangeTimeMax);
      var dir2d = Random.insideUnitCircle;
      _direction = new Vector3(dir2d.x, 0, dir2d.y).normalized;
      _currentForce = Random.Range(ForceMin, ForceMax);

      _elapsed = 0f;
    }

    public override void FixedUpdate()
    {
      base.FixedUpdate();

      var velocity = Control.Body.velocity; 
      
      var lerped = Vector3.Lerp(velocity, -_direction * _currentForce, Time.deltaTime * 3f);
      lerped.y = velocity.y;

      Control.Body.velocity = lerped; 
      
      Control.Body.MoveRotation(Quaternion.Lerp(Control.Body.rotation, Quaternion.LookRotation(_direction),
        Time.deltaTime * 5f));
    }
  }
}