using System;
using UnityEngine;

public class Player:MonoBehaviour
{
  public static Player Instance;

  public float FovMin = 60;
  public float FovMax = 85;
  
  [NonSerialized]
  public Rigidbody Rigidbody;
  [NonSerialized]
  public BallSpeedUp SpeedUp;

  
  
  
  void Awake()
  {
    Instance = this;
    Rigidbody = GetComponent<Rigidbody>();
    SpeedUp = GetComponent<BallSpeedUp>();
  }


  private void Update()
  {
    
    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
    {
      var forward = CameraMain.Instance.transform.forward;
      SpeedUp.Use(forward);
    }

    var speed = Rigidbody.velocity.magnitude;
    CameraMain.Instance.fieldOfView = Mathf.Lerp(CameraMain.Instance.fieldOfView, Mathf.Lerp(FovMin, FovMax, speed / 20), Time.deltaTime * 1f);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("WinTrigger"))
    {
      GameManager.Instance.Win();
    }
  }
}