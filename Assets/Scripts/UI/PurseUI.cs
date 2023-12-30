using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using TMPro;
using UnityEngine;


namespace RPG.UI
{
  public class PurseUI : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI balanceField = null;

    private Purse purse;

    private void Start()
    {
      purse = GameObject.FindGameObjectWithTag("Player").GetComponent<Purse>();

      if (purse != null)
      {
        purse.onChange += UpdateBalance;
      }
      UpdateBalance();
    }

    private void UpdateBalance()
    {
      balanceField.text = $"${purse.GetBalance():N2}";
    }
  }

}
