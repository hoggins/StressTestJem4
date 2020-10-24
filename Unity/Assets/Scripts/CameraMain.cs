using UnityEngine;

public class CameraMain : MonoBehaviour
{
  public static Camera Instance;
  void Awake()
  {
    Instance = GetComponent<Camera>();
  }
    
}