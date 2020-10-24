using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

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
  
  
  public static readonly HashSet<CatControl> AliveNearPlayer = new HashSet<CatControl>();
  
  void Awake()
  {
    Instance = this;
  }

  private void Update()
  {
    AliveNearPlayer.Clear();
    foreach (var aliveCat in CatControl.AliveCats)
    {
      if (Vector3.Distance(Player.Instance.transform.position, aliveCat.transform.position) < 70)
      {
        AliveNearPlayer.Add(aliveCat);
      }
    }
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