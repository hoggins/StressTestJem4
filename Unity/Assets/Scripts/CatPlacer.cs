using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SphereCollider))]
public class CatPlacer : MonoBehaviour
{
  public int Level => _freePointsByLevel.Count;
  public int CatsAttached => _busyPointsByLevel.Sum(x => x.Count);

  public GameObject DefaultCat;
  public bool AttachTest = false;

  public float[] SizeByLevel = new[]
  {
    0.62f,
    0.74f,
    1.01f,
    1.22f,
    1.4f,
    1.72f
  };

  public int AttachedCatsCount { get; private set; }

  private SphereCollider Collider;

  private List<List<Vector3>> _freePointsByLevel = new List<List<Vector3>>();
  private List<List<GameObject>> _busyPointsByLevel = new List<List<GameObject>>();
  private CatPointProvider _pointProvider;


  private void Awake()
  {
    Collider = GetComponent<SphereCollider>();
  }

  void Start()
  {
    _pointProvider = GameObject.Find("App").GetComponent<CatPointProvider>();


    if (AttachTest)
    {
      for (int i = 0; i < 3; i++)
      {
        AttachCat(Instantiate(DefaultCat), Vector3.zero);
      }

      StartCoroutine(Fade());
    }
  }

  private void OnCollisionEnter(Collision other)
  {
    if (other.gameObject.CompareTag("Cat"))
    {
      AttachCat(other.gameObject, other.GetContact(0).point);
    }
  }

  IEnumerator Fade()
  {
    while (true)
    {
      var go = Instantiate(DefaultCat);
      AttachCat(go, Vector3.zero);
      yield return new WaitForSeconds(1f);
    }
  }

  public void AttachCat(GameObject catGo, Vector3 hitPosition)
  {
    AttachedCatsCount++;
    
    var freeLevel = _freePointsByLevel.FirstOrDefault(x => x.Count > 0);
    if (freeLevel == null)
    {
      _freePointsByLevel.Add(freeLevel = _pointProvider.GetForLevel(_freePointsByLevel.Count));
      _busyPointsByLevel.Add(new List<GameObject>());
    }

    var point = freeLevel[freeLevel.Count - 1];
    freeLevel.RemoveAt(freeLevel.Count - 1);

    catGo.GetComponent<CatControl>().PrepareToBePart();
    catGo.transform.SetParent(transform, true);
    catGo.transform.localPosition = point /*+ (Random.insideUnitSphere * 0.1f)*/;
    catGo.transform.localRotation = Random.rotation;

    _busyPointsByLevel[_busyPointsByLevel.Count-1].Add(catGo);

    UpdateStats();
  }

  private void UpdateStats()
  {
    if (_busyPointsByLevel.Count == 0)
      Collider.radius = 0.1f;
    else
      Collider.radius = SizeByLevel[_busyPointsByLevel.Count-1];
  }

  public void RemoveLayer(int layer)
  {
    if (_busyPointsByLevel.Count == 0 && layer == 0)
      return;

    if (layer >= _busyPointsByLevel.Count || layer < 0)
    {
      Debug.LogWarning($"cant remove layer {layer} of total {_busyPointsByLevel.Count}");
      return;
    }

    foreach (var o in _busyPointsByLevel[layer])
    {
      Destroy(o);
    }

    _busyPointsByLevel[layer].Clear();
    var lastIdx = _busyPointsByLevel.Count-1;
    if (layer == lastIdx)
    {
      _busyPointsByLevel.RemoveAt(lastIdx);
      _freePointsByLevel.RemoveAt(lastIdx);
    }

    UpdateStats();
  }

  public void AddLayer(int layer)
  {
    while (_freePointsByLevel.Count <= layer || _freePointsByLevel.Last().Count > 0)
    {
      var go = Instantiate(DefaultCat);
      AttachCat(go, Vector3.zero);
    }
  }
}