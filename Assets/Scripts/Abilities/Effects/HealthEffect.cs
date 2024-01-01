using System;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Abilities.Filters
{
  [CreateAssetMenu(fileName = "Health Effect", menuName = "Abilities/Effects/Health", order = 0)]
  public class HealthEffect : EffectStrategy
  {
    [SerializeField] private float healthChange = 10f;

    public override void StartEffect(AbilityData data, Action finished)
    {

      foreach (GameObject target in data.GetTargets())
      {
        Health health = target.GetComponent<Health>();
        if (health == null) continue;

        if (healthChange < 0)
        {
          health.TakeDamage(data.GetUser(), -healthChange);

        }
        else
        {
          health.Heal(healthChange);
        }

      }

      finished();
    }
  }
}