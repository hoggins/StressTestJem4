using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Controllers
{
  public class AudioController : MonoBehaviour
  {
    public AudioClip[] HitEnemy;
    public AudioClip[] RandomPhrases;

    public AudioSource Player;
    public AudioSource Music;
    public AudioSource MusicMenu;

    public static AudioController Instance;

    private void Awake()
    {
      if (Instance != null)
      {
        Destroy(gameObject);
        return;
      }

      Instance = this;
      DontDestroyOnLoad(this);
    }

    public void PlayHitEnemy(AudioSource audioSource)
    {
      PlayRandom(audioSource, HitEnemy);
    }


    private Coroutine _musicCoroutine;
    private bool _isPlayingMenuMusic;

    public void PlayMusic()
    {
      Music.Play();
      Music.volume = 0f;

      if (_musicCoroutine != null)
        StopCoroutine(_musicCoroutine);

      _musicCoroutine = StartCoroutine(ChangeVolumeTo(Music, 1f, 3f));
    }

    public void StopMusic()
    {
      if (_musicCoroutine != null)
        StopCoroutine(_musicCoroutine);
      _musicCoroutine = StartCoroutine(ChangeVolumeTo(Music, 0f));
    }

    private Coroutine _musicMenuCoroutine;

    public void PlayMusicMenu()
    {
      if (_isPlayingMenuMusic)
        return;
      _isPlayingMenuMusic = true;

      MusicMenu.Play();
      MusicMenu.volume = 0f;
      if (_musicMenuCoroutine != null)
        StopCoroutine(_musicMenuCoroutine);
      _musicMenuCoroutine = StartCoroutine(ChangeVolumeTo(MusicMenu, 1f));
    }

    public void StopMusicMenu()
    {
      if (!_isPlayingMenuMusic)
        return;
      _isPlayingMenuMusic = false;

      if (_musicMenuCoroutine != null)
        StopCoroutine(_musicMenuCoroutine);
      _musicMenuCoroutine = StartCoroutine(ChangeVolumeTo(MusicMenu, 0f, 0.3f));
    }


    private IEnumerator ChangeVolumeTo(AudioSource source, float to, float duration = 1f)
    {
      var from = source.volume;
      var t = 0f;
      while (t / duration <= 1f)
      {
        t += Time.deltaTime;
        source.volume = Mathf.Lerp(from, to, t / duration);
        yield return null;
      }

      if (source.volume == 0)
        source.Stop();
    }

    private void PlayRandom(AudioSource source, AudioClip[] clips)
    {
      var phrase = clips[UnityEngine.Random.Range(0, clips.Length)];
      source.PlayOneShot(phrase);
    }
  }
}