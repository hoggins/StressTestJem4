using System.Collections.Generic;
using UnityEngine;

public class CatControl : MonoBehaviour
{
  public static readonly HashSet<CatControl> AliveCats = new HashSet<CatControl>();

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
}