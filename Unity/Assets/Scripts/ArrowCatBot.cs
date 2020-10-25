using System;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
  [RequireComponent(typeof(CatPlacer))]
  public class ArrowCatBot : MonoBehaviour
  {
    public GameObject ArrowPrefab;
    private CatPlacer _placer;

    private GameObject _arrow;
    private CatPlacer[] _botPlacers;

    private void Awake()
    {
      _placer = GetComponent<CatPlacer>();
    }


    private void Update()
    {
      return;
      Transform target = null;

      if (_placer.CatsAttached >= 30 && _arrow == null)
      {
        _arrow = Instantiate(ArrowPrefab);
      }
      else if (_placer.CatsAttached < 30 && _arrow != null)
      {
        Destroy(_arrow);
      }

      if (_arrow != null)
      {
        _arrow.transform.position = transform.position + Vector3.up * 3;
        _arrow.transform.localScale = Vector3.one * 0.4f;
        _arrow.transform.rotation = Quaternion.Euler(new Vector3(90,0,0));
      }
    }
  }
}