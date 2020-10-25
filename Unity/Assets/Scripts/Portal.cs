using UnityEngine;

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