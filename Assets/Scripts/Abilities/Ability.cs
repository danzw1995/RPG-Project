using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;



namespace RPG.Abilities
{
  [CreateAssetMenu(fileName = "New Ability", menuName = "Abilities/Ability", order = 0)]
  public class Ability : ActionItem
  {
    [SerializeField] private TargetingStrategy targetingStrategy = null;
    [SerializeField] private FilterStrategy[] filterStrategies = null;
    [SerializeField] private EffectStrategy[] effectStrategies = null;

    public override void Use(GameObject user)
    {
      targetingStrategy.StartTargeting(user, (IEnumerable<GameObject> targets) => TargetAquired(user, targets));
    }

    private void TargetAquired(GameObject user, IEnumerable<GameObject> targets)
    {
      foreach (FilterStrategy filterStrategy in filterStrategies)
      {
        targets = filterStrategy.FilterTarget(targets);
      }

      foreach (EffectStrategy effectStrategy in effectStrategies)
      {
        effectStrategy.StartEffect(user, targets, EffectFinished);
      }
    }

    private void EffectFinished()
    {
      Debug.Log("effect complete.");
    }
  }
}
