
using System;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Abilities
{

  public abstract class FilterStrategy : ScriptableObject
  {
    public abstract IEnumerable<GameObject> FilterTarget(IEnumerable<GameObject> gameObjectToFilter);
  }
}