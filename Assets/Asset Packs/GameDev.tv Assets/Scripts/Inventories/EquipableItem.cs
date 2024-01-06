using GameDevTV.Utils;
using UnityEngine;

namespace GameDevTV.Inventories
{
  /// <summary>
  /// An inventory item that can be equipped to the player. Weapons could be a
  /// subclass of this.
  /// </summary>
  [CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.InventorySystem/Equipable Item"))]
  public class EquipableItem : InventoryItem
  {
    // CONFIG DATA
    [Tooltip("Where are we allowed to put this item.")]
    [SerializeField] EquipLocation allowedEquipLocation = EquipLocation.Weapon;

    [SerializeField] private Condition equipCondition = null;

    public bool CanEquip(EquipLocation location, Equipment equipment)
    {
      if (location != allowedEquipLocation) return false;

      return equipCondition.Check(equipment.GetComponents<IPredicateEvaluator>());

    }

    public EquipLocation GetAllowedEquipLocation()
    {
      return allowedEquipLocation;
    }
  }
}