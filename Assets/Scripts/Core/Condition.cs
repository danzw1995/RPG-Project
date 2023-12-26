using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Core
{
  [Serializable]
  public class Condition
  {

    [SerializeField]
    private Disjunction[] and;

    public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
    {
      foreach (Disjunction disjunction in and)
      {
        if (!disjunction.Check(evaluators))
        {
          return false;
        }
      }
      return true;
    }


    [System.Serializable]
    private class Disjunction
    {
      [SerializeField]
      Predicate[] or;

      public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
      {
        foreach (Predicate predicate in or)
        {
          if (predicate.Check(evaluators))
          {
            return true;
          }
        }
        return false;
      }

    }

    [System.Serializable]
    public class Predicate
    {
      [SerializeField] private string predicate;
      [SerializeField] private string[] parameters;

      [SerializeField] private bool negate = false;

      public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
      {
        foreach (var evaluator in evaluators)
        {
          bool? result = evaluator.Evaluate(predicate, parameters);
          if (result == null)
          {
            continue;
          }

          if (result == negate)
          {
            return false;
          }
        }
        return true;
      }
    }

  }

}
