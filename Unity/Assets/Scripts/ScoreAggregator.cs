using UnityEngine;

namespace DefaultNamespace
{
  public class ScoreAggregator : MonoBehaviour
  {
    public bool IsPlayer;

    public void TrackTakenCat()
    {
      if (!IsPlayer)
        return;
      Program.Score.Taken += 1;
    }

    public void TrackClearedCat(int toDetach)
    {
      if (!IsPlayer)
        return;
      Program.Score.Cleared += toDetach;
    }
  }
}