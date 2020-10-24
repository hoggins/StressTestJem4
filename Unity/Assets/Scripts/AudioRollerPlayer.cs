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

    private bool IsPlayingRails;
    private Rigidbody _body;
    public AudioSource Source { get; set; }

    private void Awake()
    {
      Source = GetComponent<AudioSource>();
      _body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
      if (AudioController.Instance == null)
        return;
      UpdateRails();
    }

    private void UpdateRails()
    {
      var moveMagnitude = _body.velocity.magnitude;
      var rails = AudioController.Instance.PlayerRails;
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