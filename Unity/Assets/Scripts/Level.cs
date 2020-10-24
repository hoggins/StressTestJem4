using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace DefaultNamespace
{
  public class Level : MonoBehaviour
  {
    public Bounds LevelBounds;
    public static Level Instance; 

    void Awake()
    {
      Instance = this;
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = new Color(0, 1, 0, .9f);
      Gizmos.DrawWireCube(LevelBounds.center, LevelBounds.size);
    }
  }
}