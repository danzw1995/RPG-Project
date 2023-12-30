﻿using System;
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
    public string GetShopName()
    {
      return shopName;
    }

    [System.Serializable]
    private class StockItemConfig
    {
      public InventoryItem item;
      public int initialStock;
      [Range(0, 100)]
      public float buyingDiscountPercentage;
    }
    public IEnumerable<ShopItem> GetFilteredItems()
    {
      foreach (StockItemConfig config in stockItemConfigs)
      {
        float price = config.item.GetPrice() * (1 - config.buyingDiscountPercentage / 100);
        yield return new ShopItem(config.item, config.initialStock, price, 0);
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

    }

    public float TransactionTotal()
    {
      return 0;
    }

    public void AddToTransaction(InventoryItem item, int quantity)
    {

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
