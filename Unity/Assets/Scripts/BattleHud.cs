using UnityEngine;
using UnityEngine.UI;

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
      CatScoreText.text = Player.Instance.GetComponent<CatPlacer>().CatsAttached.ToString() + " x";
      // DashCooldown.fillAmount = Player.Instance.GetComponent<BallSpeedUp>().Fill;
    }
  }
}