
using System;
using UnityEngine;


namespace RPG.Inventories
{

  public class Purse : MonoBehaviour
  {
    [SerializeField] private float startingBalance = 400f;

    private float balance = 0;

    public Action onChange;
    private void Awake()
    {
      balance = startingBalance;
      Debug.Log("balance: " + balance);
    }

    public float GetBalance()
    {
      return balance;
    }

    public void UpdateBalance(float amount)
    {
      balance += amount;
      if (onChange != null)
      {
        onChange();
      }

      Debug.Log("balance: " + balance);
    }
  }

}