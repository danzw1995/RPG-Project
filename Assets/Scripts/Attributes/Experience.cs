using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Attributes
{
  public class Experience : MonoBehaviour, ISaveable
  {
    [SerializeField] private float experience = 0f;
    public event Action onExperienceGained;

    public void GainExperience(float rewardExp)
    {
      experience += rewardExp;
      onExperienceGained?.Invoke();
    }

    public float GetExperience()
    {
      return experience;
    }

    public object CaptureState()
    {
      return experience;
    }


    public void RestoreState(object state)
    {
      experience = (float)state;
    }
  }

}
