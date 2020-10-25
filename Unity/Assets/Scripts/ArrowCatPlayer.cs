using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
  [RequireComponent(typeof(CatPlacer))]
  public class ArrowCatPlayer : MonoBehaviour
  {
    public GameObject ArrowPrefab;
    private CatPlacer _placer;

    private GameObject _arrow;
    private GameObject _portal;
    private CatPlacer[] _botPlacers;


    private void Awake()
    {
      _placer = GetComponent<CatPlacer>();
      _portal = GameObject.Find("PortalPoi");
    }

    private void Start()
    {
      _botPlacers = FindObjectsOfType<BallBotControl>()
        .Select(x => x.GetComponent<CatPlacer>()).ToArray();
    }

    private void Update()
    {
      Transform target = null;

      if (_placer.CatsAttached >= 30)
      {
        target = _portal.transform;
      }

      if (target == null)
      {
        var targetBot = _botPlacers.FirstOrDefault(x => x.CatsAttached >= 30);
        if (targetBot != null)
          target = targetBot.transform;
      }

      if (target != null && _arrow == null)
        _arrow = Instantiate(ArrowPrefab);
      else if (target == null && _arrow != null)
      {
        Destroy(_arrow);
      }

      if (_arrow != null && target != null)
      {
        _arrow.transform.position = transform.position + Vector3.up * 2;
        _arrow.transform.localScale = Vector3.one * 0.4f;
        //_arrow.transform.rotation = Quaternion.(_arrow.transform.position, _portal.transform.position);
        _arrow.transform.LookAt(target.transform);
      }
    }
  }
}