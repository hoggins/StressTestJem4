using System.Collections;
using UnityEngine;

public class CatDropper : MonoBehaviour
{
  public GameObject CatPrefab;

  void Start()
  {
    StartCoroutine(DropCats());
  }

  IEnumerator DropCats()
  {
    while (true)
    {
      var cat = CatFactory.Instance.MakeCat();
      cat.transform.position = transform.position;
      yield return new WaitForSeconds(1.2f);
    }
  }

}