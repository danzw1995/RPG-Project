using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{

  [CreateAssetMenu(fileName = "Quest", menuName = "RPG/Quest", order = 0)]
  public class Quest : ScriptableObject
  {
    [SerializeField] private List<string> objectives;

    public string GetTitle()
    {
      return name;
    }

    public int GetObjectiveCount()
    {
      return objectives.Count;
    }

    public IEnumerable<string> GetObjectives()
    {
      return objectives;
    }

    public bool HasObjective(string objective)
    {
      return objectives.Contains(objective);
    }
  }
}

