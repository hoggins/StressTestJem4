using UnityEngine;
using System.Collections.Generic;

namespace EZCameraShake
{
  public class CameraShaker
  {
    /// <summary>
    /// The default position influcence of all shakes created by this shaker.
    /// </summary>
    public Vector3 DefaultPosInfluence = new Vector3(0.15f, 0.15f, 0.15f);

    /// <summary>
    /// The default rotation influcence of all shakes created by this shaker.
    /// </summary>
    public Vector3 DefaultRotInfluence = new Vector3(1, 1, 1);

    public Vector3 PosAddShake, RotAddShake;

    private readonly List<CameraShakeInstance> _cameraShakeInstances = new List<CameraShakeInstance>();
    public static CameraShaker Instance;

    private readonly Vector3 _zero = new Vector3();

    public CameraShaker()
    {
      Instance = this;
    }

    public void DoUpdate()
    {
      PosAddShake = _zero;
      RotAddShake = _zero;

      for (int i = 0; i < _cameraShakeInstances.Count; i++)
      {
        if (i >= _cameraShakeInstances.Count)
          break;

        var c = _cameraShakeInstances[i];

        if (c.CurrentState == CameraShakeState.Inactive && c.DeleteOnInactive)
        {
          _cameraShakeInstances.RemoveAt(i);
          i--;
        }
        else if (c.CurrentState != CameraShakeState.Inactive)
        {
          PosAddShake += CameraUtilities.MultiplyVectors(c.UpdateShake(c.PositionShakeKind), c.PositionInfluence);
          RotAddShake += CameraUtilities.MultiplyVectors(c.UpdateShake(c.RotationShakeKind), c.RotationInfluence);
          RotAddShake += CameraUtilities.MultiplyVectors(c.UpdateShake(c.RotationShakeKind2), c.RotationInfluence2);
        }
      }
    }

    /// <summary>
    /// Starts a shake using the given preset.
    /// </summary>
    /// <param name="shake">The preset to use.</param>
    /// <returns>A CameraShakeInstance that can be used to alter the shake's properties.</returns>
    public CameraShakeInstance Shake(CameraShakeInstance shake)
    {
      _cameraShakeInstances.Add(shake);
      return shake;
    }

    /// <summary>
    /// Shake the camera once, fading in and out  over a specified durations.
    /// </summary>
    /// <param name="magnitude">The intensity of the shake.</param>
    /// <param name="roughness">Roughness of the shake. Lower values are smoother, higher values are more jarring.</param>
    /// <param name="fadeInTime">How long to fade in the shake, in seconds.</param>
    /// <param name="fadeOutTime">How long to fade out the shake, in seconds.</param>
    /// <returns>A CameraShakeInstance that can be used to alter the shake's properties.</returns>
    public CameraShakeInstance ShakeOnce(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
    {
      CameraShakeInstance shake = new CameraShakeInstance(magnitude, roughness, fadeInTime, fadeOutTime);
      shake.PositionInfluence = DefaultPosInfluence;
      shake.RotationInfluence = DefaultRotInfluence;
      _cameraShakeInstances.Add(shake);

      return shake;
    }

    /// <summary>
    /// Shake the camera once, fading in and out over a specified durations.
    /// </summary>
    /// <param name="magnitude">The intensity of the shake.</param>
    /// <param name="roughness">Roughness of the shake. Lower values are smoother, higher values are more jarring.</param>
    /// <param name="fadeInTime">How long to fade in the shake, in seconds.</param>
    /// <param name="fadeOutTime">How long to fade out the shake, in seconds.</param>
    /// <param name="posInfluence">How much this shake influences position.</param>
    /// <param name="rotInfluence">How much this shake influences rotation.</param>
    /// <returns>A CameraShakeInstance that can be used to alter the shake's properties.</returns>
    public CameraShakeInstance ShakeOnce(float magnitude, float roughness, float fadeInTime, float fadeOutTime,
      Vector3 posInfluence, Vector3 rotInfluence)
    {
      CameraShakeInstance shake = new CameraShakeInstance(magnitude, roughness, fadeInTime, fadeOutTime);
      shake.PositionInfluence = posInfluence;
      shake.RotationInfluence = rotInfluence;
      _cameraShakeInstances.Add(shake);

      return shake;
    }

    /// <summary>
    /// Start shaking the camera.
    /// </summary>
    /// <param name="magnitude">The intensity of the shake.</param>
    /// <param name="roughness">Roughness of the shake. Lower values are smoother, higher values are more jarring.</param>
    /// <param name="fadeInTime">How long to fade in the shake, in seconds.</param>
    /// <returns>A CameraShakeInstance that can be used to alter the shake's properties.</returns>
    public CameraShakeInstance StartShake(float magnitude, float roughness, float fadeInTime)
    {
      CameraShakeInstance shake = new CameraShakeInstance(magnitude, roughness);
      shake.PositionInfluence = DefaultPosInfluence;
      shake.RotationInfluence = DefaultRotInfluence;
      shake.StartFadeIn(fadeInTime);
      _cameraShakeInstances.Add(shake);
      return shake;
    }

    /// <summary>
    /// Start shaking the camera.
    /// </summary>
    /// <param name="magnitude">The intensity of the shake.</param>
    /// <param name="roughness">Roughness of the shake. Lower values are smoother, higher values are more jarring.</param>
    /// <param name="fadeInTime">How long to fade in the shake, in seconds.</param>
    /// <param name="posInfluence">How much this shake influences position.</param>
    /// <param name="rotInfluence">How much this shake influences rotation.</param>
    /// <returns>A CameraShakeInstance that can be used to alter the shake's properties.</returns>
    public CameraShakeInstance StartShake(float magnitude, float roughness, float fadeInTime, Vector3 posInfluence,
      Vector3 rotInfluence)
    {
      CameraShakeInstance shake = new CameraShakeInstance(magnitude, roughness);
      shake.PositionInfluence = posInfluence;
      shake.RotationInfluence = rotInfluence;
      shake.StartFadeIn(fadeInTime);
      _cameraShakeInstances.Add(shake);
      return shake;
    }

    /// <summary>
    /// Gets a copy of the list of current camera shake instances.
    /// </summary>
    public List<CameraShakeInstance> ShakeInstances
    {
      get { return _cameraShakeInstances; }
    }
  }
}