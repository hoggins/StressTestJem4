using System;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace DefaultNamespace
{
  [RequireComponent(typeof(CatPlacer))]
  public class CatBallCollider : MonoBehaviour
  {
    private const float InactiveTime = 1f;

    private CatPlacer _placer;

    private LinkedList<FlyingCat> _flyingQueue = new LinkedList<FlyingCat>();
    private AudioRollerPlayer _audio;

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
      _audio = GetComponent<AudioRollerPlayer>();
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

      var force = collision.impulse.magnitude;

      if (force < 1)
        return;

      var totalCount = _placer.CatsAttached;
      int toDetach = 0;
      if (force >= 10)
        toDetach = Math.Max(6, (int) (totalCount * 0.3f));
      else if (force >= 5)
        toDetach = Math.Max(3, (int) (totalCount * 0.3f));
      else if (force >= 3)
        toDetach = Math.Max(3, (int) (totalCount * 0.2f));
      else if (force >= 1)
        toDetach = Math.Max(1, (int) (totalCount * 0.1f));

      toDetach *= collision.gameObject.GetComponent<BonusCollector>().KillBonus;

      var ballSpeedUp = collision.gameObject.GetComponent<BallSpeedUp>();
      toDetach *= ballSpeedUp.IsDashing ? ballSpeedUp.KillBonus : 1;

      if (GetComponent<BallSpeedUp>().IsDashing)
        toDetach = 0;

      if (_audio != null)
      {
        _audio.LastHitEnemyTime = Time.time;
        AudioController.Instance.PlayHitEnemy(_audio.Source);
      }

      if (toDetach == 0)
        return;

      var contact = collision.GetContact(0);

      var cats = _placer.DrainCats(contact.point, toDetach);

      var selfPos = gameObject.transform.position;
      foreach (var cat in cats)
      {
        cat.name = "killed";

        cat.GetComponent<CatControl>().PrepareToBeFly();

        cat.transform.SetParent(null, true);

        if (Vector3.Distance(cat.transform.position, selfPos) < 6)
        {
          var body = cat.GetComponent<Rigidbody>();
          body.velocity = contact.normal * 7;
//          body.velocity = (cat.transform.position- selfPos).normalized * 7;
        }

        _flyingQueue.AddLast(new FlyingCat(Time.time + InactiveTime, cat));
      }

    }
  }
}