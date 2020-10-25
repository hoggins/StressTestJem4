using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartUiController : MonoBehaviour
{
  public IntroPlayController IntroController;

  public GameObject MainMenu;
  public GameObject HelpMenu;

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

  public void HelpClick()
  {
    MainMenu.SetActive(false);
    HelpMenu.SetActive(true);
  }

  public void HelpCloseClick()
  {
    MainMenu.SetActive(true);
    HelpMenu.SetActive(false);
  }
}