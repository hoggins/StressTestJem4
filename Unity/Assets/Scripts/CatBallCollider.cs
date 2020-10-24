using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
  [RequireComponent(typeof(CatPlacer))]
  public class CatBallCollider : MonoBehaviour
  {
    private CatPlacer _placer;

    private LinkedList<FlyingCat> _flyingQueue = new LinkedList<FlyingCat>();

    private class FlyingCat
    {
      public float LandTime;
      public GameObject Cat;

      public FlyingCat(float landTime, GameObject cat)
      {
        LandTime = landTime;
        Cat = cat;
      }
    }

    private void Awake()
    {
      _placer = GetComponent<CatPlacer>();
    }

    private void Update()
    {
      while (_flyingQueue.Count > 0)
      {
        var fly = _flyingQueue.First;
        if (fly.Value.LandTime > Time.time)
          break;
        _flyingQueue.RemoveFirst();
        var cat = fly.Value.Cat;
        cat.GetComponent<CatControl>().PrepareToBeReal();
        //cat.layer
      }

    }

    void OnCollisionEnter(Collision collision)
    {
      if (!collision.gameObject.CompareTag("Bot") && !collision.gameObject.CompareTag("Player"))
        return;

      foreach (var cc in collision.contacts)
      {
        Debug.DrawRay(cc.point, cc.normal, Color.white, 3);
      }

      var contact = collision.GetContact(0);

      var cats = _placer.DrainCats(contact.point, 3);

      foreach (var cat in cats)
      {
        cat.name = "killed";

        var vector = cat.transform.localPosition;

        cat.transform.SetParent(null, true);
        var body = cat.GetComponent<Rigidbody>();

        cat.GetComponent<CatControl>().PrepareToBeFly();

        body.AddForce(Vector3.up);

        _flyingQueue.AddLast(new FlyingCat(Time.time + 1f, cat));
      }

    }
  }
}