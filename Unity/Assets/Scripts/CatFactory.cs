using System;
using UnityEngine;

public class CatFactory : MonoBehaviour
{
  [NonSerialized] public static CatFactory Instance;

  public Color[] Colors = new []{Color.blue};

  public GameObject[] Prefabs;
  public GameObject ActorRoot;

  private System.Random Random = new System.Random();
  private void Awake()
  {
    Instance = this;
    ActorRoot = new GameObject("ActorRoot");
  }

  public GameObject MakeCat()
  {
    var prefab = Prefabs[Random.Next(Prefabs.Length)];

#if UNITY_EDITOR
    var go = Instantiate(prefab, ActorRoot.transform);
#else
    var go = Instantiate(prefab);
#endif

    var ren = go.GetComponent<Renderer>();
    var newMat = new Material(ren.material);
    newMat.SetColor("_BaseColor", Colors[Random.Next(Colors.Length)]);
    ren.material = newMat;


    return go;
  }
}