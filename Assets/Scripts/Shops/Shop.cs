using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Control;
using RPG.Inventories;
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
    private Dictionary<InventoryItem, int> stock = new Dictionary<InventoryItem, int>();

    private Shopper currentShopper = null;

    private void Awake()
    {
      foreach (StockItemConfig config in stockItemConfigs)
      {
        stock.Add(config.item, config.initialStock);
      }
    }

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
      return GetAllItems();
    }

    private IEnumerable<ShopItem> GetAllItems()
    {
      foreach (StockItemConfig config in stockItemConfigs)
      {
        float price = config.item.GetPrice() * (1 - config.buyingDiscountPercentage / 100);
        int quantityTransaction;
        transaction.TryGetValue(config.item, out quantityTransaction);

        int currentStock = stock[config.item];
        yield return new ShopItem(config.item, currentStock, price, quantityTransaction);
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
      if (IsTransactionEmpty()) return false;
      if (!HasSufficientFunds()) return false;
      if (!HasInventorySpace()) return false;

      return true;
    }

    public bool HasInventorySpace()
    {
      Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
      if (shopperInventory == null) return false;

      List<InventoryItem> flatItems = new List<InventoryItem>();

      foreach (ShopItem item in GetAllItems())
      {
        int quantity = item.GetQuantityInTransaction();
        for (int i = 0; i < quantity; i++)
        {
          flatItems.Add(item.GetItem());
        }
      }

      return shopperInventory.HasSpaceFor(flatItems);
    }

    public bool HasSufficientFunds()
    {
      Purse shopperPurse = currentShopper.GetComponent<Purse>();
      if (shopperPurse == null) return false;
      return shopperPurse.GetBalance() >= TransactionTotal();
    }

    public bool IsTransactionEmpty()
    {
      return transaction.Count == 0;
    }


    public void ConfirmTransaction()
    {
      Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
      Purse shopperPurse = currentShopper.GetComponent<Purse>();
      if (shopperInventory == null || shopperPurse == null) return;


      foreach (ShopItem item in GetAllItems())
      {
        int quantity = item.GetQuantityInTransaction();
        float price = item.GetPrice();

        for (int i = 0; i < quantity; i++)
        {
          if (shopperPurse.GetBalance() < price)
          {
            break;
          }

          bool success = shopperInventory.AddToFirstEmptySlot(item.GetItem(), 1);
          if (success)
          {
            AddToTransaction(item.GetItem(), -1);
            stock[item.GetItem()]--;
            shopperPurse.UpdateBalance(-price);
          }
        }
      }

      if (onChange != null)
      {
        onChange();
      }
    }

    public float TransactionTotal()
    {
      float total = 0;
      foreach (ShopItem shopItem in GetAllItems())
      {
        total += shopItem.GetPrice() * shopItem.GetQuantityInTransaction();
      }
      return total;
    }

    public void AddToTransaction(InventoryItem item, int quantity)
    {
      if (!transaction.ContainsKey(item))
      {
        transaction.Add(item, 0);
      }


      if (transaction[item] + quantity > stock[item])
      {
        transaction[item] = stock[item];
      }
      else
      {
        transaction[item] += quantity;
        if (transaction[item] < 0)
        {
          transaction.Remove(item);
        }
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
