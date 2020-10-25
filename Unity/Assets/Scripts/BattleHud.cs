using System.Linq;
using UnityEditor.Experimental.GraphView;
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
    
    
    
    private CatPlacer[] _botPlacers;

    private float _elapsedScale = 0f;
    private float _elapsedScale2 = 0f;

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
        var sin = Mathf.Abs(Mathf.Sin(_elapsedScale));
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
        var sin = Mathf.Abs(Mathf.Sin(_elapsedScale2));
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
    }
  }
}