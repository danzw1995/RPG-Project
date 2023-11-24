using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat 
{
    public class Fighter : MonoBehaviour, IAction {
      [SerializeField] private float weaponRange = 2f;
      private Transform target;
      private Animator animator;

      private Mover mover;
      private ActionScheduler actionScheduler;

      private void Awake() {
        mover = GetComponent<Mover>();
        actionScheduler = GetComponent<ActionScheduler>();
        
        animator = GetComponent<Animator>();
      }

      private void Update() {
        if (target != null) {
          // 计算当前位置与target之间的距离,距离小于weaponRange时停止移动     
          if (Vector3.Distance(transform.position, target.position) <  weaponRange) {
            mover.Cancel();
            AttackBehavior();
          } else {
            mover.MoveTo(target.position);
          }
        }
      }

      private AttackBehavior() {
        animator.SetTrigger("attack");
      }

      // Animation Event
      private void Hit() {

      }

      public void Attack (CombatTarget combatTarget) {
        actionScheduler.StartAction(this);
        target = combatTarget.transform;
      }

      public void Cancel() {
        target = null;
      }
    } 
}
