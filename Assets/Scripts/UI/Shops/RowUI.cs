using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.UI.Shops
{
  public class RowUI : MonoBehaviour
  {
    [SerializeField] private Image image = null;
    [SerializeField] private TextMeshProUGUI item = null;
    [SerializeField] private TextMeshProUGUI availability = null;
    [SerializeField] private TextMeshProUGUI price = null;
    [SerializeField] private TextMeshProUGUI quantity = null;
    public void SetUp(ShopItem shopItem)
    {
      image.sprite = shopItem.GetIcon();
      item.text = shopItem.GetName();
      availability.text = $"{shopItem.GetAvailability()}";
      price.text = $"${shopItem.GetPrice():N2}";
      quantity.text = $"{shopItem.GetQuantityInTransaction()}";
    }


  }

}
