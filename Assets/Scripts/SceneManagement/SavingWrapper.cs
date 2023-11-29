using System.Collections;

using UnityEngine;

using RPG.Saving;

namespace RPG.SceneManagement
{
  public class SavingWrapper : MonoBehaviour
  {
    private const string defaultSaveFile = "save";
    [SerializeField] private float fadeInTime = 0.5f;

    private SavingSystem savingSystem;
    private IEnumerator Start() {
      savingSystem = GetComponent<SavingSystem>();
      Fader fader =  FindObjectOfType<Fader>();
      fader.FadeOutImmediate();
      yield return savingSystem.LoadLastScene(defaultSaveFile);
      yield return fader.FadeIn(fadeInTime);

    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.L))
      {
        Load();
      }
      else if (Input.GetKeyDown(KeyCode.S))
      {
        Save();
      }
    }

    public void Load()
    {
      savingSystem.Load(defaultSaveFile);
    }

    public void Save()
    {
      savingSystem.Save(defaultSaveFile);

    }
  }
}
