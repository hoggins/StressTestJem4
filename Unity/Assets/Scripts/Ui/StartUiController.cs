using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartUiController : MonoBehaviour
{
  public VideoPlayer Player;

  public void OnPlayClick()
  {
    var op = SceneManager.LoadSceneAsync(UnityContract.SceneGame, LoadSceneMode.Single);
    op.allowSceneActivation = false;

    Player.targetCamera = Camera.main;
    Player.seekCompleted += source =>
    {
      Player.Play();
    };

    Player.started += source => { StartCoroutine(DelayDisableUi()); };
    Player.loopPointReached += source =>
    {
      op.allowSceneActivation = true;
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

  private IEnumerator DelayDisableUi()
  {
    yield return null;
    yield return null;
    yield return null;
    yield return null;
    gameObject.SetActive(false);
  }

  public void OnExitClick()
  {
    Application.Quit();
  }

  public void OnSettingsClick()
  {

  }

  public void OnSoundClick()
  {
    AudioListener.pause = !AudioListener.pause;
  }
}