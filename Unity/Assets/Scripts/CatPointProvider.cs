using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

[ExecuteInEditMode]
public class CatPointProvider : MonoBehaviour
{
  public int StepScale;
  public GameObject Container;
  private MeshFilter[] MeshFilters;
  private List<List<Vector3>> _cachedLevels;

  private void Awake()
  {
    var rnd = new Random();
    MeshFilters = Container.GetComponentsInChildren<MeshFilter>();

    _cachedLevels = new List<List<Vector3>>();
    var level = 0;
    foreach (var meshFilter in MeshFilters)
    {
      level++;
      var ver = new List<Vector3>();
      meshFilter.sharedMesh.GetVertices(ver);
      ver = ver.Distinct().ToList();
      Shuffle(ver);
      _cachedLevels.Add(ver);

      for (var i = 0; i < ver.Count; i++)
      {
        var point = ver[i];
        var s = StepScale * (level +1);
        var pScale = new Vector3(point.x *s, point.y*s, point.z * s);
        ver[i] = pScale;
      }
    }


    void Shuffle<T>(IList<T> list)
    {
      int n = list.Count;
      while (n > 1) {
        n--;
        int k = rnd.Next(n + 1);
        T value = list[k];
        list[k] = list[n];
        list[n] = value;
      }
    }
  }

  public List<Vector3> GetForLevel(int level)
  {
    level = Math.Min(level, MeshFilters.Length - 1);
    return _cachedLevels[level].ToList();
  }
}