using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using RPG.Control;
using RPG.Inventories;
using RPG.Stats;
using UnityEngine;


namespace RPG.Shops
{
  public class Shop : MonoBehaviour, IRaycastable, ISaveable
  {
    [SerializeField] private string shopName = null;

    [SerializeField] private StockItemConfig[] stockItemConfigs;

    [SerializeField] private float sellPercentage = 80f;

    [SerializeField] private float maxBarterDiscountPercentage = 10f;
    public Action onChange;

    [Serializable]
    private class StockItemConfig
    {
      public InventoryItem item;
      public int initialStock;
      [Range(0, 100)]
      public float buyingDiscountPercentage;

      public int levelToUnlock = 0;
    }

    private Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
    private Dictionary<InventoryItem, int> stockSold = new Dictionary<InventoryItem, int>();

    private Shopper currentShopper = null;

    private bool isBuyingMode = true;

    private ItemCategory filter = ItemCategory.None;

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
      foreach (ShopItem shopItem in GetAllItems())
      {
        if (filter == ItemCategory.None || filter == shopItem.GetItem().GetItemCategory())
        {
          yield return shopItem;
        }
      }

    }

    private IEnumerable<ShopItem> GetAllItems()
    {

      Dictionary<InventoryItem, float> prices = GetPrices();
      Dictionary<InventoryItem, int> availabilities = GetAvailabilities();
      foreach (InventoryItem item in availabilities.Keys)
      {
        if (availabilities[item] <= 0) continue;
        float price = prices[item];
        int quantityTransaction;
        transaction.TryGetValue(item, out quantityTransaction);

        int availability = availabilities[item];
        yield return new ShopItem(item, availability, price, quantityTransaction);
      }
    }

    private Dictionary<InventoryItem, int> GetAvailabilities()
    {
      Dictionary<InventoryItem, int> availabilities = new Dictionary<InventoryItem, int>();
      foreach (StockItemConfig config in GetAvailabilityConfigs())
      {
        if (isBuyingMode)
        {
          if (!availabilities.ContainsKey(config.item))
          {
            int sold = 0;
            stockSold.TryGetValue(config.item, out sold);
            availabilities[config.item] = -sold;
          }
          availabilities[config.item] += config.initialStock;
        }
        else
        {
          availabilities[config.item] = CountItemsInInventory(config.item);
        }

      }
      return availabilities;
    }

    private Dictionary<InventoryItem, float> GetPrices()
    {
      Dictionary<InventoryItem, float> prices = new Dictionary<InventoryItem, float>();
      foreach (StockItemConfig config in GetAvailabilityConfigs())
      {
        if (isBuyingMode)
        {
          if (!prices.ContainsKey(config.item))
          {
            prices[config.item] = config.item.GetPrice() * GetBarterDiscount();

          }
          prices[config.item] *= 1 - config.buyingDiscountPercentage / 100;
        }
        else
        {
          prices[config.item] = config.item.GetPrice() * sellPercentage / 100;
        }

      }
      return prices;
    }

    private float GetBarterDiscount()
    {
      float discount = currentShopper.GetComponent<BaseStats>().GetStat(Stat.buyingDiscountPercentage);

      return 1 - Math.Min(discount, maxBarterDiscountPercentage) / 100;
    }

    private IEnumerable<StockItemConfig> GetAvailabilityConfigs()
    {
      int currentShopperLevel = GetCurrentShopperLevel();
      foreach (StockItemConfig config in stockItemConfigs)
      {
        if (config.levelToUnlock > currentShopperLevel)
        {
          continue;
        }
        yield return config;
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

    public void SelectFilter(ItemCategory category)
    {
      filter = category;
      if (onChange != null)
      {
        onChange();
      }
    }

    public ItemCategory GetFilter()
    {
      return filter;
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
      if (!stockSold.ContainsKey(item))
      {
        stockSold[item] = 0;
      }
      stockSold[item]--;
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
        if (!stockSold.ContainsKey(item))
        {
          stockSold[item] = 0;
        }
        stockSold[item]++;
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

      var availabilities = GetAvailabilities();
      int availability = availabilities[item];
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

    private int GetCurrentShopperLevel()
    {
      BaseStats stats = currentShopper.GetComponent<BaseStats>();
      if (stats == null) return 0;
      return stats.GetLevel();
    }

    public object CaptureState()
    {
      Dictionary<string, int> stateObject = new Dictionary<string, int>();

      foreach (var pair in stockSold)
      {
        stateObject[pair.Key.GetItemID()] = pair.Value;
      }
      return stateObject;
    }

    public void RestoreState(object state)
    {
      Dictionary<string, int> stateObject = (Dictionary<string, int>)state;
      stockSold.Clear();
      foreach (var pair in stateObject)
      {
        stockSold[InventoryItem.GetFromID(pair.Key)] = pair.Value;
      }
    }
  }

}
