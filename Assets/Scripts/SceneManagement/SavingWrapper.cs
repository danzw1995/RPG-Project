
using UnityEngine;

using RPG.Saving;

namespace RPG.SceneManagement
{
  public class SavingWrapper : MonoBehaviour
  {
    private const string defaultSaveFile = "save";
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
      GetComponent<SavingSystem>().Load(defaultSaveFile);
    }

    public void Save()
    {
      GetComponent<SavingSystem>().Save(defaultSaveFile);

    }
  }
}
