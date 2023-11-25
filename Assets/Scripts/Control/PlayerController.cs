using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
  public class PlayerController : MonoBehaviour
  {

    private Camera mainCamera;

    private Mover mover;
    private Fighter fighter;

    private void Awake()
    {
      mainCamera = Camera.main;
      mover = GetComponent<Mover>();
      fighter = GetComponent<Fighter>();
    }

    private void Update()
    {
      if (InteractWithCombat()) return;
      if (InteractWithMovement()) return;

      print("nothing to do.");

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
        if (Input.GetMouseButtonDown(0))
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
          mover.StartMoveAction(hit.point);
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
