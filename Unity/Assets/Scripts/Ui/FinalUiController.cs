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

    // targetCollect = 150;
    // targetCleared = 350;
    // targetTime = TimeSpan.FromSeconds(300);

    StartCoroutine(AnimateTextNumber(targetCollect, CollectText, 1.5f, SosObjects[0], 100));
    StartCoroutine(AnimateTextNumber(targetCleared, ClearedText, 1.5f, SosObjects[1], 100));
    StartCoroutine(AnimateTextTime(targetTime, TimeText, 1.5f, SosObjects[2], 180));


      SosObjects[0].SetActive(targetCollect > 100);
      SosObjects[1].SetActive(targetCleared > 100);
      SosObjects[2].SetActive(targetTime < TimeSpan.FromSeconds(180));
  }

  public void HomeClick()
  {
    SceneManager.LoadScene(UnityContract.SceneStartMenu);
  }

  public void PlayClick()
  {
    SceneManager.LoadScene(UnityContract.SceneGame);
  }

  private IEnumerator AnimateTextNumber(int target, Text text, float duration, GameObject go, int goLimit)
  {
    var from = 0;
    var t = 0f;
    var lastVal = 0;
    while (t / duration <= 1f)
    {
      t += Time.deltaTime;
      var newVal = (int)Mathf.Lerp(@from, target, t / duration);
      text.text = newVal.ToString();

      // if (lastVal < goLimit && newVal >= goLimit)
        // go.SetActive(true);
      lastVal = newVal;

      yield return null;
    }

    text.text = target.ToString();
  }

  private IEnumerator AnimateTextTime(TimeSpan target, Text text, float duration, GameObject go, float goLimit)
  {
    var from = 0;
    var to = (float)target.TotalSeconds;
    var t = 0f;
    var lastVal = 0f;
    while (t / duration <= 1f)
    {
      t += Time.deltaTime;
      var sec = Mathf.Lerp(from, to, t / duration);
      var time = TimeSpan.FromSeconds(sec);
      SetTime(time);

      // if (lastVal > goLimit && sec <= goLimit)
        // go.SetActive(false);

      // if (lastVal < goLimit && sec >= goLimit)
        // go.SetActive(false);

      yield return null;
    }

    SetTime(target);

    void SetTime(TimeSpan time) => text.text = $"{time.Minutes}:{time.Seconds}";
  }
}