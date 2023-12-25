using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;


namespace RPG.UI.Quests
{
  public class QuestItemUI : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI progress;

    private QuestStatus status;
    public void Setup(QuestStatus status)
    {
      this.status = status;
      Quest quest = status.GetQuest();

      title.text = quest.GetTitle();
      progress.text = status.GetCompleteObjectivesCount() + "/" + quest.GetObjectiveCount();
    }

    public QuestStatus GetStatus()
    {
      return status;
    }
  }

}
