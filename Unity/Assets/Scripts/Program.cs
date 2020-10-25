using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityContract
{
  public const string SceneStartMenu = "Scenes/MenuStartScene";
  public const string SceneFinalMenu = "Scenes/MenuFinalScene";
  public const string SceneGame = "Scenes/RollEnvironment01";
}

public class GameScore
{
  public int Taken { get; set; }
  public int Cleared { get; set; }
  public float StartSecond { get; set; }
  public float TotalSeconds { get; set; }
}

public class Program : MonoBehaviour
{
  public static Program Instance { get; private set; }

  public static GameScore Score = new GameScore();

  private void Awake()
  {
    if (Instance != null)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(this);

    Application.targetFrameRate = 30;
    //Screen.SetResolution(1980,1080, FullScreenMode.ExclusiveFullScreen);
  }

  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
  }
}