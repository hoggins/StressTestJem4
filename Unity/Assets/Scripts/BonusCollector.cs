using System;
using System.Collections;
using DefaultNamespace.Bonuses;
using UnityEngine;
using UnityStandardAssets.Vehicles.Ball;

public class BonusCollector : MonoBehaviour
{
  private Ball _ball;
  
  public float SpeedUpPower = 2;
  public float KillPower = 2;
  public float DoublePower = 2;
  
  public float SpeedUpDuration = 10;
  public float KillDuration = 10;
  public float DoubleDuration = 10;
  
  private Coroutine _speedCoroutine;
  private Coroutine _killCoroutine;
  private Coroutine _doubleCoroutine;

  public GameObject SpeedUpEffect;
  public GameObject KillEffect;
  public GameObject DoubleBonusEffect;
  
  [NonSerialized]
  public int KillBonus = 1;
  [NonSerialized]
  public int DoubleBonus = 1;

  void Awake()
  {
    _ball = GetComponent<Ball>();
    SpeedUpEffect.SetActive(false);
    KillEffect.SetActive(false);
    DoubleBonusEffect.SetActive(false);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("SpeedBonus"))
    {
      if (_speedCoroutine != null)
      {
        StopCoroutine(_speedCoroutine);
        _speedCoroutine = null;
      }

      _speedCoroutine = StartCoroutine(StartSpeedUpBonus());
      Collect(other);
    }
    else if (other.CompareTag("KillBonus"))
    {
      if (_killCoroutine != null)
      {
        StopCoroutine(_killCoroutine);
        _killCoroutine = null;
      }

      _killCoroutine = StartCoroutine(StartKillBonus());
      Collect(other);
    }
    else if (other.CompareTag("DoubleTakeBonus"))
    {
      if (_doubleCoroutine != null)
      {
        StopCoroutine(_doubleCoroutine);
        _doubleCoroutine = null;
      }

      _doubleCoroutine = StartCoroutine(StartDoubleBonus());
      Collect(other);
    }
  }

  private void Collect(Collider other)
  {
    other.GetComponent<Bonus>().WaitToRespawn();
  }

  private IEnumerator StartSpeedUpBonus()
  {
    _ball.m_bonusMult = SpeedUpPower;
    ActivateEffect(SpeedUpEffect, true);

    yield return new WaitForSeconds(SpeedUpDuration);
    ActivateEffect(SpeedUpEffect, false);

    _ball.m_bonusMult = 1f;
    SpeedUpEffect.SetActive(false);
    if (_speedCoroutine != null)
    {
      StopCoroutine(_speedCoroutine);
      _speedCoroutine = null;
    }
  }
  
  private IEnumerator StartKillBonus()
  {
    KillBonus = 2;

    ActivateEffect(KillEffect, true);
    yield return new WaitForSeconds(KillDuration);
    ActivateEffect(KillEffect, false);

    KillBonus = 1;
    
    if (_killCoroutine != null)
    {
      StopCoroutine(_killCoroutine);
      _killCoroutine = null;
    }
  }
  
  private IEnumerator StartDoubleBonus()
  {
    DoubleBonus = 2;

    ActivateEffect(DoubleBonusEffect, true);
    yield return new WaitForSeconds(DoubleDuration);
    ActivateEffect(DoubleBonusEffect, false);

    DoubleBonus = 1;
    
    if (_doubleCoroutine != null)
    {
      StopCoroutine(_doubleCoroutine);
      _doubleCoroutine = null;
    }
  }

  private void ActivateEffect(GameObject go, bool active)
  {
    go.gameObject.SetActive(active);
  }
}