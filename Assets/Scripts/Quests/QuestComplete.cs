﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Quests
{
  public class QuestComplete : MonoBehaviour
  {
    [SerializeField] private Quest quest;
    [SerializeField] private string objective;

    public void CompleteObjective()
    {
      QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
      questList.CompleteObjective(quest, objective);
    }
  }

}
