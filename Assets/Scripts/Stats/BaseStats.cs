using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Stats
{
  public class BaseStats : MonoBehaviour
  {
    [Range(1, 99)]
    [SerializeField] private int startingLevel = 1;

    [SerializeField] private CharacterClass characterClass;

    [SerializeField] private Progression progression = null;

    public float GetStat(Stat stat)
    {
      return progression.GetStat(characterClass, stat, GetLevel());
    }

    public int GetLevel()
    {
      Experience experience = gameObject.GetComponent<Experience>();
      if (experience == null) return startingLevel;

      float[] levels = progression.GetLevels(characterClass, Stat.ExperienceToLevel);
      float experiencePoint = experience.GetExperience();

      int level = 0;

      for(; level < levels.Length; level ++)
      {
        if (levels[level] > experiencePoint)
        {
          break;
        }
      }

      return level + 1;
    }

  }
}

