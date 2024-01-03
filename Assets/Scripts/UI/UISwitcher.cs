using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
  public class UISwitcher : MonoBehaviour
  {
    [SerializeField] private GameObject entryPoint = null;
    // Start is called before the first frame update
    void Start()
    {
      SwitchTo(entryPoint);
    }

    public void SwitchTo(GameObject switchTo)
    {
      if (switchTo.transform.parent != transform) return;
      foreach (Transform child in transform)
      {
        child.gameObject.SetActive(child.gameObject == switchTo);
      }
    }


  }
}

