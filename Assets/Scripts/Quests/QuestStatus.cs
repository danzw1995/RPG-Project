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

    [System.Serializable]
    private class QuestRecord
    {
      public string questName;
      public List<string> objectives;
    }

    public QuestStatus(Quest quest)
    {
      this.quest = quest;
    }

    public QuestStatus(object stateObject)
    {
      QuestRecord record = stateObject as QuestRecord;

      quest = Quest.GetQuestByName(record.questName);

      completedObjectives = record.objectives;
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

    public object CaptureState()
    {
      QuestRecord questRecord = new QuestRecord
      {
        questName = quest.name,
        objectives = completedObjectives
      };
      return questRecord;
    }
  }

}
