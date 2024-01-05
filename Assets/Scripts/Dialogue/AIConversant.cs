using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Dialogue
{
  public class AIConversant : MonoBehaviour, IRaycastable
  {
    [SerializeField] private Dialogue dialogue = null;

    [SerializeField] private string conversantName = "";
    public CursorType GetCursorType()
    {
      return CursorType.Dialogue;
    }

    public bool HandleRaycast(PlayerController callingController)
    {
      if (dialogue == null)
      {
        return false;
      }
      Health health = GetComponent<Health>();
      if (health && health.IsDead()) return false;
      if (Input.GetMouseButton(0))
      {
        callingController.GetComponent<PlayerConversant>().StartDialogue(this, dialogue);

      }
      return true;
    }

    public string GetConversantName()
    {
      return conversantName;
    }
  }

}
