
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Filters
{
  [CreateAssetMenu(fileName = "Tag Filter", menuName = "Abilities/Filters/Tag", order = 0)]
  public class TagFilter : FilterStrategy
  {
    [SerializeField] private string tagName = "";
    public override IEnumerable<GameObject> FilterTarget(IEnumerable<GameObject> gameObjectToFilter)
    {
      foreach (GameObject obj in gameObjectToFilter)
      {
        if (obj.tag == tagName)
        {
          yield return obj;
        }
      }
    }
  }
}