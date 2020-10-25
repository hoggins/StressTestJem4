using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartUiController : MonoBehaviour
{
  public IntroPlayController IntroController;

  public void OnPlayClick()
  {
    IntroController.PlayAndGo(()=>gameObject.SetActive(false));
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