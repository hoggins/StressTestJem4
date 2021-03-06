﻿using UnityEngine;

namespace DefaultNamespace.CatStates
{
  public class CatStateJump : CatStateBase
  {
    private const float JumpDelay = 3f;
    private float _elapsed = 0f;
    
    public CatStateJump(CatControl control) : base(control)
    {
      
      ChangeStateDurMin = 3f;
      ChangeStateDurMax = 9f;
    }

    public override void Update()
    {
      _elapsed += Time.deltaTime;
      if (_elapsed > JumpDelay)
      {
        Control.Body.AddForce(Vector3.up * 400f + Random.insideUnitSphere*40);
        _elapsed = 0f;
      }

      if(Control.Body.velocity.magnitude > 0.01f)
        Control.Body.MoveRotation(Quaternion.Lerp(Control.Body.rotation, Quaternion.LookRotation(Control.Body.velocity.normalized),
          Time.deltaTime * 2f));
    }
  }
}