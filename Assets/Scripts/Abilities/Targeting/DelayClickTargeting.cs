
using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Targeting
{

  [CreateAssetMenu(fileName = "DelayClick Targeting", menuName = "Abilities/Targeting/DelayClick", order = 0)]
  public class DelayClickTargeting : TargetingStrategy
  {
    [SerializeField] private Vector2 cursorHotspot;
    [SerializeField] private Texture2D cursorType;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float areaEffectRadius;

    [SerializeField] private Transform summonCirclePrefab = null;

    private Transform summonCircle;
    public override void StartTargeting(GameObject user, Action<IEnumerable<GameObject>> finished)
    {
      PlayerController playerController = user.GetComponent<PlayerController>();
      playerController.StartCoroutine(Targeting(user, playerController, finished));
    }

    private IEnumerator Targeting(GameObject user, PlayerController playerController, Action<IEnumerable<GameObject>> finished)
    {
      playerController.enabled = false;
      if (summonCircle == null)
      {
        summonCircle = Instantiate(summonCirclePrefab);
        summonCircle.localScale = new Vector3(areaEffectRadius * 2, 1, areaEffectRadius * 2);
      }
      else
      {
        summonCircle.gameObject.SetActive(true);
      }
      while (true)
      {
        Cursor.SetCursor(cursorType, cursorHotspot, CursorMode.Auto);

        RaycastHit raycastHit;
        if (Physics.Raycast(PlayerController.GetMouseRay(), out raycastHit, 1000, layerMask))
        {
          summonCircle.position = raycastHit.point;

          if (Input.GetMouseButtonDown(0))
          {
            yield return new WaitWhile(() => Input.GetMouseButton(0));
            summonCircle.gameObject.SetActive(false);
            playerController.enabled = true;
            finished(GetGameObjectsInRadius(raycastHit.point));
            yield break;

          }
        }
        yield return null;
      }
    }

    private IEnumerable<GameObject> GetGameObjectsInRadius(Vector2 point)
    {

      RaycastHit[] hits = Physics.SphereCastAll(point, areaEffectRadius, Vector3.up, 0);

      foreach (var hit in hits)
      {
        yield return hit.collider.gameObject;
      }

    }
  }
}