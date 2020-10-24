using System;
using Controllers;
using UnityEngine;

namespace DefaultNamespace
{
  [RequireComponent(typeof(AudioSource))]
  public class AudioRollerPlayer : MonoBehaviour
  {
    public float minRollSpeed = 0.4f;
    public float maxRollSpeed = 8;

    public AudioSource DefaultSource;
    public AudioSource RailsSource;

    private bool IsPlayingRails;
    private Rigidbody _body;

    private Vector3 _lastVel;


    public float LastHitEnemyTime { get; set; }

    private void Awake()
    {
      _body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
      if (AudioController.Instance == null)
        return;
      UpdateRails();
      UpdateWallHit();
    }

    private void UpdateWallHit()
    {
      var newVel = _body.velocity;
      var timeSinceEnemyHit = Time.time - LastHitEnemyTime;
      if ((_lastVel - newVel).magnitude > 2
          && timeSinceEnemyHit > 0.3)
      {
        AudioController.Instance.PlayHitWall(DefaultSource);
        LastHitEnemyTime = Time.time;
      }
      _lastVel = newVel;
    }

    private void UpdateRails()
    {
      var moveMagnitude = _body.velocity.magnitude;
      var rails = RailsSource;
      if (moveMagnitude > minRollSpeed && !IsPlayingRails)
      {
        IsPlayingRails = true;
        rails.Play();
      }

      if (moveMagnitude <= minRollSpeed && IsPlayingRails)
      {
        IsPlayingRails = false;
        rails.Pause();
      }

      if (IsPlayingRails)
      {
        var vol = Mathf.Min(1f,moveMagnitude / maxRollSpeed);
        rails.volume = vol;
      }
    }
  }
}