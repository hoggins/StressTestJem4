using System;
using UnityEngine;

public readonly struct GizmosColor : IDisposable
{
  private readonly Color _oldColor;

  public GizmosColor(Color red)
  {
    _oldColor = Gizmos.color;
    Gizmos.color = red;
  }

  public void Dispose()
  {
    Gizmos.color = _oldColor;
  }
}