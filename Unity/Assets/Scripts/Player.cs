using System;
using UnityEngine;

public class Player:MonoBehaviour
{
  public static Player Instance;

  public Rigidbody Rigidbody;
  
  void Awake()
  {
    Instance = this;
    Rigidbody = GetComponent<Rigidbody>();
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("WinTrigger"))
    {
      GameManager.Instance.Win();
    }
  }
}