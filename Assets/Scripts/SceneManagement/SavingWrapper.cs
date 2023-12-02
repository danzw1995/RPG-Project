using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
  public class SavingWrapper : MonoBehaviour
  {

    [SerializeField] private string defaultFile = "save";
    [SerializeField] private float fadeInTime = 0.5f;

    private IEnumerator Start()
    {
      Fader fader = FindObjectOfType<Fader>();
      fader.FadeOutImmediate();
      // 加载最后保存的场景
      yield return GetComponent<SavingSystem>().LoadLastScene(defaultFile);
      yield return fader.FadeIn(fadeInTime);
    }
    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.L))
      {
        Load();
      }

      if (Input.GetKeyDown(KeyCode.S))
      {
        Save();
      }
    }
    public void Load()
    {
      GetComponent<SavingSystem>().Load(defaultFile);

    }

    public void Save()
    {
      GetComponent<SavingSystem>().Save(defaultFile);

    }

  }

}
