using System;
using System.Collections;
using RPG.Attributes;
using RPG.Combat;
using UnityEngine;

namespace RPG.Abilities.Filters
{
  [CreateAssetMenu(fileName = "Spawn Projectile Effect", menuName = "Abilities/Effects/Spawn Projectile", order = 0)]
  public class SpawnProjectileEffect : EffectStrategy
  {
    [SerializeField] private Projectile projectilePrefab = null;

    [SerializeField] private float damage = 10f;
    [SerializeField] private bool isRightHand = true;
    [SerializeField] private bool useTargetPoint = true;


    public override void StartEffect(AbilityData data, Action finished)
    {

      Fighter fighter = data.GetUser().GetComponent<Fighter>();

      Transform handTransform = fighter.GetHandTransform(isRightHand);

      if (useTargetPoint)
      {
        SpawnProejctilesForTargetPoint(data, handTransform);
      }
      else
      {
        SpawnProejctilesForTargets(data, handTransform);

      }


      finished();
    }

    private void SpawnProejctilesForTargetPoint(AbilityData data, Transform handTransform)
    {
      Projectile projectileInstance = Instantiate(projectilePrefab, handTransform.position, Quaternion.identity);

      projectileInstance.SetTarget(data.GetTargetPoint(), data.GetUser(), damage);
    }

    private void SpawnProejctilesForTargets(AbilityData data, Transform handTransform)
    {
      foreach (GameObject target in data.GetTargets())
      {
        Projectile projectileInstance = Instantiate(projectilePrefab, handTransform.position, Quaternion.identity);

        Health health = target.GetComponent<Health>();
        if (health == null) continue;

        projectileInstance.SetTarget(health, data.GetUser(), damage);
      }
    }
  }
}