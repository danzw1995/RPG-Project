using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Control;
using UnityEngine;


namespace RPG.Shops
{
  public class Shop : MonoBehaviour, IRaycastable
  {
    [SerializeField] private string shopName = null;

    [SerializeField] private StockItemConfig[] stockItemConfigs;
    public Action onChange;

    [System.Serializable]
    private class StockItemConfig
    {
      public InventoryItem item;
      public int initialStock;
      [Range(0, 100)]
      public float buyingDiscountPercentage;
    }

    private Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();

    private Shopper currentShopper = null;

    public string GetShopName()
    {
      return shopName;
    }

    public void SetShopper(Shopper shopper)
    {
      currentShopper = shopper;
    }
    public IEnumerable<ShopItem> GetFilteredItems()
    {
      foreach (StockItemConfig config in stockItemConfigs)
      {
        float price = config.item.GetPrice() * (1 - config.buyingDiscountPercentage / 100);
        int quantityTransaction;
        transaction.TryGetValue(config.item, out quantityTransaction);
        yield return new ShopItem(config.item, config.initialStock, price, quantityTransaction);
      }
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
      Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
      if (shopperInventory == null) return;

      var transactionSnapShot = new Dictionary<InventoryItem, int>(transaction);

      foreach (InventoryItem item in transactionSnapShot.Keys)
      {
        for (int i = 0; i < transactionSnapShot[item]; i++)
        {
          bool success = shopperInventory.AddToFirstEmptySlot(item, 1);
          if (success)
          {
            AddToTransaction(item, -1);
          }
        }
      }
    }

    public float TransactionTotal()
    {
      return 0;
    }

    public void AddToTransaction(InventoryItem item, int quantity)
    {
      if (!transaction.ContainsKey(item))
      {
        transaction.Add(item, 0);
      }

      transaction[item] += quantity;
      if (transaction[item] < 0)
      {
        transaction[item] = 0;
      }

      if (onChange != null)
      {
        onChange();
      }

    }

    public CursorType GetCursorType()
    {
      return CursorType.Shop;
    }

    public bool HandleRaycast(PlayerController callingController)
    {
      if (Input.GetMouseButton(0))
      {
        callingController.GetComponent<Shopper>().SetActiveShop(this);

      }

      return true;
    }


  }

}
