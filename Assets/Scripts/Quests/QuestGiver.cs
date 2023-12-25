using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Quests
{
  public class QuestGiver : MonoBehaviour
  {
    [SerializeField] private Quest quest = null;

    public void GiverQuest()
    {
      QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
      questList.AddQuest(quest);
    }
  }

}
