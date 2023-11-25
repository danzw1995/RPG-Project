using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat 
{
    public class Fighter : MonoBehaviour, IAction {
      // 武器的使用范围
      [SerializeField] private float weaponRange = 2f;
      // 武器伤害值
      [SerializeField] private float weaponDamage = 5f;
      // 攻击间隔时间
      [SerializeField] private float timeBetweenAttacks = 1f;
      // 上次攻击时间
      private float timeSinceLastAttack = 0f;
      private Health target;
      private Animator animator;

      private Mover mover;
      private ActionScheduler actionScheduler;

      private void Awake() {
        mover = GetComponent<Mover>();
        actionScheduler = GetComponent<ActionScheduler>();
        
        animator = GetComponent<Animator>();
      }

      private void Update() {
        timeSinceLastAttack += Time.deltaTime;
        if (target == null) return;
        if (target.IsDead()) return;
        // 计算当前位置与target之间的距离,距离小于weaponRange时停止移动     
        if (Vector3.Distance(transform.position, target.transform.position) <  weaponRange) {
          mover.Cancel();
          AttackBehavior();
        } else {
          mover.MoveTo(target.transform.position);
        }
    }

      private void AttackBehavior() {
        if (timeSinceLastAttack >= timeBetweenAttacks) {
          if (target != null) {
            transform.LookAt(target.transform);
          }
          AttackTrigger();
          timeSinceLastAttack = 0f;
        }
      }

      private void AttackTrigger() {
        animator.ResetTrigger("stopAttack");
        animator.SetTrigger("attack");
      }

      // Animation Event
      private void Hit() {
        if (target != null) {
          target.TakeDamage(weaponDamage);
        }
      }

      // 判断目标能否被攻击
      public bool CanAttack(CombatTarget combatTarget) {
        if (combatTarget == null) {
          return false;
        }
        Health health = combatTarget.GetComponent<Health>();
        return health != null && !health.IsDead();
      }

      // 攻击
      public void Attack (CombatTarget combatTarget) {
        actionScheduler.StartAction(this);
        target = combatTarget.GetComponent<Health>();
      }

      public void Cancel() {
        target = null;
        StopAttackTrigger();
      }

      private void StopAttackTrigger() {
        animator.ResetTrigger("attack");
        animator.SetTrigger("stopAttack");
      }
    } 
}
