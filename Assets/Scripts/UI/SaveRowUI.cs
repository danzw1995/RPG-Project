using System.Collections;
using System.Collections.Generic;
using RPG.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{

  public class SaveRowUI : MonoBehaviour
  {
    [SerializeField] TextMeshProUGUI textField = null;

    [SerializeField] Button loadSave = null;

    [SerializeField] Button deleteSave = null;

    private string saveFile;

    public void SetUp(string saveFile)
    {
      this.saveFile = saveFile;
      textField.text = saveFile;
    }

    public void LoadSave()
    {
      FindObjectOfType<SavingWrapper>().LoadGame(saveFile);
    }


  }

}