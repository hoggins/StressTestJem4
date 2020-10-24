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
  
  [NonSerialized]
  public int KillBonus = 1;
  [NonSerialized]
  public int DoubleBonus = 1;

  void Awake()
  {
    _ball = GetComponent<Ball>();
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

    yield return new WaitForSeconds(SpeedUpDuration);

    _ball.m_bonusMult = 1f;
    if (_speedCoroutine != null)
    {
      StopCoroutine(_speedCoroutine);
      _speedCoroutine = null;
    }
  }
  
  private IEnumerator StartKillBonus()
  {
    KillBonus = 2;

    yield return new WaitForSeconds(KillDuration);

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

    yield return new WaitForSeconds(DoubleDuration);

    DoubleBonus = 1;
    
    if (_doubleCoroutine != null)
    {
      StopCoroutine(_doubleCoroutine);
      _doubleCoroutine = null;
    }
  }

}