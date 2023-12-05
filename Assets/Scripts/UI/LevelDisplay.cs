using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
  public class LevelDisplay : MonoBehaviour
  {
    private BaseStats baseStats;

    private void Awake()
    {
      baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
    }

    private void Update()
    {
      GetComponent<Text>().text = baseStats.GetLevel().ToString();
    }
  }

}
