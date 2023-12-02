using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
  public class SavingSystem : MonoBehaviour
  {
    // 加载最后保存的场景
    public IEnumerator LoadLastScene(string saveFile)
    {
      Dictionary<string, object> state = LoadFile(saveFile);
      if (state.ContainsKey("lastSceneBuildIndex"))
      {
        int buildIndex = (int)state["lastSceneBuildIndex"];
        if (buildIndex != SceneManager.GetActiveScene().buildIndex)
        {
          yield return SceneManager.LoadSceneAsync(buildIndex);
        }
      }
      RestoreState(state);
    }
    public void Load(string saveFile)
    {
      RestoreState(LoadFile(saveFile));
    }



    public void Save(string saveFile)
    {
      Dictionary<string, object> state = LoadFile(saveFile);
      CaptureState(state);

      SaveFile(saveFile, state);
    }

    private void SaveFile(string saveFile, object state)
    {
      string path = GetPathFromSaveFile(saveFile);
      print("SaveFile in " + path);

      using (FileStream stream = File.Open(path, FileMode.OpenOrCreate)
)
      {

        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, state);



      }
    }

    private Dictionary<string, object> LoadFile(string saveFile)
    {
      string path = GetPathFromSaveFile(saveFile);
      print("LoadFile in " + path);

      if (File.Exists(path) == false)
      {
        return new Dictionary<string, object>();
      }

      using (FileStream stream = File.Open(path, FileMode.Open))
      {
        BinaryFormatter formatter = new BinaryFormatter();

        return (Dictionary<string, object>)formatter.Deserialize(stream);
      }

    }

    private void CaptureState(Dictionary<string, object> state)
    {

      foreach (var entity in FindObjectsOfType<SaveableEntity>())
      {
        state[entity.GetUniqueIdentifier()] = entity.CaptureState();
      }
      state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
    }
    private void RestoreState(Dictionary<string, object> state)
    {
      foreach (var entity in FindObjectsOfType<SaveableEntity>())
      {
        string key = entity.GetUniqueIdentifier();
        if (state.ContainsKey(key))
        {
          entity.RestoreState(state[key]);

        }

      }

    }

    private string GetPathFromSaveFile(string saveFile)
    {
      return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }
  }
}

