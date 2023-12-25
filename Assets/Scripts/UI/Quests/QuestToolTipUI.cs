using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;


namespace RPG.UI.Quests
{
  public class QuestToolTipUI : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI title = null;

    [SerializeField] private Transform objectiveContainer = null;

    [SerializeField] private GameObject objectivePrefab = null;
    [SerializeField] private GameObject objectiveIncompletedPrefab = null;
    public void Setup(QuestStatus status)
    {

      Quest quest = status.GetQuest();
      title.text = quest.GetTitle();

      objectiveContainer.DetachChildren();

      foreach (string objective in quest.GetObjectives())
      {
        GameObject prefab = status.IsCompleteObjective(objective) ? objectivePrefab : objectiveIncompletedPrefab;

        GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);
        TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
        objectiveText.text = objective;
      }

    }
  }
}

