using UnityEngine;

namespace BotStates
{
  public class BotStateBase
  {
    public BallBotControl Control { get; }
    public float StateDuration = 10f;
    public float StateDurationRandom = 0;
    
    protected Vector3 _move;

    public BotStateBase(BallBotControl control)
    {
      Control = control;
    }

    public virtual bool CanSelect()
    {
      return true;
    }

    public virtual void OnEnter()
    {
      
    }
    
    public virtual void Update()
    {
      
    }

    public virtual void LateUpdate()
    {
      
    }

    public virtual void FixedUpdate()
    {
      Control.Ball.Move(_move, false);
    }
  }
}