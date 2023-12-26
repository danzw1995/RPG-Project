using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Quests
{

  [CreateAssetMenu(fileName = "Quest", menuName = "RPG/Quest", order = 0)]
  public class Quest : ScriptableObject
  {
    [SerializeField] private List<Objective> objectives = new List<Objective>();

    [SerializeField] private List<Reward> rewards = new List<Reward>();

    [Serializable]
    public class Reward
    {
      public int number;
      public InventoryItem item;
    }

    [Serializable]
    public class Objective
    {
      public string reference;
      public string description;
    }

    public string GetTitle()
    {
      return name;
    }

    public int GetObjectiveCount()
    {
      return objectives.Count;
    }

    public IEnumerable<Objective> GetObjectives()
    {
      return objectives;
    }

    public IEnumerable<Reward> GetRewards()
    {
      return rewards;
    }

    public bool HasObjective(string objectiveRef)
    {

      foreach (var objective in objectives)
      {
        if (objective.reference == objectiveRef)
        {
          return true;
        }
      }
      return false;
    }

    public static Quest GetQuestByName(string questName)
    {
      foreach (Quest quest in Resources.LoadAll<Quest>(""))
      {
        if (quest.name == questName)
        {
          return quest;
        }
      }
      return null;
    }

    public string GetRewardText()
    {
      string rewardStr = "";

      foreach (var reward in GetRewards())
      {
        if (reward.number > 1)
        {
          rewardStr += reward.item.GetDisplayName() + " x " + reward.number.ToString() + ", ";
        }
        else
        {
          rewardStr += reward.item.GetDisplayName() + ", ";
        }
      }
      if (rewardStr == "")
      {
        rewardStr = "No Rewards.";
      }

      return rewardStr;
    }
  }
}

