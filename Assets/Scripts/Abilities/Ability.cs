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

    [SerializeField] private float cooldownTime = 0;

    public override void Use(GameObject user)
    {
      CooldownStore cooldownStore = user.GetComponent<CooldownStore>();
      if (cooldownStore.GetTimeRemaining(this) > 0)
      {
        return;
      }

      AbilityData data = new AbilityData(user);
      targetingStrategy.StartTargeting(data, () => TargetAquired(data));
    }

    private void TargetAquired(AbilityData data)
    {
      CooldownStore cooldownStore = data.GetUser().GetComponent<CooldownStore>();
      cooldownStore.StartCooldown(this, cooldownTime);

      foreach (FilterStrategy filterStrategy in filterStrategies)
      {
        data.SetTargets(filterStrategy.FilterTarget(data.GetTargets()));
      }

      foreach (EffectStrategy effectStrategy in effectStrategies)
      {
        effectStrategy.StartEffect(data, EffectFinished);
      }
    }

    private void EffectFinished()
    {
      Debug.Log("effect complete.");
    }
  }
}
