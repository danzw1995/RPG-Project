

using GameDevTV.Inventories;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
  public class RandomDropper : ItemDropper
  {
    [Tooltip("How far can the pickups be scattered from the dropper.")]
    [SerializeField] private float scatterDistance = 1f;
    [SerializeField] private DropLibrary dropLibrary;
    private const int ATTEMPTS = 30;
    protected override Vector3 GetDropLocation()
    {

      for (int i = 0; i < ATTEMPTS; i++)
      {
        Vector3 randomPoint = transform.position + UnityEngine.Random.insideUnitSphere * scatterDistance;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
        {
          return hit.position;
        }
      }

      return transform.position;
    }
    public void RandomDrop()
    {

      BaseStats baseStats = GetComponent<BaseStats>();
      var drops = dropLibrary.GetRandomDrops(baseStats.GetLevel());

      foreach (var drop in drops)
      {
        DropItem(drop.item, drop.number);

      }

    }
  }
}