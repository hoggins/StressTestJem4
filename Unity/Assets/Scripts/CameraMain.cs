using EZCameraShake;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class CameraMain : MonoBehaviour
{
  public static CameraMain Instance;
  public Camera Cam;
  
  public CameraShaker Shaker { get; set; }
  public FreeLookCam FreeLook { get; set; }
  
  void Awake()
  {
    Instance = this;
    Cam = GetComponent<Camera>();
    FreeLook = GetComponentInParent<FreeLookCam>();
    
    Shaker = FreeLook.Shaker;
  }
}