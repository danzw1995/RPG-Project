using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
  public class PatrolPath : MonoBehaviour
  {
    private void OnDrawGizmos()
    {
      float waypointGizmosRadius = 0.3f;
      for (int i = 0; i < transform.childCount; i++)
      {

        Gizmos.DrawSphere(GetWaypoint(i), waypointGizmosRadius);
        Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i)));

      }
    }

    public int GetNextIndex(int i)
    {
      int nextIndex = i + 1;
      if (nextIndex >= transform.childCount)
      {
        return 0;
      }
      return nextIndex;
    }

    public Vector3 GetWaypoint(int i)
    {
      return transform.GetChild(i).position;
    }
  }

}
