using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Quests;
using UnityEngine;

namespace RPG.Quests
{
  [System.Serializable]
  public class QuestStatus
  {
    private Quest quest = null;
    private List<string> completedObjectives = new List<string>();

    public QuestStatus(Quest quest)
    {
      this.quest = quest;
    }

    public Quest GetQuest()
    {
      return quest;
    }

    public IEnumerable<string> GetCompletedObjectives()
    {
      return completedObjectives;
    }

    public bool IsCompleteObjective(string objective)
    {
      return completedObjectives.Contains(objective);
    }

    public string GetCompleteObjectivesCount()
    {
      return completedObjectives.Count.ToString();
    }

    public void CompleteObjective(string objective)
    {
      if (quest.HasObjective(objective))
      {
        completedObjectives.Add(objective);
      }
    }
  }

}
