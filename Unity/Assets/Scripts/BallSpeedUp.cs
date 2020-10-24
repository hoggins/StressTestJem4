using System;
using UnityEngine;
using UnityStandardAssets.Vehicles.Ball;

public class BallSpeedUp : MonoBehaviour
{
  public bool Speeding { get; }
  public bool TryingToSpeed { get; private set; }
  public float SpeedPercent => _speedBudget / SpeedDuration;

  public bool Cooldown => _cooldown;

  public float SpeedDuration = 2.0f;
  public float SpeedCooldown = 2.0f;
  public float SpeedMult = 2.0f;
  public float TorgueBonus = 3.0f;

  private bool _cooldown;
  private float _speedBudget;
  private Ball _ball;

  void Awake()
  {
    _ball = GetComponent<Ball>();
  }
  public void Use()
  {
    TryingToSpeed = true;
  }

  public void Stop()
  {
    TryingToSpeed = false;
  }

  private void Update()
  {
    _ball.m_MoveRunBonus = 1f;
    _ball.m_TorgueBonus = 1f;
    
    if (TryingToSpeed && !_cooldown)
    {
        _speedBudget -= Time.deltaTime;
        if (_speedBudget > 0)
        {
          _ball.m_MoveRunBonus = SpeedMult;
          _ball.m_TorgueBonus = TorgueBonus;
        }
        else
        {
          _cooldown = true;
        }
    }
    else
    {
      _speedBudget += Time.deltaTime*(SpeedDuration/SpeedCooldown);
      if (_speedBudget > SpeedDuration)
      {
        _speedBudget = SpeedDuration;
        _cooldown = false;
      }
    }
  }
}