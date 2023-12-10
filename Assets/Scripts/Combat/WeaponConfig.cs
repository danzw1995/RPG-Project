

using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
  [CreateAssetMenu(fileName = "Weapons", menuName = "Weapons/Make New Weapon", order = 0)]
  public class WeaponConfig: ScriptableObject
  {
    [SerializeField] private AnimatorOverrideController animatorOverride = null;

    [SerializeField] private Weapon equippedPrefab = null;

    // 武器的使用范围
    [SerializeField] private float weaponRange = 2f;
    // 武器伤害值
    [SerializeField] private float weaponDamage = 5f;
    // 伤害加成百分比
    [SerializeField] private int percentDounce = 0;
    // 是否为右手武器
    [SerializeField] private bool isRightHanded = true;

    // 武器发射物
    [SerializeField] private Projectile projectile = null;

    private const string weaponName = "Weapon";

    public Weapon Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
    {
      DestroyOldWeapon(rightHandTransform, leftHandTransform);

      Weapon weapon = null;

      if (equippedPrefab != null)
      {
        weapon =  Instantiate(equippedPrefab, GetTransform(rightHandTransform, leftHandTransform));
        weapon.gameObject.name = weaponName;
      }
      var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
      if (animatorOverride != null)
      {
        animator.runtimeAnimatorController = animatorOverride;

      } else if (overrideController != null)
      {
        // 重置动画 
        animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
      }

      return weapon;
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

    public void LaunchProjectile(Transform rightHandTransform, Transform leftHandTransform, Health health, GameObject instigator, float damage)
    {
     Projectile projectileInstance =  Instantiate(projectile, GetTransform(rightHandTransform, leftHandTransform).position, Quaternion.identity);
      projectileInstance.SetTarget(health, damage, instigator);
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

    public float GetPercentAgeDounce()
    {
      return percentDounce;
    }

    public float GetRange()
    {
      return weaponRange;
    }
  }
}
