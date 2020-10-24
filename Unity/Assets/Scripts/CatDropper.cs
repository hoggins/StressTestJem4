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
      Instantiate(CatPrefab, transform.position, Quaternion.identity);
      yield return new WaitForSeconds(1.2f);
    }
  }

}