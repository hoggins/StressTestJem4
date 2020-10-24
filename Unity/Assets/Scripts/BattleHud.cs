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
    
    public Image DashCooldown;
    public Image JumpCooldown;
    
    
    
    private CatPlacer[] _botPlacers;

    private void Awake()
    {
      Instance = this;

      _botPlacers = FindObjectsOfType<CatPlacer>();
    }

    private void Update()
    {
      var catPlacer = Player.Instance.GetComponent<CatPlacer>();
      CatScoreText.text = catPlacer.CatsAttached + " / " + GameManager.Instance.WinScore;

      var maxCats = _botPlacers.Max(x => x.CatsAttached);
      EnemyBestScoreText.text = maxCats + " / " + GameManager.Instance.WinScore;
      
      var ballSpeedUp = Player.Instance.GetComponent<BallSpeedUp>();
      DashCooldown.fillAmount = ballSpeedUp.Fill;
      
      var component = Player.Instance.GetComponent<Ball>();
      JumpCooldown.fillAmount = component.Fill;
    }
  }
}