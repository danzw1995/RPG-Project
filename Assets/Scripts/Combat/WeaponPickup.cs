﻿using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{

  public class WeaponPickup : MonoBehaviour, IRaycastable
  {
    [SerializeField] private WeaponConfig weapon = null;
    [SerializeField] private float respawnTime = 5f;
    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.tag == "Player")
      {
        Pickup(other.gameObject.GetComponent<Fighter>());
      }
    }

    private void Pickup(Fighter fighter)
    {
      fighter.EquipWeapon(weapon);

      StartCoroutine(HideForSeconds(respawnTime));
    }
    private IEnumerator HideForSeconds(float seconds)
    {
      ShowPickup(false);
      yield return new WaitForSeconds(seconds);
      ShowPickup(true);
    }

    private void ShowPickup(bool shouldShow)
    {
      GetComponent<Collider>().enabled = shouldShow;
      foreach (Transform child in transform)
      {
        child.gameObject.SetActive(shouldShow);
      }
    }

    public bool HandleRaycast(PlayerController playerController)
    {
      if (Input.GetMouseButtonDown(0))
      {
        Pickup(playerController.GetComponent<Fighter>());
      }
      return true;
    }

    public CursorType GetCursorType()
    {
      return CursorType.Pickup;
    }
  }

}
