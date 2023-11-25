﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
  public class AIController : MonoBehaviour
  {
    [SerializeField] private float chaseDistance = 5f;

    private GameObject player;
    private Fighter fighter;
    private Health health;

    private void Start()
    {
      player = GameObject.FindWithTag("Player");
      fighter = GetComponent<Fighter>();
      health = GetComponent<Health>();
    }
    // Update is called once per frame
    private void Update()
    {
      if (health.IsDead()) return;

      if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
      {
        fighter.Attack(player);
      }
      else
      {
        fighter.Cancel();
      }
    }

    private bool InAttackRangeOfPlayer()
    {
      float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
      return distanceToPlayer < chaseDistance;
    }
  }

}
