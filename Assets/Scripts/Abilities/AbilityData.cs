

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
  public class AbilityData
  {
    private GameObject user;
    private IEnumerable<GameObject> targets;

    public AbilityData(GameObject user)
    {
      this.user = user;
    }

    public AbilityData(GameObject user, IEnumerable<GameObject> targets)
    {
      this.user = user;
      this.targets = targets;

    }
    public GameObject GetUser()
    {
      return user;
    }

    public IEnumerable<GameObject> GetTargets()
    {
      return targets;
    }

    public void SetTargets(IEnumerable<GameObject> enumerable)
    {
      targets = enumerable;
    }

  }
}