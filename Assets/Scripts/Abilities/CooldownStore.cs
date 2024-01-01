

using System;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Abilities
{
  public class CooldownStore : MonoBehaviour
  {
    Dictionary<InventoryItem, float> cooldownTimes = new Dictionary<InventoryItem, float>();
    Dictionary<InventoryItem, float> initalCooldownTimes = new Dictionary<InventoryItem, float>();

    private void Update()
    {
      List<InventoryItem> keys = new List<InventoryItem>(cooldownTimes.Keys);
      foreach (InventoryItem ability in keys)
      {
        cooldownTimes[ability] -= Time.deltaTime;
        if (cooldownTimes[ability] < 0)
        {
          cooldownTimes.Remove(ability);
          initalCooldownTimes.Remove(ability);
        }
      }
    }

    public void StartCooldown(InventoryItem ability, float cooldownTime)
    {
      cooldownTimes[ability] = cooldownTime;
      initalCooldownTimes[ability] = cooldownTime;
    }

    public float GetTimeRemaining(InventoryItem ability)
    {
      if (!cooldownTimes.ContainsKey(ability)) return 0;
      return cooldownTimes[ability];
    }

    public float GetFractionRemaining(InventoryItem inventoryItem)
    {
      if (inventoryItem == null)
      {
        return 0;
      }

      if (!cooldownTimes.ContainsKey(inventoryItem)) { return 0; }

      return cooldownTimes[inventoryItem] / initalCooldownTimes[inventoryItem];
    }
  }
}
