using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
  [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
  public class Progression : ScriptableObject
  {
    [Serializable]
    class ProgressionCharacterClass
    {
      public CharacterClass characterClass;
      public ProgressionStats[] progressionStats;
    }

    [Serializable]
    class ProgressionStats
    {
      public Stat stat;
      public float[] levels;
    }

    [SerializeField]
    private ProgressionCharacterClass[] characterClasses = null;

    private Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable;

    public float GetStat(CharacterClass characterClass, Stat stat, int level)
    {
      BuildLookup();

      if (lookupTable.ContainsKey(characterClass))
      {
        if (lookupTable[characterClass].ContainsKey(stat)) {
          if (level > 0 && level <= lookupTable[characterClass][stat].Length)
          {
            return lookupTable[characterClass][stat][level - 1];
          }
        }
      }

      return 0;
    }

    public void BuildLookup()
    {
      if (lookupTable != null) return;

      lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

      foreach (ProgressionCharacterClass progressionCharacterClass in characterClasses)
      {

        var statLookupTable = new Dictionary<Stat, float[]>();
        foreach (ProgressionStats progressionStat in progressionCharacterClass.progressionStats)
        {
          statLookupTable.Add(progressionStat.stat, progressionStat.levels);
        }

        lookupTable.Add(progressionCharacterClass.characterClass, statLookupTable);

      }
    }
  }
}
