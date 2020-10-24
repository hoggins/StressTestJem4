using UnityEngine;

namespace DefaultNamespace.CatStates
{
  public class CatStateIdle : CatStateBase
  {
    public CatStateIdle(CatControl control) : base(control)
    {
      ChangeStateDurMin = 0.7f;
      ChangeStateDurMax = 3f;
    }
    
    private const float DirectionChangeTimeMin = 0.5f;
    private const float DirectionChangeTimeMax = 1.0f;

    private float _elapsed = 0f;
    private float _changetime;
    private Vector3 _direction;

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
      _elapsed = 0f;
    }

    public override void FixedUpdate()
    {
      base.FixedUpdate();

      Control.Body.MoveRotation(Quaternion.Lerp(Control.Body.rotation, Quaternion.LookRotation(_direction),
        Time.deltaTime * 2f));
    }
  }
}