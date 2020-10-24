using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.CatStates;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class CatControl : MonoBehaviour
{
  public static readonly HashSet<CatControl> AliveCats = new HashSet<CatControl>();
  [NonSerialized]
  public Collider Collider;
  [NonSerialized]
  public Rigidbody Body;



  private List<CatStateBase> _allStates;
  private CatStateBase _currentState;
  private float _changeStateDur;


  private void Awake()
  {
    Collider = GetComponent<Collider>();
    Body = GetComponent<Rigidbody>();
    _allStates = new List<CatStateBase>()
    {
      new CatStateIdle(this),
      new CatStateJump(this),
      new CatStateRunAround(this),
      new CatStateRunAwayFromPlayer(this),
    };

    SelectRandomState();
  }

  public void PrepareToBePart()
  {
    Collider.enabled = false;
    Body.isKinematic = true;
    AliveCats.Remove(this);
    _currentState = null;
    _changeStateDur = float.MaxValue;
  }

  public void PrepareToBeFly()
  {
    gameObject.layer = 9;
    Collider.enabled = true;
    Body.isKinematic = false;
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
      catPlacer.AttachCat(gameObject);
    }
    else
    {
      gameObject.layer = 8;
      AliveCats.Add(this);
    Collider.enabled = true;
    Body.isKinematic = false;

    SelectRandomState();
    }
  }

  void Update()
  {
    _changeStateDur -= Time.deltaTime;
    if (_changeStateDur <= 0f)
      SelectRandomState();

    _currentState?.Update();

    if (transform.position.y < -100)
      Destroy(gameObject);
  }

  private void FixedUpdate()
  {
    _currentState?.FixedUpdate();
  }

  void SelectRandomState()
  {
    _currentState = _allStates[Random.Range(0, _allStates.Count)];
    _changeStateDur = Random.Range(_currentState.ChangeStateDurMin, _currentState.ChangeStateDurMax);
    _currentState.OnEnter();
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

  private void OnDrawGizmos()
  {
    if(_currentState == null)
      return;

#if UNITY_EDITOR
    UnityEditor.Handles.Label(transform.position + new Vector3(1, 2, 0), _currentState.GetType().Name);
#endif
  }
}