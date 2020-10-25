using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EZCameraShake
{
  public enum CameraShakeState
  {
    FadingIn,
    FadingOut,
    Sustained,
    Inactive
  }

  public enum CameraShakeKind
  {
    PerlinNoise,
    Damped
  }

  public class CameraShakeInstance
  {
    /// <summary>
    /// The intensity of the shake. It is recommended that you use ScaleMagnitude to alter the magnitude of a shake.
    /// </summary>
    public float Magnitude;

    /// <summary>
    /// Roughness of the shake. It is recommended that you use ScaleRoughness to alter the roughness of a shake.
    /// </summary>
    public float Roughness;

    /// <summary>
    /// How much influence this shake has over the local position axes of the camera.
    /// </summary>
    public Vector3 PositionInfluence;

    /// <summary>
    /// How much influence this shake has over the local rotation axes of the camera.
    /// </summary>
    public Vector3 RotationInfluence;
    public Vector3 RotationInfluence2;

    /// <summary>
    /// Should this shake be removed from the CameraShakeInstance list when not active?
    /// </summary>
    public bool DeleteOnInactive = true;


    public float FadeOutDuration;
    public float FadeInDuration;

    public CameraShakeKind PositionShakeKind;

    public CameraShakeKind RotationShakeKind;
    public CameraShakeKind RotationShakeKind2;


    private float _roughMod = 1, _magnMod = 1;

    private bool _sustain;
    private float _currentFadeTime;
    private float _tick = 0;
    private Vector3 _amt;

    public CameraShakeInstance(CameraShakeSettings settings)
      :this(settings.Magnitude, settings.Roughness, settings.FadeInDuration, settings.FadeOutDuration)
    {
      PositionInfluence = settings.PositionInfluence;
      RotationInfluence = settings.RotationInfluence;
      RotationInfluence2 = settings.RotationInfluence2;

      PositionShakeKind = settings.PositionKind;
      RotationShakeKind = settings.RotationKind;
      RotationShakeKind2 = settings.RotationKind2;
    }

    /// <summary>
    /// Will create a new instance that will shake once and fade over the given number of seconds.
    /// </summary>
    /// <param name="magnitude">The intensity of the shake.</param>
    /// <param name="fadeOutTime">How long, in seconds, to fade out the shake.</param>
    /// <param name="roughness">Roughness of the shake. Lower values are smoother, higher values are more jarring.</param>
    public CameraShakeInstance(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
    {
      this.Magnitude = magnitude;
      FadeOutDuration = fadeOutTime;
      FadeInDuration = fadeInTime;
      this.Roughness = roughness;
      if (fadeInTime > 0)
      {
        _sustain = true;
        _currentFadeTime = 0;
      }
      else
      {
        _sustain = false;
        _currentFadeTime = 1;
      }

      _tick = Random.Range(-100, 100);
    }

    /// <summary>
    /// Will create a new instance that will start a sustained shake.
    /// </summary>
    /// <param name="magnitude">The intensity of the shake.</param>
    /// <param name="roughness">Roughness of the shake. Lower values are smoother, higher values are more jarring.</param>
    public CameraShakeInstance(float magnitude, float roughness)
    {
      this.Magnitude = magnitude;
      this.Roughness = roughness;
      _sustain = true;

      _tick = Random.Range(-100, 100);
    }

    public Vector3 UpdateShake(CameraShakeKind shakeKind)
    {
      UpdateTime();

      switch (shakeKind)
      {
        case CameraShakeKind.PerlinNoise:
          return UpdatePerlinShake();
        case CameraShakeKind.Damped:
          return UpdateSpringShake();
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void UpdateTime()
    {
      if (FadeInDuration > 0 && _sustain)
      {
        if (_currentFadeTime < 1)
          _currentFadeTime += Time.deltaTime/FadeInDuration;
        else if (FadeOutDuration > 0)
          _sustain = false;
      }

      if (!_sustain)
        _currentFadeTime -= Time.deltaTime/FadeOutDuration;

      if (_sustain)
        _tick += Time.deltaTime*Roughness*_roughMod;
      else
        _tick += Time.deltaTime*Roughness*_roughMod*_currentFadeTime;
    }

    private Vector3 UpdatePerlinShake()
    {
      _amt.x = Mathf.PerlinNoise(_tick, 0) - 0.5f;
      _amt.y = Mathf.PerlinNoise(0, _tick) - 0.5f;
      _amt.z = Mathf.PerlinNoise(_tick, _tick) - 0.5f;

      return _amt*Magnitude*_magnMod*_currentFadeTime;
    }

    private Vector3 UpdateSpringShake()
    {
      _amt = Vector3.one;
      return _amt * Magnitude * _magnMod * _currentFadeTime;
    }

    /// <summary>
    /// Starts a fade out over the given number of seconds.
    /// </summary>
    /// <param name="fadeOutTime">The duration, in seconds, of the fade out.</param>
    public void StartFadeOut(float fadeOutTime)
    {
      if (fadeOutTime == 0)
        _currentFadeTime = 0;

      FadeOutDuration = fadeOutTime;
      FadeInDuration = 0;
      _sustain = false;
    }

    /// <summary>
    /// Starts a fade in over the given number of seconds.
    /// </summary>
    /// <param name="fadeInTime">The duration, in seconds, of the fade in.</param>
    public void StartFadeIn(float fadeInTime)
    {
      if (fadeInTime == 0)
        _currentFadeTime = 1;

      FadeInDuration = fadeInTime;
      FadeOutDuration = 0;
      _sustain = true;
    }

    /// <summary>
    /// Scales this shake's roughness while preserving the initial Roughness.
    /// </summary>
    public float ScaleRoughness
    {
      get { return _roughMod; }
      set { _roughMod = value; }
    }

    /// <summary>
    /// Scales this shake's magnitude while preserving the initial Magnitude.
    /// </summary>
    public float ScaleMagnitude
    {
      get { return _magnMod; }
      set { _magnMod = value; }
    }

    /// <summary>
    /// A normalized value (about 0 to about 1) that represents the current level of intensity.
    /// </summary>
    public float NormalizedFadeTime
    {
      get { return _currentFadeTime; }
    }

    bool IsShaking
    {
      get { return _currentFadeTime > 0 || _sustain; }
    }

    bool IsFadingOut
    {
      get { return !_sustain && _currentFadeTime > 0; }
    }

    bool IsFadingIn
    {
      get { return _currentFadeTime < 1 && _sustain && FadeInDuration > 0; }
    }

    /// <summary>
    /// Gets the current state of the shake.
    /// </summary>
    public CameraShakeState CurrentState
    {
      get
      {
        if (IsFadingIn)
          return CameraShakeState.FadingIn;
        if (IsFadingOut)
          return CameraShakeState.FadingOut;
        if (IsShaking)
          return CameraShakeState.Sustained;

        return CameraShakeState.Inactive;
      }
    }

    public CameraShakeInstance Clone()
    {
      return new CameraShakeInstance(Magnitude, Roughness, FadeInDuration, FadeOutDuration)
      {
        PositionInfluence = PositionInfluence,
        RotationInfluence = RotationInfluence,
        RotationInfluence2 = RotationInfluence2,

        RotationShakeKind = RotationShakeKind,
        RotationShakeKind2 = RotationShakeKind2,
        PositionShakeKind = PositionShakeKind
      };
    }
  }
}