using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.UI
{
  public class MainMenuUI : MonoBehaviour
  {
    [SerializeField] private Button continueButton = null;
    [SerializeField] private Button newGameButton = null;
    [SerializeField] private Button loadGameButton = null;
    [SerializeField] private Button quitButton = null;

    [SerializeField] private TMP_InputField newGameNameInput = null;

    private LazyValue<SavingWrapper> savingWrapper;

    private void Awake()
    {
      savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
    }

    public SavingWrapper GetSavingWrapper()
    {
      return FindObjectOfType<SavingWrapper>();
    }

    public void ContinueGame()
    {
      savingWrapper.value.ContinueGame();
    }

    public void NewGame()
    {
      savingWrapper.value.NewGame(newGameNameInput.text);
    }
  }

}
