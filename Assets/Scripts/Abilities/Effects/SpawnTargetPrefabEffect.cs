using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Filters
{
  [CreateAssetMenu(fileName = "Spawn Target Prefab Effect", menuName = "Abilities/Effects/Spawn Target Prefab", order = 0)]
  public class SpawnTargetPrefabEffect : EffectStrategy
  {
    [SerializeField] private Transform targetPrefab = null;
    [SerializeField] private float delayToDestroy = -1;
    public override void StartEffect(AbilityData data, Action finished)
    {
      data.StartCoroutine(Effect(data, finished));
    }

    private IEnumerator Effect(AbilityData data, Action finished)
    {
      Transform targetInstance = Instantiate(targetPrefab);
      targetInstance.position = data.GetTargetPoint();
      if (delayToDestroy > 0)
      {
        yield return new WaitForSeconds(delayToDestroy);
        Destroy(targetInstance);

      }
      finished();

    }
  }
}