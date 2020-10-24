using UnityEngine;

namespace BotStates
{
  public class BotStateBase
  {
    public BallBotControl Control { get; }
    public float StateDuration = 10f;
    public float StateDurationRandom = 0;
    public float ChanceToSelect = 1f;
    public bool Finish;
    
    protected Vector3 Move;

    public BotStateBase(BallBotControl control)
    {
      Control = control;
    }

    public virtual bool CanSelect()
    {
      return Random.Range(0, 1f) < ChanceToSelect;
    }

    public virtual void OnEnter()
    {
      Finish = false;

    }
    
    public virtual void Update()
    {
      
    }

    public virtual void LateUpdate()
    {
      
    }

    public virtual void FixedUpdate()
    {
      Control.Ball.Move(Move, Control.Jump);
    }
  }
}