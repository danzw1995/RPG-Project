using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
  public class SavingWrapper : MonoBehaviour
  {
    const string currentSaveKey = "currentSaveFile";

    [SerializeField] float fadeInTime = 0.2f;
    [SerializeField] float fadeOutTime = 0.2f;

    [SerializeField] int firstBuildScene = 1;
    [SerializeField] int menuScene = 0;

    public void ContinueGame()
    {
      if (!PlayerPrefs.HasKey(currentSaveKey)) return;
      if (!GetComponent<SavingSystem>().SaveFileExist(GetCurrentSave())) return;
      StartCoroutine(LoadLastScene());
    }

    public void NewGame(string saveFile)
    {
      if (String.IsNullOrEmpty(saveFile)) return;
      SetCurrentSave(saveFile);
      StartCoroutine(LoadFirstScene());
    }

    public void LoadGame(string saveFile)
    {
      SetCurrentSave(saveFile);
      StartCoroutine(LoadLastScene());
    }

    public void LoadMenu()
    {
      StartCoroutine(LoadMenuScene());
    }
    private void SetCurrentSave(string saveFile)
    {
      PlayerPrefs.SetString(currentSaveKey, saveFile);
    }

    private string GetCurrentSave()
    {
      return PlayerPrefs.GetString(currentSaveKey);
    }

    private IEnumerator LoadMenuScene()
    {
      Fader fader = FindObjectOfType<Fader>();
      fader.FadeOut(fadeOutTime);
      yield return SceneManager.LoadSceneAsync(menuScene);

      yield return fader.FadeIn(fadeInTime);
    }
    private IEnumerator LoadFirstScene()
    {
      Fader fader = FindObjectOfType<Fader>();
      fader.FadeOut(fadeOutTime);
      yield return SceneManager.LoadSceneAsync(firstBuildScene);

      yield return fader.FadeIn(fadeInTime);
    }

    private IEnumerator LoadLastScene()
    {
      Fader fader = FindObjectOfType<Fader>();
      fader.FadeOut(fadeOutTime);
      yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());

      yield return fader.FadeIn(fadeInTime);
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.S))
      {
        Save();
      }
      if (Input.GetKeyDown(KeyCode.L))
      {
        Load();
      }
      if (Input.GetKeyDown(KeyCode.Delete))
      {
        Delete();
      }
    }

    public void Load()
    {
      GetComponent<SavingSystem>().Load(GetCurrentSave());
    }

    public void Save()
    {
      GetComponent<SavingSystem>().Save(GetCurrentSave());
    }

    public void Delete()
    {
      GetComponent<SavingSystem>().Delete(GetCurrentSave());
    }

    public IEnumerable<string> ListSaves()
    {
      return GetComponent<SavingSystem>().ListSaves();
    }
  }
}