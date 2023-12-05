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

    private void Awake()
    {
      health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    private void Update()
    {
      GetComponent<Text>().text = string.Format("{0:0.0}%", health.GetHealthAge().ToString());
    }
  }

}
