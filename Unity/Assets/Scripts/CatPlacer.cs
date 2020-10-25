using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Controllers;
using DefaultNamespace;
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

  private SphereCollider Collider;

  private List<List<Vector3>> _freePointsByLevel = new List<List<Vector3>>();
  private List<List<GameObject>> _busyPointsByLevel = new List<List<GameObject>>();
  private CatPointProvider _pointProvider;

  private Dictionary<GameObject, Vector3> _targetPoints = new Dictionary<GameObject, Vector3>();

  private Dictionary<GameObject, Coroutine> _attachingCatsCoroutines = new Dictionary<GameObject, Coroutine>();
  private AudioRollerPlayer _audio;


  private void Awake()
  {
    Collider = GetComponent<SphereCollider>();
    _audio = GetComponent<AudioRollerPlayer>();
  }

  void Start()
  {
    _pointProvider = GameObject.Find("App").GetComponent<CatPointProvider>();

    for (int i = 0; i < 6; i++)
    {
      var makeCat = CatFactory.Instance.MakeCat();
      makeCat.transform.position = transform.position;
      AttachCat(makeCat, true);
    }

    if (AttachTest)
      StartCoroutine(Fade());
  }

  private void OnCollisionEnter(Collision other)
  {
    if (other.gameObject.CompareTag("Cat"))
    {
      AttachCat(other.gameObject);

      var count = GetComponent<BonusCollector>().DoubleBonus;
      for (int i = 1; i < count; i++)
      {
        var newCat = Instantiate(other.gameObject, transform.position, Quaternion.identity);
        AttachCat(newCat);
      }
    }
  }

  IEnumerator Fade()
  {
    while (true)
    {
      var go =CatFactory.Instance.MakeCat();
      go.transform.position = transform.position;
      AttachCat(go);
      yield return new WaitForSeconds(1f);
    }
  }

  public void AttachCat(GameObject catGo, bool silent = false)
  {
    if (!silent && _audio != null)
    {
      _audio.LastHitEnemyTime = Time.time;
      AudioController.Instance.PlayPickCat(_audio.DefaultSource);
    }

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

    _targetPoints[catGo] = point;
    _attachingCatsCoroutines.Add(catGo, StartCoroutine(AttachCoroutine(catGo, point)));

    _busyPointsByLevel[_busyPointsByLevel.Count-1].Add(catGo);

    UpdateStats();
  }

  private IEnumerator AttachCoroutine(GameObject catGo, Vector3 targetPoint)
  {
    var startPoint = catGo.transform.localPosition;
    var elapsed = 0f;
    const float duration = 1f;

    var currentCatRotation = catGo.transform.localRotation;
    var targetCatRotation = Random.rotation;
    do
    {
      elapsed += Time.deltaTime;

      var t = elapsed / duration;

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
      go.transform.position = transform.position;
      AttachCat(go, true);
    }
  }

  public List<GameObject> DrainCats(Vector3 point, int countToTake)
  {
    const int minCount = 3;
    var catsAttached = CatsAttached;
    if (catsAttached <= minCount)
      return new List<GameObject>();

    if (countToTake > catsAttached - minCount)
      countToTake = catsAttached - minCount;

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

          RemoveCat(i, cat);

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

          RemoveCat(busyLayerIdx, cat);

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

  private void RemoveCat(int layerIdx, GameObject cat)
  {
    _freePointsByLevel[layerIdx].Add(_targetPoints[cat]);
    _targetPoints.Remove(cat);

    if (_attachingCatsCoroutines.ContainsKey(cat))
    {
      StopCoroutine(_attachingCatsCoroutines[cat]);
      _attachingCatsCoroutines.Remove(cat);
    }
  }
}