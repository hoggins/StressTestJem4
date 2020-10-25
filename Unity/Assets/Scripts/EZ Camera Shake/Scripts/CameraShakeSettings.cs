using UnityEngine;

namespace EZCameraShake
{
  [CreateAssetMenu(fileName = "cameraShake", menuName = "Legion/CameraShake", order = 1)]
  public class CameraShakeSettings : ScriptableObject
  {
    public CameraShakeKind PositionKind;
    public Vector3 PositionInfluence = new Vector3(1, 1, 1);
    public CameraShakeKind RotationKind;
    public Vector3 RotationInfluence = new Vector3(1, 1, 0);

    //todo: probably temp, can be replaced with second instance of shake with different rotation settings
    public CameraShakeKind RotationKind2 = CameraShakeKind.Damped;
    public Vector3 RotationInfluence2 = new Vector3(0, 0, 0);

    public float Magnitude = 1;

    /// <summary>
    /// Roughness of the shake. Small value smooth shake, high rough
    /// </summary>
    public float Roughness = 1;

    public float FadeOutDuration = 1;
    public float FadeInDuration;
  }
}