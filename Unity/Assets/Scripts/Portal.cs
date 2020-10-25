using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Portal : MonoBehaviour
{
  public static Portal Instance;

  public GameObject Explosion;
  public GameObject Fx;
  public Transform Root;
  public Transform ExplPosition;

  public float Distance = 200;
  public float Power = 2000;
  public bool IsActive;

  private void Awake()
  {
    Instance = this;
    foreach (var particles in Fx.GetComponentsInChildren<ParticleSystem>())
    {
      particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
  }

  public void Active()
  {
    if(IsActive)
      return;

    IsActive = true;
    var inst = GameObject.Instantiate(Explosion, Root.transform.position, Quaternion.identity);
    Destroy(inst, 15);
    
    foreach (var particles in Fx.GetComponentsInChildren<ParticleSystem>())
    {
      particles.Play(true);
    }

    var rigidodies = FindObjectsOfType<Rigidbody>();
    foreach (var rb in rigidodies)
    {
      if (rb.isKinematic)
      {
        continue;
      }

      var direction = (rb.position - ExplPosition.transform.position);
      var distance = direction.magnitude;
      direction.Normalize();

      var dp = 1f - Mathf.Clamp01(distance / Distance);
      
      rb.AddForce(direction * dp * Power);
    }

    StartCoroutine(ChangeCameraSettings());
  }

  private IEnumerator ChangeCameraSettings()
  {
    var elapsed = 0f;
    var volume  = CameraMain.Instance.GetComponent<PostProcessVolume>();

    do
    {
      elapsed += Time.deltaTime;
      yield return null;

      var bloom = volume.profile.settings.Find(x => x is Bloom) as Bloom;
      bloom.intensity.Interp(2, 8, elapsed / 0.3f);
      bloom.threshold.Interp(0.75f, 0.5f, elapsed / 0.3f);
    } while (elapsed < 0.3);
    
    // yield return new WaitForSeconds(1.0f);

    elapsed = 0f;
    do
    {
      elapsed += Time.deltaTime;
      yield return null;

      var bloom = volume.profile.settings.Find(x => x is Bloom) as Bloom;
      bloom.intensity.Interp(8, 2, elapsed / 3f);
      bloom.threshold.Interp(0.5f, 0.75f, elapsed / 3f);
    } while (elapsed < 3f);
  }

  public void Deactivate()
  {
    if (!IsActive)
      return;

    IsActive = false;
    
    foreach (var particles in Fx.GetComponentsInChildren<ParticleSystem>())
    {
      particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
  }
}