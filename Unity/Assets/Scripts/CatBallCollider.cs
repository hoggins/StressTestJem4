using System;
using UnityEngine;

namespace DefaultNamespace
{
  [RequireComponent(typeof(CatPlacer))]
  public class CatBallCollider : MonoBehaviour
  {
    private CatPlacer _placer;

    private void Awake()
    {
      _placer = GetComponent<CatPlacer>();
    }

    void OnCollisionEnter(Collision collision)
    {
      if (!collision.gameObject.CompareTag("Bot") && !collision.gameObject.CompareTag("Player"))
        return;

      foreach (var cc in collision.contacts)
      {
        Debug.DrawRay(cc.point, cc.normal, Color.white);
      }

      var contact = collision.GetContact(0);

      var cats = _placer.DrainCats(contact.point);

      foreach (var cat in cats)
      {
        cat.name = "killed";
        Destroy(cat);
      }

    }
  }
}