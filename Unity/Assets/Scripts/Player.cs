using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    if(Input.GetKeyDown(KeyCode.F))
    {
      CameraMain.Instance.Shaker.ShakeOnce(11.0f, 3.0f, 0, 1.2f);
    }

    if (Input.GetKeyDown(KeyCode.T))
    {
      Instance.GetComponent<CatPlacer>().AddLayer(4);
    }

    var speed = Rigidbody.velocity.magnitude;
    CameraMain.Instance.Cam.fieldOfView = Mathf.Lerp(CameraMain.Instance.Cam.fieldOfView, Mathf.Lerp(FovMin, FovMax, speed / 20), Time.deltaTime * 1f);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("WinTrigger"))
    {
      GameManager.Instance.Win();
    }
  }
}