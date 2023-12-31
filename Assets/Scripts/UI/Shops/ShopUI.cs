using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
  public class ShopUI : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI shopName = null;
    [SerializeField] private Transform contentTransform = null;
    [SerializeField] private GameObject rowPrefab = null;
    [SerializeField] private TextMeshProUGUI totalField = null;
    [SerializeField] private Button confirmButton = null;

    [SerializeField] private Button switchButton = null;

    private Color originalColor;
    private Shopper shopper = null;

    private Shop currentShop = null;

    private void Start()
    {
      originalColor = totalField.color;

      switchButton.onClick.AddListener(SwitchBuyingMode);
      confirmButton.onClick.AddListener(ConfirmTransaction);
      shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
      if (shopper == null) return;
      shopper.OnActiveShopChange += ActiveShopChange;
      ActiveShopChange();

    }
    private void ActiveShopChange()
    {
      if (currentShop != null)
      {
        currentShop.onChange -= UpdateUI;
      }
      currentShop = shopper.GetActiveShop();

      bool flag = currentShop != null;

      gameObject.SetActive(flag);
      if (flag)
      {
        UpdateUI();
        currentShop.onChange += UpdateUI;
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
        shopItemInstance.GetComponent<RowUI>().SetUp(currentShop, shopItem);
      }

      totalField.text = $"${currentShop.TransactionTotal():N2}";

      totalField.color = currentShop.HasSufficientFunds() ? originalColor : Color.red;
      confirmButton.interactable = currentShop.CanTransact();
      TextMeshProUGUI switchText = switchButton.GetComponentInChildren<TextMeshProUGUI>();
      TextMeshProUGUI confirmText = confirmButton.GetComponentInChildren<TextMeshProUGUI>();
      if (currentShop.IsBuyingMode())
      {
        switchText.text = "switch to selling";
        confirmText.text = "buy";
      }
      else
      {
        switchText.text = "switch to buy";
        confirmText.text = "sell";
      }

    }

    public void OnClose()
    {
      shopper.SetActiveShop(null);
    }

    public void ConfirmTransaction()
    {
      currentShop.ConfirmTransaction();
    }

    public void SwitchBuyingMode()
    {
      currentShop.SelectMode(!currentShop.IsBuyingMode());
    }
  }

}
