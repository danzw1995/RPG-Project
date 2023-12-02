

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

    public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
    {
      if (equippedPrefab != null)
      {
        Instantiate(equippedPrefab, isRightHanded ? rightHandTransform : leftHandTransform);
      }
      if (animatorOverride != null)
      {
        animator.runtimeAnimatorController = animatorOverride;

      }
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
