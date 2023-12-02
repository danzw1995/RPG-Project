using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;

namespace RPG.Combat
{
  public class Fighter : MonoBehaviour, IAction
  {
    // 右手装备点
    [SerializeField] private Transform rightHandTransform = null;
    // 左手装备点
    [SerializeField] private Transform lefthandTransform = null;
    [SerializeField] private Weapon defaultWeapon = null;
    // 攻击间隔时间
    [SerializeField] private float timeBetweenAttacks = 1f;
    // 上次攻击时间
    private float timeSinceLastAttack = Mathf.Infinity;
    private Health target;
    private Animator animator;

    private Mover mover;
    private ActionScheduler actionScheduler;

    private Weapon currentWeapon = null;

    private void Awake()
    {
      mover = GetComponent<Mover>();
      actionScheduler = GetComponent<ActionScheduler>();

      animator = GetComponent<Animator>();
    }
    private void Start()
    {

      EquipWeapon(defaultWeapon);
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
    public void EquipWeapon(Weapon weapon)
    {
      currentWeapon = weapon;
      if (currentWeapon == null) return;
      weapon.Spawn(rightHandTransform, lefthandTransform, animator);

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
        target.TakeDamage(currentWeapon.GetDamage());
      }
    }

    private bool GetIsInRange()
    {
      return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
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
  }
}
