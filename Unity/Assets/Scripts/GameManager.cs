using UnityEngine;

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
  
  void Awake()
  {
    Instance = this;
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