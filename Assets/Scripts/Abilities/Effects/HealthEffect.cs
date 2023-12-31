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

    public override void StartEffect(GameObject user, IEnumerable<GameObject> targets, Action finished)
    {
      foreach (GameObject target in targets)
      {
        Health health = target.GetComponent<Health>();
        if (health == null) continue;

        if (healthChange < 0)
        {
          health.TakeDamage(user, -healthChange);

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