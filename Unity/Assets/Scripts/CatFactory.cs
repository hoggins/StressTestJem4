using System;
using UnityEngine;

public class CatFactory : MonoBehaviour
{
  [NonSerialized] public static CatFactory Instance;

  public Color[] Colors = new []{Color.blue};

  public GameObject[] Prefabs;

  private System.Random Random = new System.Random();
  private void Awake()
  {
    Instance = this;
  }

  public GameObject MakeCat()
  {
    var prefab = Prefabs[Random.Next(Prefabs.Length)];

    var go = Instantiate(prefab);

    var ren = go.GetComponent<Renderer>();
    var newMat = new Material(ren.material);
    newMat.SetColor("_BaseColor", Colors[Random.Next(Colors.Length)]);
    ren.material = newMat;


    return go;
  }
}