using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Shops
{
  public class Shopper : MonoBehaviour
  {

    private Shop activeShop = null;

    public Action OnActiveShopChange = null;
    public void SetActiveShop(Shop shop)
    {
      activeShop = shop;
      if (OnActiveShopChange != null)
      {
        OnActiveShopChange();
      }
    }

    public Shop GetActiveShop()
    {
      return activeShop;
    }
  }

}
