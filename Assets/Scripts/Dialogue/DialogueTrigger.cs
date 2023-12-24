using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
  public class DialogueTrigger : MonoBehaviour
  {
    [SerializeField] private string action;
    [SerializeField] private UnityEvent onTrigger;

    public void OnTrigger(string actionTrigger)
    {
      if (actionTrigger == action)
      {
        onTrigger.Invoke();
      }
    }
  }

}
