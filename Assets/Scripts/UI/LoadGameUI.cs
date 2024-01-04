
using System;
using GameDevTV.Utils;
using RPG.SceneManagement;
using UnityEngine;

namespace RPG.UI
{
  public class LoadGameUI : MonoBehaviour
  {
    [SerializeField] private SaveRowUI saveRowPrefab = null;
    [SerializeField] private Transform content = null;


    private void OnEnable()
    {
      foreach (Transform child in content)
      {
        Destroy(child.gameObject);
      }

      LoadSaves();
    }

    private void LoadSaves()
    {
      SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

      foreach (string saveFile in savingWrapper.ListSaves())
      {
        SaveRowUI instance = Instantiate(saveRowPrefab, content);
        instance.SetUp(saveFile);
      }
    }
  }
}
