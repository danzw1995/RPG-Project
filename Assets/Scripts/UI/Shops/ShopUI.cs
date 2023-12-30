using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Shops;
using TMPro;
using UnityEngine;

namespace RPG.UI.Shops
{
  public class ShopUI : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI shopName = null;
    [SerializeField] private Transform contentTransform = null;
    [SerializeField] private GameObject rowPrefab = null;
    private Shopper shopper = null;

    private Shop currentShop = null;

    private void Start()
    {
      shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
      if (shopper == null) return;
      shopper.OnActiveShopChange += ActiveShopChange;
      ActiveShopChange();

    }
    private void ActiveShopChange()
    {
      currentShop = shopper.GetActiveShop();

      bool flag = currentShop != null;

      gameObject.SetActive(flag);
      if (flag)
      {
        UpdateUI();
      }
    }

    private void UpdateUI()
    {
      shopName.text = currentShop.GetShopName();
      foreach (Transform child in contentTransform)
      {
        Destroy(child.gameObject);
      }

      foreach (ShopItem shopItem in currentShop.GetFilteredItems())
      {
        GameObject shopItemInstance = Instantiate(rowPrefab, contentTransform);
        shopItemInstance.GetComponent<RowUI>().SetUp(shopItem);
      }

    }

    public void OnClose()
    {
      shopper.SetActiveShop(null);
    }
  }

}
