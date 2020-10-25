using System;
using UnityEngine;

public class KillZone : MonoBehaviour
{
  private void Update()
  {
    if (transform.position.y < -100)
    {
      transform.position = new Vector3(0, 50, 0);
    }
  }
}