using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Ball;

namespace DefaultNamespace
{
  public class BattleHud : MonoBehaviour
  {
    public static BattleHud Instance;

    public Text CatScoreText;
    public Text EnemyBestScoreText;

    public Transform EnemyScoreRoot;
    public Transform ScoreRoot;

    public Image DashCooldown;
    public Image JumpCooldown;


    public Image EndImage;
    public Text EndText;
    
    
    private CatPlacer[] _botPlacers;

    private float _elapsedScale = 0f;
    private float _elapsedScale2 = 0f;
    private float _elapsedRot = 0f;

    private void Awake()
    {
      Instance = this;

      _botPlacers = FindObjectsOfType<CatPlacer>().Where(x => x.GetComponent<Player>() == null).ToArray();
    }

    private void Update()
    {
      var catPlacer = Player.Instance.GetComponent<CatPlacer>();
      var catPlacerCatsAttached = catPlacer.CatsAttached;
      CatScoreText.text = catPlacerCatsAttached + " / " + GameManager.Instance.WinScore;

      var maxCats = _botPlacers.Max(x => x.CatsAttached);
      EnemyBestScoreText.text = maxCats + " / " + GameManager.Instance.WinScore;

      var ballSpeedUp = Player.Instance.GetComponent<BallSpeedUp>();
      DashCooldown.fillAmount = ballSpeedUp.Fill;

      var component = Player.Instance.GetComponent<Ball>();
      JumpCooldown.fillAmount = component.Fill;

      if (maxCats >= GameManager.Instance.WinScore)
      {
        _elapsedScale += Time.deltaTime * 3;
        var sin = 1f + Mathf.Abs( Mathf.Sin(_elapsedScale))*0.25f;
        EnemyScoreRoot.transform.localScale = Vector3.Lerp(EnemyScoreRoot.transform.localScale,
          new Vector3(sin, sin, sin), Time.deltaTime*3f);
      }
      else
      {
        EnemyScoreRoot.transform.localScale =
          Vector3.Lerp(EnemyScoreRoot.transform.localScale, new Vector3(1, 1, 1), Time.deltaTime);
        _elapsedScale = 0;
      }

      if (catPlacerCatsAttached >= GameManager.Instance.WinScore)
      {
        _elapsedScale2 += Time.deltaTime * 3f;
        var sin = 1f + Mathf.Abs(Mathf.Sin(_elapsedScale2)) * 0.25f;
        CatScoreText.transform.localScale = Vector3.Lerp(CatScoreText.transform.localScale,
          new Vector3(sin, sin, sin), Time.deltaTime*3f);
      }
      else
      {
        CatScoreText.transform.localScale =
          Vector3.Lerp(CatScoreText.transform.localScale, new Vector3(1, 1, 1), Time.deltaTime);
        _elapsedScale2 = 0;
      }


      if (maxCats >= GameManager.Instance.WinScore || catPlacerCatsAttached >= GameManager.Instance.WinScore)
      {
        Portal.Instance.Active();
      }
      else
      {
        Portal.Instance.Deactivate();
      }

      if (GameManager.Instance.CurrentWinState != GameManager.WinState.None)
      {
        _elapsedRot += Time.deltaTime;
        var c = EndImage.color;
        c.a = Mathf.Lerp(c.a, 1f, Time.deltaTime*10f);
        EndImage.color = c;

        var c2 = EndText.color;
        var canHideUi = GameManager.Instance.CanHideUi;
        c2.a = Mathf.Lerp(c2.a, canHideUi ? 0f : 1f, Time.deltaTime*(canHideUi ? 10f : 1.5f));
        EndText.color = c2;

        var angle = EndText.transform.localEulerAngles;
        EndText.transform.localEulerAngles = Vector3.Lerp(angle, new Vector3(0, 0, Mathf.Sin(_elapsedRot*2) * 15), Time.deltaTime*3f);
      }

      if (GameManager.Instance.CurrentWinState == GameManager.WinState.Win)
      {
        EndText.text = "Победа!";
      }
      else if (GameManager.Instance.CurrentWinState == GameManager.WinState.Lose)
      {
        EndText.text = "Сосиски сожрали другие коты...";
      }
    }
  }
}