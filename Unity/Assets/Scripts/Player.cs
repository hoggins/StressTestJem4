using UnityEngine;

public class Player:MonoBehaviour
{
  public static Player Instance;
  
  void Awake()
  {
    Instance = this;
  }
}