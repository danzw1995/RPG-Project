using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Shops;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.UI.Shops
{
  public class FilterButtonUI : MonoBehaviour
  {
    [SerializeField] private ItemCategory itemCategory = ItemCategory.None;

    private Button button;

    private Shop currentShop;

    private void Awake()
    {
      button = GetComponent<Button>();

      button.onClick.AddListener(SelectFilter);
    }

    public void SetShop(Shop shop)
    {
      currentShop = shop;
    }

    public void UpdateUI()
    {
      button.interactable = currentShop.GetFilter() != itemCategory;
    }

    private void SelectFilter()
    {
      currentShop.SelectFilter(itemCategory);
    }
  }
}

