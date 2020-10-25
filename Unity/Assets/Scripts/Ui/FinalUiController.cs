using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalUiController : MonoBehaviour
{
  public GameObject[] SosObjects;

  public Text CollectText;

  public Text ClearedText;

  public Text TimeText;

  void Start()
  {
    var targetCollect = Program.Score.Taken;
    var targetCleared = Program.Score.Cleared;
    var targetTime = TimeSpan.FromSeconds(Program.Score.TotalSeconds);

    StartCoroutine(AnimateTextNumber(targetCollect, CollectText, 2));
    StartCoroutine(AnimateTextNumber(targetCleared, ClearedText, 2));
    StartCoroutine(AnimateTextTime(targetTime, TimeText, 2));
  }

  public void HomeClick()
  {
    SceneManager.LoadScene(UnityContract.SceneStartMenu);
  }

  public void PlayClick()
  {
    SceneManager.LoadScene(UnityContract.SceneGame);
  }

  private IEnumerator AnimateTextNumber(int target, Text text, int duration)
  {
    var from = 0;
    var t = 0f;
    while (t / duration <= 1f)
    {
      t += Time.deltaTime;
      text.text = ((int)Mathf.Lerp(from, target, t / duration)).ToString();
      yield return null;
    }

    text.text = target.ToString();
  }

  private IEnumerator AnimateTextTime(TimeSpan target, Text text, int duration)
  {
    var from = 0;
    var to = (float)target.TotalSeconds;
    var t = 0f;
    while (t / duration <= 1f)
    {
      t += Time.deltaTime;
      var sec = Mathf.Lerp(from, to, t / duration);
      var time = TimeSpan.FromSeconds(sec);
      SetTime(time);
      yield return null;
    }

    SetTime(target);

    void SetTime(TimeSpan time) => text.text = $"{time.Minutes}:{time.Seconds}";
  }
}