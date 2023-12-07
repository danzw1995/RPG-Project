using System;
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

    [SerializeField] private GameObject levelUpEffectPrefab = null;

    [SerializeField] private bool shouldUseModifier = false;

    public event Action onLevelUp;

    private int currentLevel = 0;

    private void Start()
    {
      currentLevel = CalculateLevel();

      Experience experience = GetComponent<Experience>();
      if (experience != null)
      {
        experience.onExperienceGained += UpdateLevel;
      }
    }

    private void UpdateLevel()
    {
      int newLevel = CalculateLevel();
      if (newLevel > currentLevel)
      {
        print("Level up");
        currentLevel = newLevel;
        LevelUpEffect();
        onLevelUp?.Invoke();
      }
    }

    private void LevelUpEffect()
    {
      Instantiate(levelUpEffectPrefab, transform);
    }

    public int GetLevel()
    {
      return currentLevel;
    }

    public float GetStat(Stat stat)
    {
      return (GetBaseStats(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
    }

    private float GetBaseStats(Stat stat)
    {
      return progression.GetStat(characterClass, stat, GetLevel());
    }

    private float GetAdditiveModifier(Stat stat)
    {
      if (!shouldUseModifier) return 0;
      float total = 0;
      foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
      {
        foreach (float modifiers in modifierProvider.GetAdditiveModifiers(stat))
        {
          total += modifiers;
        }
      }
      return total;
    }

    private float GetPercentageModifier(Stat stat)
    {
      if (!shouldUseModifier) return 0;

      float total = 0;
      foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
      {
        foreach (float modifiers in modifierProvider.GetPercentModifiers(stat))
        {
          total += modifiers;
        }
      }
      return total;
    }


    private int CalculateLevel()
    {
      Experience experience = gameObject.GetComponent<Experience>();
      if (experience == null) return startingLevel;

      float[] levels = progression.GetLevels(characterClass, Stat.ExperienceToLevel);
      float experiencePoint = experience.GetExperience();

      int level = 0;

      for (; level < levels.Length; level++)
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

