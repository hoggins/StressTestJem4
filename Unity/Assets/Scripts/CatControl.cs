using System;
using System.Collections.Generic;
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