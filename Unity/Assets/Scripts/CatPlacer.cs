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
  
  private Dictionary<GameObject, Coroutine> _attachingCatsCoroutines = new Dictionary<GameObject, Coroutine>();


  private void Awake()
  {
    Collider = GetComponent<SphereCollider>();
  }

  void Start()
  {
    _pointProvider = GameObject.Find("App").GetComponent<CatPointProvider>();

    for (int i = 0; i < 6; i++)
    {
      var makeCat = CatFactory.Instance.MakeCat();
      makeCat.transform.position = transform.position;
      AttachCat(makeCat);
    }

    if (AttachTest)
    StartCoroutine(Fade());
  }

  private void OnCollisionEnter(Collision other)
  {
    if (other.gameObject.CompareTag("Cat"))
    {
      AttachCat(other.gameObject);
    }
  }

  IEnumerator Fade()
  {
    while (true)
    {
      var go =CatFactory.Instance.MakeCat();
      AttachCat(go);
      yield return new WaitForSeconds(1f);
    }
  }

  public void AttachCat(GameObject catGo)
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

    _attachingCatsCoroutines.Add(catGo, StartCoroutine(AttachCoroutine(catGo, point)));
    
    _busyPointsByLevel[_busyPointsByLevel.Count-1].Add(catGo);

    UpdateStats();
  }

  private IEnumerator AttachCoroutine(GameObject catGo, Vector3 targetPoint)
  {
    var startPoint = catGo.transform.localPosition;
    var elapsed = 0f;
    const float duration = 1f;

    // var center = transform.position;
    // var startForward = Quaternion.LookRotation((center - catGo.transform.position).normalized, Vector3.up);
    //
    // var targetWorld = transform.TransformPoint(targetPoint);
    // var endForward = Quaternion.LookRotation((center - targetWorld).normalized, Vector3.up);
    //
    // var distanceToTarget = Vector3.Distance(center, targetWorld);
    // var distanceToStart = Vector3.Distance(center, catGo.transform.position);
    //
    //
    //
    //
    // var realStart = catGo.transform.localPosition;
    //   var posStart = startForward * Vector3.forward * distanceToStart;
    //   var posEnd = endForward * Vector3.forward * distanceToTarget;
    
    // Debug.Log($"start {catGo.transform.localPosition} {posStart}");
    
    var currentCatRotation = catGo.transform.localRotation;
    var targetCatRotation = Random.rotation;
    do
    {
      elapsed += Time.deltaTime;
      
      var t = elapsed / duration;
      
      // var currentRotation = Quaternion.Slerp(startForward, endForward, t);
      // catGo.transform.localPosition = currentRotation * Vector3.forward * Mathf.Lerp(distanceToStart, distanceToTarget, t);
      catGo.transform.localPosition = Vector3.Lerp(startPoint, targetPoint, t);
      catGo.transform.localRotation = Quaternion.Lerp(currentCatRotation, targetCatRotation, t);
      
      yield return null;
    } while (elapsed < duration);

    _attachingCatsCoroutines.Remove(catGo);
  }

  private void UpdateStats()
  {
    if (_busyPointsByLevel.Count == 0)
      Collider.radius = 0.1f;
    else if (_busyPointsByLevel.Count > SizeByLevel.Length)
      Collider.radius = SizeByLevel[SizeByLevel.Length-1];
    else
      Collider.radius = SizeByLevel[_busyPointsByLevel.Count-1];
  }

  public void RemoveLayer(int layer)
  {
    layer = Math.Min(layer, 1);
    while (layer < _busyPointsByLevel.Count)
    {
      var cats = DrainCats(Vector3.zero, 1);
      foreach (var cat in cats)
      {
        Destroy(cat);
      }
    }
  }

  public void AddLayer(int layer)
  {
    while (_freePointsByLevel.Count < layer || _freePointsByLevel.Last().Count > 0)
    {
      var go = CatFactory.Instance.MakeCat();
      AttachCat(go);
    }
  }

  public List<GameObject> DrainCats(Vector3 point, int countToTake)
  {
    const int minCount = 3;
    var catsAttached = CatsAttached;
    if (catsAttached <= minCount)
      return new List<GameObject>();

    countToTake = Math.Min(minCount, catsAttached - countToTake);

    // return DrainFromContact();

    return DrainEx();

    List<GameObject> DrainEx()
    {
      var handled = new List<GameObject>();
      for (var i = _busyPointsByLevel.Count - 1; i >= 0; i--)
      {
        var layer = _busyPointsByLevel[i];
        while (true)
        {
          var cat = layer[layer.Count-1];
          handled.Add(cat);
          layer.RemoveAt(layer.Count - 1);

          _freePointsByLevel[i].Add(cat.transform.position);
          if (layer.Count == 0)
          {
            _busyPointsByLevel.RemoveAt(i);
            _freePointsByLevel.RemoveAt(i);
            UpdateStats();
            break;
          }

          if (handled.Count == countToTake)
            return handled;
        }

        if (handled.Count == countToTake)
          return handled;

      }

      return handled;
    }

    List<GameObject> DrainFromContact()
    {
      var toRemove = _busyPointsByLevel
        .SelectMany(x => x)
        .OrderBy(x => Vector3.Distance(x.transform.position, point))
        .Take(countToTake)
        .ToList();
      var handled = new List<GameObject>();
      for (var busyLayerIdx = 0; busyLayerIdx < _busyPointsByLevel.Count; busyLayerIdx++)
      {
        var busyLayer = _busyPointsByLevel[busyLayerIdx];
        foreach (var cat in toRemove)
        {
          if (!busyLayer.Remove(cat))
            continue;
          _freePointsByLevel[busyLayerIdx].Add(cat.transform.localPosition);
          handled.Add(cat);
        }

        if (busyLayer.Count == 0)
        {
          _busyPointsByLevel.RemoveAt(busyLayerIdx);
          _freePointsByLevel.RemoveAt(busyLayerIdx);
          UpdateStats();
          Debug.Log("downshift");
        }
        else
        {
          Debug.Log($"pending for downshift {busyLayer.Count}");
        }
      }

      return handled;
    }
  }
}