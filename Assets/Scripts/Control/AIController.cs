using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
  public class AIController : MonoBehaviour
  {
    // 追逐距离
    [SerializeField] private float chaseDistance = 5f;

    // 怀疑时间
    [SerializeField] private float suspicionTime = 3f;

    private GameObject player;
    private Fighter fighter;
    private Health health;
    private Mover mover;

    private ActionScheduler actionScheduler;

    private Vector3 guardPosition;

    private float timeSinceLastSawPlayer = Mathf.Infinity;

    private void Start()
    {
      player = GameObject.FindWithTag("Player");
      fighter = GetComponent<Fighter>();
      health = GetComponent<Health>();
      mover = GetComponent<Mover>();
      actionScheduler = GetComponent<ActionScheduler>();

      guardPosition = transform.position;
    }
    // Update is called once per frame
    private void Update()
    {
      if (health.IsDead()) return;

      if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
      {
        timeSinceLastSawPlayer = 0;
        AttackBehavior();
      }
      else if (timeSinceLastSawPlayer < suspicionTime)
      {
        SuspicionBehavior();
      }
      else
      {
        GuardBehavior();
      }

      timeSinceLastSawPlayer += Time.deltaTime;
    }

    private void AttackBehavior()
    {
      fighter.Attack(player);
    }

    private void SuspicionBehavior()
    {
      actionScheduler.CancelCurrentAction();

    }

    private void GuardBehavior()
    {
      mover.StartMoveAction(guardPosition);

    }

    private bool InAttackRangeOfPlayer()
    {
      float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
      return distanceToPlayer < chaseDistance;
    }

    // Call by unity
    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
  }

}
