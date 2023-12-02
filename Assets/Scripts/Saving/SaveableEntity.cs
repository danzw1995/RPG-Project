using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Saving
{
  [ExecuteAlways]
  public class SaveableEntity : MonoBehaviour
  {
    [SerializeField] private string uniqueIdentifier = "";

    private static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();
    public string GetUniqueIdentifier()
    {
      return uniqueIdentifier;
    }

    public object CaptureState()
    {
      Dictionary<string, object> state = new Dictionary<string, object>();

      foreach (ISaveable saveable in GetComponents<ISaveable>())
      {
        state[saveable.GetType().ToString()] = saveable.CaptureState();
      }

      return state;
    }

    public void RestoreState(object state)
    {
      Dictionary<string, object> stateDict = (Dictionary<string, object>)state;

      foreach (ISaveable saveable in GetComponents<ISaveable>())
      {
        string key = saveable.GetType().ToString();
        if (stateDict.ContainsKey(key))
        {
          saveable.RestoreState(stateDict[key]);
        }
      }
    }
#if UNITY_EDITOR
    private void Update()
    {
      if (Application.IsPlaying(gameObject)) return;
      if (string.IsNullOrEmpty(gameObject.scene.path)) return;

      SerializedObject serializedObject = new SerializedObject(this);

      SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

      if (property.stringValue == "" || !IsUnique(property.stringValue))
      {
        property.stringValue = System.Guid.NewGuid().ToString();
        serializedObject.ApplyModifiedProperties();
      }

      globalLookup[property.stringValue] = this;

    }
#endif

    private bool IsUnique(string candiate)
    {
      if (!globalLookup.ContainsKey(candiate)) return true;

      if (globalLookup[candiate] == this) return true;

      if (globalLookup[candiate] == null)
      {
        globalLookup.Remove(candiate);
        return true;
      }

      if (globalLookup[candiate].GetUniqueIdentifier() != candiate)
      {
        globalLookup.Remove(candiate);
      }

      return false;

    }
  }

}

