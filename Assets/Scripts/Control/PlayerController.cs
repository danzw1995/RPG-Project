using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using RPG.Attributes;

namespace RPG.Control
{
  public class PlayerController : MonoBehaviour
  {

    private Camera mainCamera;

    private Mover mover;
    private Fighter fighter;
    private Health health;

    private void Awake()
    {
      mainCamera = Camera.main;
      mover = GetComponent<Mover>();
      fighter = GetComponent<Fighter>();
      health = GetComponent<Health>();
    }

    private void Update()
    {
      if (health.IsDead()) return;
      if (InteractWithCombat()) return;
      if (InteractWithMovement()) return;


    }

    private bool InteractWithCombat()
    {
      RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
      foreach (RaycastHit hit in hits)
      {
        CombatTarget target = hit.transform.GetComponent<CombatTarget>();
        if (target == null)
        {
          continue;
        }
        // 判断目标能否被攻击
        if (!fighter.CanAttack(target.gameObject))
        {
          continue;
        }
        if (Input.GetMouseButton(0))
        {
          GetComponent<Fighter>().Attack(target.gameObject);
        }
        return true;
      }


      return false;
    }

    private bool InteractWithMovement()
    {
      RaycastHit hit;

      if (Physics.Raycast(GetMouseRay(), out hit))
      {
        if (Input.GetMouseButton(0))
        {
          mover.StartMoveAction(hit.point, 1f);
        }
        return true;
      }
      return false;
    }


    private Ray GetMouseRay()
    {
      return mainCamera.ScreenPointToRay(Input.mousePosition);
    }
  }
}
