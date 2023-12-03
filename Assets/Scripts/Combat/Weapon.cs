﻿

using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
  [CreateAssetMenu(fileName = "Weapons", menuName = "Weapons/Make New Weapon", order = 0)]
  public class Weapon: ScriptableObject
  {
    [SerializeField] private AnimatorOverrideController animatorOverride = null;

    [SerializeField] private GameObject equippedPrefab = null;

    // 武器的使用范围
    [SerializeField] private float weaponRange = 2f;
    // 武器伤害值
    [SerializeField] private float weaponDamage = 5f;
    // 是否为右手武器
    [SerializeField] private bool isRightHanded = true;

    // 武器发射物
    [SerializeField] private Projectile projectile = null;

    private const string weaponName = "Weapon";

    public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
    {
      DestroyOldWeapon(rightHandTransform, leftHandTransform);

      if (equippedPrefab != null)
      {
        GameObject weapon =  Instantiate(equippedPrefab, GetTransform(rightHandTransform, leftHandTransform));
        weapon.name = weaponName;
      }
      if (animatorOverride != null)
      {
        animator.runtimeAnimatorController = animatorOverride;

      }
    }

    private void DestroyOldWeapon(Transform rightHandTransform, Transform leftHandTransform)
    {
      Transform oldWeapon = rightHandTransform.Find(weaponName);
      if (oldWeapon == null)
      {
        oldWeapon = leftHandTransform.Find(weaponName);
      }
      if (oldWeapon == null) return;
      oldWeapon.name = "DESTROYING";
      Destroy(oldWeapon.gameObject);
    }

    public void LaunchProjectile(Transform rightHandTransform, Transform leftHandTransform, Health health)
    {
     Projectile projectileInstance =  Instantiate(projectile, GetTransform(rightHandTransform, leftHandTransform).position, Quaternion.identity);
      projectileInstance.SetTarget(health, weaponDamage);
    }

    public Transform GetTransform(Transform rightHandTransform, Transform leftHandTransform)
    {
      return isRightHanded ? rightHandTransform : leftHandTransform;
    }

    public bool HasProjectile()
    {
      return projectile != null;
    }

    public float GetDamage()
    {
      return weaponDamage;
    }
    public float GetRange()
    {
      return weaponRange;
    }
  }
}
