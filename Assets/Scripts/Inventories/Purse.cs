
using System;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using UnityEngine;


namespace RPG.Inventories
{

  public class Purse : MonoBehaviour, ISaveable, IItemStore
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

    public object CaptureState()
    {
      return balance;
    }

    public void RestoreState(object state)
    {
      balance = (float)state;
    }

    public int AddItems(InventoryItem item, int number)
    {
      if (item is CurrencyItem)
      {

        UpdateBalance(number);
        return number;
      }
      return 0;

    }
  }

}