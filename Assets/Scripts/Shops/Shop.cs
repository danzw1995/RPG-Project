using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;


namespace RPG.Shops
{
  public class Shop : MonoBehaviour
  {

    public Action onChange;
    public class ShopItem
    {
      private InventoryItem item;

      private int availability;
      private float price;
      private int quantityInTransaction;
    }
    public IEnumerable<ShopItem> GetFilteredItems()
    {
      return default;
    }

    public void SelectFilter(ItemCategory category)
    {

    }

    public ItemCategory GetFilter()
    {
      return ItemCategory.None;
    }

    public void SelectMode(bool isBuying) { }

    public bool CanTransact()
    {
      return true;
    }

    public void ConfirmTransaction()
    {

    }

    public float TransactionTotal()
    {
      return 0;
    }

    public void AddToTransaction(InventoryItem item, int quantity)
    {

    }
  }

}
