
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
  public class TraitStore : MonoBehaviour
  {
    private Dictionary<Trait, int> assignPoints = new Dictionary<Trait, int>();
    private Dictionary<Trait, int> stagedPoints = new Dictionary<Trait, int>();

    public int GetProposedPoints(Trait trait)
    {
      return GetPoints(trait) + GetStagedPoints(trait);
    }


    public int GetPoints(Trait trait)
    {
      return assignPoints.ContainsKey(trait) ? assignPoints[trait] : 0;
    }

    public int GetStagedPoints(Trait trait)
    {
      return stagedPoints.ContainsKey(trait) ? stagedPoints[trait] : 0;
    }
    public void AssignPoints(Trait trait, int point)
    {

      stagedPoints[trait] = GetStagedPoints(trait) + point;
    }

    public bool CanAssignPoints(Trait trait, int point)
    {
      if (GetStagedPoints(trait) + point < 0) return false;
      if (point > GetUnassignPoints()) return false;
      return true;
    }

    public int GetUnassignPoints()
    {
      return GetAssignablePoints() - GetTotalProposedPoints();
    }

    public int GetTotalProposedPoints()
    {
      int total = 0;
      foreach (int point in assignPoints.Values)
      {
        total += point;
      }
      foreach (int point in stagedPoints.Values)
      {
        total += point;
      }

      return total;
    }

    public int GetAssignablePoints()
    {
      return (int)GetComponent<BaseStats>().GetStat(Stat.TotalTraitPoints);
    }

    public bool IsEmptyStagePoints()
    {
      foreach (Trait trait in stagedPoints.Keys)
      {
        if (GetStagedPoints(trait) > 0)
        {
          return false;
        }
      }

      return true;
    }

    public void Commit()
    {
      foreach (Trait trait in stagedPoints.Keys)
      {
        assignPoints[trait] = GetProposedPoints(trait);
      }
      stagedPoints.Clear();
    }
  }
}
