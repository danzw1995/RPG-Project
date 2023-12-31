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

    [SerializeField] private float sellPercentage = 80f;
    public Action onChange;

    [Serializable]
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

    private bool isBuyingMode = true;

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
        float price = GetPrice(config);
        int quantityTransaction;
        transaction.TryGetValue(config.item, out quantityTransaction);

        int availability = GetAvailability(config.item);
        yield return new ShopItem(config.item, availability, price, quantityTransaction);
      }
    }

    private int GetAvailability(InventoryItem item)
    {
      if (isBuyingMode)
      {
        return stock[item];
      }
      else
      {
        return CountItemsInInventory(item);
      }
    }

    private int CountItemsInInventory(InventoryItem item)
    {
      Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
      if (shopperInventory == null) return 0;
      int total = 0;
      for (int i = 0; i < shopperInventory.GetSize(); i++)
      {
        if (shopperInventory.GetItemInSlot(i) == item)
        {
          total += shopperInventory.GetNumberInSlot(i);
        }
      }
      return total;
    }

    private float GetPrice(StockItemConfig config)
    {
      if (isBuyingMode)
      {
        return config.item.GetPrice() * (1 - config.buyingDiscountPercentage / 100);
      }
      else
      {
        return config.item.GetPrice() * (sellPercentage / 100);
      }

    }

    public void SelectFilter(ItemCategory category)
    {

    }

    public ItemCategory GetFilter()
    {
      return ItemCategory.None;
    }

    public void SelectMode(bool isBuying)
    {
      isBuyingMode = isBuying;
      if (onChange != null)
      {
        onChange();
      }
    }

    public bool IsBuyingMode()
    {
      return isBuyingMode;
    }

    public bool CanTransact()
    {
      if (IsTransactionEmpty()) return false;
      if (!HasSufficientFunds()) return false;
      if (!HasInventorySpace()) return false;

      return true;
    }

    public bool HasInventorySpace()
    {
      if (!isBuyingMode) return true;
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
      if (!isBuyingMode) return true;
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


      foreach (ShopItem shopItem in GetAllItems())
      {
        int quantity = shopItem.GetQuantityInTransaction();
        float price = shopItem.GetPrice();
        InventoryItem item = shopItem.GetItem();

        for (int i = 0; i < quantity; i++)
        {
          if (isBuyingMode)
          {
            BuyItem(shopperInventory, shopperPurse, item, price);

          }
          else
          {
            SellItem(shopperInventory, shopperPurse, item, price);
          }
        }
      }

      if (onChange != null)
      {
        onChange();
      }
    }

    private void SellItem(Inventory shopperInventory, Purse shopperPurse, InventoryItem item, float price)
    {
      int i = FindSlotInItem(shopperInventory, item);
      if (i < 0) return;

      AddToTransaction(item, -1);
      shopperInventory.RemoveFromSlot(i, 1);
      stock[item]++;
      shopperPurse.UpdateBalance(price);


    }

    private void BuyItem(Inventory shopperInventory, Purse shopperPurse, InventoryItem item, float price)
    {
      if (shopperPurse.GetBalance() < price)
      {
        return;
      }

      bool success = shopperInventory.AddToFirstEmptySlot(item, 1);
      if (success)
      {
        AddToTransaction(item, -1);
        stock[item]--;
        shopperPurse.UpdateBalance(-price);
      }
    }

    private int FindSlotInItem(Inventory shopperInventory, InventoryItem item)
    {
      for (int i = 0; i < shopperInventory.GetSize(); i++)
      {
        if (shopperInventory.GetItemInSlot(i) == item)
        {
          return i;
        }
      }
      return -1;
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

      int availability = GetAvailability(item);
      if (transaction[item] + quantity > availability)
      {
        transaction[item] = availability;
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
