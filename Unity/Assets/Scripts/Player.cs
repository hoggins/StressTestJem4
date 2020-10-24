using UnityEngine;

public class Player:MonoBehaviour
{
  public Player Instance;
  
  void Awake()
  {
    Instance = this;
  }
}