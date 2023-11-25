using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat 
{
    public class Fighter : MonoBehaviour, IAction {
      [SerializeField] private float weaponRange = 2f;
      [SerializeField] private float weaponDamage = 5f;
      [SerializeField] private float timeBetweenAttacks = 1f;

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
          animator.SetTrigger("attack");
          timeSinceLastAttack = 0f;
        }
      }

      // Animation Event
      private void Hit() {
        if (target != null) {
          target.TakeDamage(weaponDamage);
        }
      }

      public void Attack (CombatTarget combatTarget) {
        actionScheduler.StartAction(this);
        target = combatTarget.GetComponent<Health>();
      }

      public void Cancel() {
        animator.SetTrigger("stopAttack");
        target = null;
      }
    } 
}
