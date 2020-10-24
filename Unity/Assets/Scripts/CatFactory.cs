using System;
using UnityEngine;

public class CatFactory : MonoBehaviour
{
  [NonSerialized] public static CatFactory Instance;

  public Color[] Colors = new []{Color.blue};

  public Mesh[] Meshes;

  public GameObject DefaultCat;


  private System.Random Random = new System.Random();
  private void Awake()
  {
    Instance = this;
  }

  public GameObject MakeCat()
  {
    var go = Instantiate(DefaultCat);

    var ren = go.GetComponent<Renderer>();
    var newMat = new Material(ren.material);
    newMat.SetColor("_BaseColor", Colors[Random.Next(Colors.Length)]);
    ren.material = newMat;

    var meshFilter = go.GetComponent<MeshFilter>();
    meshFilter.mesh = Meshes[Random.Next(Meshes.Length)];

    return go;
  }
}