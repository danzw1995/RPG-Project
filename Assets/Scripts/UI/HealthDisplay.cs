using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
  public class HealthDisplay : MonoBehaviour
  {
    private Health health;
    [SerializeField] private Text healthValueText = null;

    private void Awake()
    {
      health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    private void Update()
    {
      healthValueText.text = string.Format("{0:0.0}%", health.GetHealthAge().ToString());
    }
  }

}
