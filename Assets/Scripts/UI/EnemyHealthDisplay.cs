using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
  public class EnemyHealthDisplay : MonoBehaviour
  {
    private Fighter fighter;

    private void Awake()
    {
      fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
    }

    private void Update()
    {
      Health target = fighter.GetTarget();
      if (target == null)
      {
        GetComponent<Text>().text = "NA";
      } else
      {
        GetComponent<Text>().text = string.Format("{0:0.0}%", target.GetHealthAge().ToString());

      }
    }
  }

}
