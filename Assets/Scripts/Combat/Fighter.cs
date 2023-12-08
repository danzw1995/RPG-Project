using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat
{
  public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
  {
    // 右手装备点
    [SerializeField] private Transform rightHandTransform = null;
    // 左手装备点
    [SerializeField] private Transform lefthandTransform = null;
    [SerializeField] private Weapon defaultWeapon = null;
    [SerializeField] private string defaultWeaponName = "Unarmed";
    // 攻击间隔时间
    [SerializeField] private float timeBetweenAttacks = 1f;
    // 上次攻击时间
    private float timeSinceLastAttack = Mathf.Infinity;
    private Health target;
    private Animator animator;

    private Mover mover;
    private ActionScheduler actionScheduler;

    private LazyValue<Weapon> currentWeapon;

    private void Awake()
    {
      mover = GetComponent<Mover>();
      actionScheduler = GetComponent<ActionScheduler>();

      animator = GetComponent<Animator>();

      currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
    }
    private void Start()
    {
      currentWeapon.ForceInit();
    }

    private void Update()
    {
      timeSinceLastAttack += Time.deltaTime;
      if (target == null) return;
      if (target.IsDead()) return;
      // 计算当前位置与target之间的距离,距离大于weaponRange时移动  
      if (!GetIsInRange())
      {
        mover.MoveTo(target.transform.position, 1f);
      }
      else
      {
        mover.Cancel();
        AttackBehavior();
      }
    }

    private Weapon SetupDefaultWeapon()
    {
      Weapon weapon = Resources.Load<Weapon>(defaultWeaponName);
      AttackWeapon(weapon);
      return weapon;
    }

    private void AttackWeapon(Weapon weapon)
    {
      weapon.Spawn(rightHandTransform, lefthandTransform, animator);
    }

    public void EquipWeapon(Weapon weapon)
    {
      currentWeapon.value = weapon;
      AttackWeapon(weapon);
    }

    public Health GetTarget()
    {
      return target;
    }

    private void AttackBehavior()
    {
      if (timeSinceLastAttack >= timeBetweenAttacks)
      {
        if (target != null)
        {
          transform.LookAt(target.transform);
        }
        AttackTrigger();
        timeSinceLastAttack = 0f;
      }
    }

    private void AttackTrigger()
    {
      animator.ResetTrigger("stopAttack");
      animator.SetTrigger("attack");
    }

    // Animation Event
    private void Hit()
    {
      if (target != null)
      {
        float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

        if (currentWeapon.value.HasProjectile())
        {
          currentWeapon.value.LaunchProjectile(rightHandTransform, lefthandTransform, target, gameObject,damage);
        } else
        {
          target.TakeDamage(damage, gameObject);

        }

      }
    }

    private void Shoot()
    {
      Hit();
    }

    private bool GetIsInRange()
    {
      return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.value.GetRange();
    }

    // 判断目标能否被攻击
    public bool CanAttack(GameObject combatTarget)
    {
      if (combatTarget == null)
      {
        return false;
      }
      Health health = combatTarget.GetComponent<Health>();
      return health != null && !health.IsDead();
    }

    // 攻击
    public void Attack(GameObject combatTarget)
    {
      actionScheduler.StartAction(this);
      target = combatTarget.GetComponent<Health>();
    }

    public void Cancel()
    {
      target = null;
      StopAttackTrigger();
    }

    private void StopAttackTrigger()
    {
      animator.ResetTrigger("attack");
      animator.SetTrigger("stopAttack");
    }

    public object CaptureState()
    {
      return currentWeapon.value.name;
    }

    public void RestoreState(object state)
    {
      string weaponName = (string)state;
      Weapon weapon = Resources.Load<Weapon>(weaponName);
      EquipWeapon(weapon);
    }

    public IEnumerable<float> GetAdditiveModifiers(Stat stat)
    {
      if (stat == Stat.Damage)
      {
        yield return currentWeapon.value.GetDamage();
      }
    }

    public IEnumerable<float> GetPercentModifiers(Stat stat)
    {
      if (stat == Stat.Damage)
      {
        yield return currentWeapon.value.GetPercentAgeDounce();
      }
    }
  }
}
