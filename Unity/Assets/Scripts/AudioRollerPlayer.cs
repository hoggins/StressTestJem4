using System;
using UnityEngine;

namespace DefaultNamespace
{
  [RequireComponent(typeof(AudioSource))]
  public class AudioRollerPlayer : MonoBehaviour
  {
    public AudioSource Source { get; set; }

    private void Awake()
    {
      Source = GetComponent<AudioSource>();
    }
  }
}