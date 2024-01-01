

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
  public class AbilityData
  {
    private GameObject user;
    private IEnumerable<GameObject> targets;

    private Vector3 targetPoint;

    public AbilityData(GameObject user)
    {
      this.user = user;
    }

    public AbilityData(GameObject user, IEnumerable<GameObject> targets)
    {
      this.user = user;
      this.targets = targets;

    }
    public GameObject GetUser()
    {
      return user;
    }

    public IEnumerable<GameObject> GetTargets()
    {
      return targets;
    }

    public void SetTargets(IEnumerable<GameObject> enumerable)
    {
      targets = enumerable;
    }

    public Vector3 GetTargetPoint()
    {
      return targetPoint;
    }

    public void SetTargetPoint(Vector3 point)
    {
      targetPoint = point;
    }

    public void StartCoroutine(IEnumerator coroutine)
    {
      user.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
    }
  }
}