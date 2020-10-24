using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace.Bonuses
{
  public class Bonus : MonoBehaviour
  {
    public float RespawnDuration = 30f;
    private Collider _collider;
    private Renderer _renderer;

    private void Awake()
    {
      _collider = GetComponent<Collider>();
      _renderer = GetComponent<Renderer>();
    }

    public void WaitToRespawn()
    {
      _collider.enabled = false;
      foreach (var renderer in GetComponentsInChildren<Renderer>())
      {
        renderer.enabled = false;
      }
      _renderer.enabled = false;
      StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
      yield return new WaitForSeconds(RespawnDuration);
      _collider.enabled = true;
      
      foreach (var renderer in GetComponentsInChildren<Renderer>(true))
      {
        renderer.enabled = true;
      }
    }
  }
}