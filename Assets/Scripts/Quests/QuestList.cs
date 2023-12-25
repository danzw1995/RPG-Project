using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Quests
{

  public class QuestList : MonoBehaviour
  {
    private List<QuestStatus> statuses = new List<QuestStatus>();

    public event Action onUpdateQuest;


    public IEnumerable<QuestStatus> GetStatues()
    {
      return statuses;
    }

    public void AddQuest(Quest quest)
    {
      if (HasQuest(quest))
      {
        return;
      }

      QuestStatus newStatus = new QuestStatus(quest);

      statuses.Add(newStatus);
      if (onUpdateQuest != null)
      {
        onUpdateQuest();
      }
    }

    private bool HasQuest(Quest quest)
    {
      foreach (QuestStatus status in statuses)
      {
        if (status.GetQuest() == quest)
        {
          return true;
        }
      }
      return false;
    }
  }
}
