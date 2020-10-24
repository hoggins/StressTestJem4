using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Ball;

namespace DefaultNamespace
{
  public class BattleHud : MonoBehaviour
  {
    public static BattleHud Instance;
    public Text CatScoreText;
    public Image DashCooldown;
    public Image JumpCooldown;
    
    private void Awake()
    {
      Instance = this;
    }

    private void Update()
    {
      var catPlacer = Player.Instance.GetComponent<CatPlacer>();
      CatScoreText.text = catPlacer.CatsAttached.ToString() + " x";
      
      var ballSpeedUp = Player.Instance.GetComponent<BallSpeedUp>();
      DashCooldown.fillAmount = ballSpeedUp.Fill;
      
      var component = Player.Instance.GetComponent<Ball>();
      JumpCooldown.fillAmount = component.Fill;
    }
  }
}