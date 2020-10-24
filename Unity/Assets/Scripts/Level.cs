using UnityEngine;
using UnityEngine.Assertions;

public class Level : MonoBehaviour
{
  public static Level Instance; 
    
  public Bounds LevelBounds;
  public Transform WinTargetPoint;

  void Awake()
  {
    Instance = this;
    Assert.IsNotNull(WinTargetPoint);
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = new Color(0, 1, 0, .9f);
    Gizmos.DrawWireCube(LevelBounds.center, LevelBounds.size);
  }
}