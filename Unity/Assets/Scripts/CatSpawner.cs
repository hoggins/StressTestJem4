using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
  public class CatSpawner : MonoBehaviour
  {
    public int TargetAliveCatCount;
    public GameObject ActorRoot;
    
    void Awake()
    {
      StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
    var firstSpawn = true;
      while (true)
      {
        for (int i = CatControl.AliveCats.Count; i < TargetAliveCatCount; i++)
        {
          var position = GetTargetSpawnPosition(firstSpawn);

          if(position == null)
            continue;

          var cat = CatFactory.Instance.MakeCat();
          cat.transform.position = position.Value + new Vector3(0, 0.5f, 0);

          const float power = 10f;
          cat.GetComponent<Rigidbody>().velocity =
            new Vector3(Random.Range(0, power), Random.Range(0, power), Random.Range(0, power));

          if(!firstSpawn)
            yield return null;
        }

        firstSpawn = false;
        yield return new WaitForSeconds(1.0f);
      }
    }

    private static Vector3? GetTargetSpawnPosition(bool firstSpawn)
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
          var camPosition = CameraMain.Instance.Cam.transform.position;
          var dirToPOint = (hit.position - camPosition);
          var distgance = dirToPOint.magnitude;
          dirToPOint.Normalize();

          var inViewport = viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1 &&
                  viewportPoint.z > 0;
          var visible = !Physics.Raycast(new Ray(camPosition, dirToPOint), out var hit2, distgance, int.MaxValue);
          if (!visible)
          {
            if (hit2.collider.CompareTag("Player") || hit2.collider.CompareTag("Bot"))
            {
              visible = true;
            }
          }

          if (inViewport && visible && !firstSpawn)
          {
            retries--;
            continue;
          }
          else
          {
            if (inViewport)
            {
              // Debug.DrawLine(camPosition, camPosition + dirToPOint * distgance, Color.cyan, 5.0f, false);
              // Debug.DrawLine(camPosition + Vector3.up*0.05f, hit2.point + Vector3.up*0.05f, Color.red, 5.0f, false);
            }

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