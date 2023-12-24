using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
  public class AggroGroup : MonoBehaviour
  {
    [SerializeField] private Fighter[] fighters = null;

    [SerializeField] private bool activeOnStart = false;

    private void Start()
    {
      Activate(activeOnStart);
    }

    public void Activate(bool shouldActive)
    {
      foreach (Fighter fighter in fighters)
      {
        if (fighter.TryGetComponent<CombatTarget>(out var target))
        {
          target.enabled = shouldActive;
        }
        fighter.enabled = shouldActive;
      }
    }
  }

}
