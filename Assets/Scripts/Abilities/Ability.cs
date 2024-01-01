using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Core;
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

    [SerializeField] private float manaCost = 0;

    public override void Use(GameObject user)
    {
      CooldownStore cooldownStore = user.GetComponent<CooldownStore>();

      Mana mana = user.GetComponent<Mana>();
      if (cooldownStore.GetTimeRemaining(this) > 0)
      {
        return;
      }

      if (mana.GetMana() < manaCost)
      {
        return;
      }

      AbilityData data = new AbilityData(user);

      ActionScheduler actionScheduler = user.GetComponent<ActionScheduler>();
      actionScheduler.StartAction(data);
      targetingStrategy.StartTargeting(data, () => TargetAquired(data));
    }

    private void TargetAquired(AbilityData data)
    {
      if (data.IsCanceled()) return;
      GameObject user = data.GetUser();
      CooldownStore cooldownStore = user.GetComponent<CooldownStore>();
      Mana mana = user.GetComponent<Mana>();

      if (!mana.UseMana(manaCost)) return;

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
