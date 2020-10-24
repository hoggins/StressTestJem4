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
}