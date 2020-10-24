using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class CatControl : MonoBehaviour
{
  public static readonly HashSet<CatControl> AliveCats = new HashSet<CatControl>();
  private Collider _collider;
  private Rigidbody _body;

  private void Awake()
  {
    _collider = GetComponent<Collider>();
    _body = GetComponent<Rigidbody>();
  }

  public void PrepareToBePart()
  {
    _collider.enabled = false;
    _body.isKinematic = true;
    AliveCats.Remove(this);
  }

  public void PrepareToBeFly()
  {
    gameObject.layer = 9;
    _collider.enabled = true;
    _body.isKinematic = false;
  }

  public void PrepareToBeReal()
  {
    var collisions = Physics.OverlapSphere(transform.position, 0.3f, LayerMask.GetMask("Actors"));
    var theHit = collisions
      .Where(x=>x.gameObject.CompareTag("Bot") || x.gameObject.CompareTag("Player"))
      .OrderBy(x => x.contactOffset)
      .FirstOrDefault();
    if (theHit != null)
    {
      var go = theHit.gameObject;
      var catPlacer = go.GetComponent<CatPlacer>();
      if (catPlacer == null)
        Debug.Log("null ", go);
      catPlacer.AttachCat(gameObject, transform.position);
    }
    else
    {
      gameObject.layer = 8;
      AliveCats.Add(this);
      _collider.enabled = true;
      _body.isKinematic = false;
    }
  }

  public static CatControl GetClosest(Vector3 position)
  {
    var minDist = float.MaxValue;
    CatControl resultCat = null;

    foreach (var cat in AliveCats)
    {
      var d = Vector3.Distance(position, cat.transform.position);
      if (minDist > d)
      {
        resultCat = cat;
        minDist = d;
      }
    }

    return resultCat;
  }
  private void OnEnable()
  {
    AliveCats.Add(this);
  }

  private void OnDisable()
  {
    AliveCats.Remove(this);
  }
}