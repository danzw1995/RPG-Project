using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.UI
{
  public class PauseMenuUI : MonoBehaviour
  {

    [SerializeField] private Button quit = null;
    [SerializeField] private Button consume = null;
    [SerializeField] private Button save = null;
    [SerializeField] private Button saveAndQuit = null;
    PlayerController playerController;

    private void Awake()
    {
      playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void OnEnable()
    {
      Time.timeScale = 0;
      playerController.enabled = false;
    }

    private void OnDisable()
    {
      ActiveUI();

    }

    private void ActiveUI()
    {
      Time.timeScale = 15;
      playerController.enabled = true;
    }

    public void ClosePauseMenu()
    {
      ActiveUI();
      gameObject.SetActive(false);
    }

    public void Save()
    {
      FindObjectOfType<SavingWrapper>().Save();
    }

    public void SaveAndQuit()
    {
      SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

      savingWrapper.Save();

      savingWrapper.LoadMenu();
    }
  }

}
