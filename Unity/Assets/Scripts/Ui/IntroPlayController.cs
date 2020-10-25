using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroPlayController : MonoBehaviour
{
  private VideoPlayer Player;

  private void Awake()
  {
    Player = GetComponent<VideoPlayer>();

    transform.SetParent(null);
    DontDestroyOnLoad(this);

    SceneManager.sceneLoaded += (scene, mode) =>
    {
      Player.targetCamera = Camera.main;
    };
  }

  public void PlayAndGo(Action hideUi)
  {
    var op = SceneManager.LoadSceneAsync(UnityContract.SceneGame, LoadSceneMode.Single);
    op.allowSceneActivation = false;

    Player.targetCamera = Camera.main;
    Player.seekCompleted += source => { Player.Play(); };

    Player.started += source => { StartCoroutine(DelayDisableUi(hideUi)); };
    Player.loopPointReached += source =>
    {
      op.allowSceneActivation = true;
      StartCoroutine(Fade(0.5f));
    };

    if (Player.isPrepared)
    {
      Player.frame = 1;
    }
    else
    {
      Player.Prepare();
      Player.prepareCompleted += source => { Player.frame = 1; };
    }
  }

  private IEnumerator DelayDisableUi(Action hideUi)
  {
    yield return null;
    yield return null;
    yield return null;
    yield return null;
    hideUi();
  }

  private IEnumerator Fade(float duration)
  {
    var from = 1;
    int target = 0;
    var t = 0f;
    while (t / duration <= 1f)
    {
      t += Time.deltaTime;
      Player.targetCameraAlpha = Mathf.Lerp(from, target, t / duration);
      yield return null;
    }

    Destroy(this);
  }
}