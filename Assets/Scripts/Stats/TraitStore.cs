
using System;
using System.Collections.Generic;
using GameDevTV.Saving;
using UnityEngine;

namespace RPG.Stats
{
  public class TraitStore : MonoBehaviour, IModifierProvider, ISaveable
  {
    private Dictionary<Trait, int> assignPoints = new Dictionary<Trait, int>();
    private Dictionary<Trait, int> stagedPoints = new Dictionary<Trait, int>();

    [SerializeField] private TraitBonus[] bonusConfig;
    [Serializable]
    private class TraitBonus
    {
      public Trait trait;
      public Stat stat;
      public float additiveBonusPerPoint = 0;
      public float percentageBonusPerPoint = 0;
    }

    private Dictionary<Stat, Dictionary<Trait, float>> additiveBonusCache;
    private Dictionary<Stat, Dictionary<Trait, float>> percentageBonusCache;

    private void Awake()
    {
      additiveBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();
      percentageBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();
      foreach (TraitBonus bonus in bonusConfig)
      {
        if (!additiveBonusCache.ContainsKey(bonus.stat))
        {
          additiveBonusCache[bonus.stat] = new Dictionary<Trait, float>();
        }

        if (!percentageBonusCache.ContainsKey(bonus.stat))
        {
          percentageBonusCache[bonus.stat] = new Dictionary<Trait, float>();
        }

        additiveBonusCache[bonus.stat][bonus.trait] = bonus.additiveBonusPerPoint;
        percentageBonusCache[bonus.stat][bonus.trait] = bonus.percentageBonusPerPoint;
      }
    }

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

    public IEnumerable<float> GetAdditiveModifiers(Stat stat)
    {
      if (!additiveBonusCache.ContainsKey(stat))
      {
        yield break;
      }
      foreach (Trait trait in additiveBonusCache[stat].Keys)
      {
        yield return GetPoints(trait) * additiveBonusCache[stat][trait];
      }
    }

    public IEnumerable<float> GetPercentageModifiers(Stat stat)
    {
      if (!percentageBonusCache.ContainsKey(stat))
      {
        yield break;
      }
      foreach (Trait trait in percentageBonusCache[stat].Keys)
      {
        yield return GetPoints(trait) * percentageBonusCache[stat][trait];
      }
    }

    public object CaptureState()
    {
      return assignPoints;
    }

    public void RestoreState(object state)
    {
      assignPoints = new Dictionary<Trait, int>((IDictionary<Trait, int>)state);
    }
  }
}
