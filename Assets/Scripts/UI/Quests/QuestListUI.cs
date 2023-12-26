using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;

namespace RPG.UI.Quests
{
  public class QuestListUI : MonoBehaviour
  {
    [SerializeField] private QuestItemUI questPrefab;

    private QuestList questList;
    // Start is called before the first frame update
    void Start()
    {
      questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
      questList.onUpdateQuest += UpdateQuests;
      UpdateQuests();

    }

    private void UpdateQuests()
    {

      foreach (Transform item in transform)
      {
        Destroy(item.gameObject);
      }

      foreach (QuestStatus status in questList.GetStatues())
      {
        QuestItemUI questItemUI = Instantiate(questPrefab, transform);
        questItemUI.Setup(status);
      }
    }
  }

}
