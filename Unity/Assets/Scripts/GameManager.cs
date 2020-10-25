using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Cameras;

public class GameManager:MonoBehaviour
{
  public static GameManager Instance;

  public enum WinState
  {
    None,
    Win,
    Lose
  }

  public WinState CurrentWinState = WinState.None;
  public int WinScore = 30;


  public static readonly HashSet<CatControl> AliveNearPlayer = new HashSet<CatControl>();

  void Awake()
  {
    Instance = this;

    Program.Score = new GameScore();
    Program.Score.StartSecond = Time.time;
  }

  private void Update()
  {
    AliveNearPlayer.Clear();
    foreach (var aliveCat in CatControl.AliveCats)
    {
      if (Vector3.Distance(Player.Instance.transform.position, aliveCat.transform.position) < 60)
      {
        AliveNearPlayer.Add(aliveCat);
      }
    }

    if (Input.GetKeyDown(KeyCode.V))
      CompleteGame();
  }

  private static void CompleteGame()
  {
    Program.Score.TotalSeconds = Time.time - Program.Score.StartSecond;
    SceneManager.LoadScene(UnityContract.SceneFinalMenu, LoadSceneMode.Single);
  }

  public void Win()
  {
    if(CurrentWinState != WinState.None)
      return;

    CurrentWinState = WinState.Win;
    Debug.Log("WIN");
  }

  public void Lose()
  {
    if(CurrentWinState != WinState.None)
      return;

    CurrentWinState = WinState.Lose;
    Debug.Log("LOSE");
  }
}