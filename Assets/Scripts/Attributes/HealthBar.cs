using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
  public class HealthBar : MonoBehaviour
  {
    [SerializeField] private Health health = null;

    [SerializeField] private RectTransform foreground = null;

    [SerializeField] private Canvas rootCanvas = null;

    // Update is called once per frame
    void Update()
    {
      float franction = health.GetHealthFranction();
      if (Mathf.Approximately(franction, 1)|| Mathf.Approximately(franction, 0))
      {
        rootCanvas.enabled = false;
      }  else
      {
        rootCanvas.enabled = true;
        foreground.localScale = new Vector3(franction, 1, 1);
      }

    }
  }
}

