using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
  public class CatSpawner : MonoBehaviour
  {
    public int TargetAliveCatCount;
    
    void Awake()
    {
      StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
      while (true)
      {
        for (int i = CatControl.AliveCats.Count; i < TargetAliveCatCount; i++)
        {
          var position = GetTargetSpawnPosition();

          if(position == null)
            continue;


          var cat = CatFactory.Instance.MakeCat();
          cat.transform.position = position.Value + new Vector3(0, 1, 0);

          const float power = 10f;
          cat.GetComponent<Rigidbody>().velocity =
            new Vector3(Random.Range(0, power), Random.Range(0, power), Random.Range(0, power));

          yield return null;
        }
        
        yield return new WaitForSeconds(1.0f);
      }
    }

    private static Vector3? GetTargetSpawnPosition()
    {
      var retries = 10;
      Vector3? targetPosition = null; 
      while (retries > 0)
      {
        var bounds = Level.Instance.LevelBounds;

        var from = new Vector2(bounds.min.x, bounds.min.z);
        var to = new Vector2(bounds.max.x, bounds.max.z);

        var result = new Vector3(Random.Range(@from.x, to.x), 200, Random.Range(@from.y, to.y));

        if (Physics.Raycast(new Ray(result, Vector3.down), out var rayHit, 400, int.MaxValue))
        {
          result.y = rayHit.point.y;
        }

        if (NavMesh.SamplePosition(result, out var hit, 10, int.MaxValue))
        {
          var viewportPoint = Camera.main.WorldToViewportPoint(hit.position);
          if (viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1 &&
              viewportPoint.z > 0)
          {
            Debug.DrawLine(hit.position, hit.position + Vector3.up * 20, Color.black, 5.0f, false);
            retries--;
            continue;
          }
          else
          {
            
            Debug.DrawLine(hit.position, hit.position + Vector3.up * 30, Color.green, 5.0f, false);
            targetPosition = hit.position;
            break;
          }
        }

        retries--;
      }

      return targetPosition;
    }
  }
}