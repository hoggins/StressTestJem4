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

      const float power = 10f;
      cat.GetComponent<Rigidbody>().velocity =
        new Vector3(Random.Range(0, power), Random.Range(0, power), Random.Range(0, power));
      
      yield return new WaitForSeconds(1.2f);
    }
  }

}