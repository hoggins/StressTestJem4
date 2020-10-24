namespace DefaultNamespace.CatStates
{
  public class CatStateBase
  {
    public CatControl Control { get; }
    
    public float ChangeStateDurMin = 3.0f;
    public float ChangeStateDurMax = 15.0f;

    public CatStateBase(CatControl control)
    {
      Control = control;
    }

    public virtual void OnEnter()
    {
      
    }
    
    public virtual void Update()
    {
    }

    public virtual void FixedUpdate()
    {
      
    }
  }
}