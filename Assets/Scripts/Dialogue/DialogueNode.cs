

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
  public class DialogueNode : ScriptableObject
  {
    [SerializeField]
    private string text;

    [SerializeField]
    private bool isPlayerSpeaking;

    [SerializeField]
    private List<string> children = new List<string>();
    [SerializeField]
    private Rect rect = new Rect(0, 0, 200, 200);

    [SerializeField]
    private string onEnterAction;

    [SerializeField]
    private string onExitAction;



    public string GetOnEnterAction()
    {
      return onEnterAction;
    }
    public string GetOnExitAction()
    {
      return onExitAction;
    }
    public string GetText()
    {
      return text;
    }

    public Rect GetRect()
    {
      return rect;
    }

    public List<string> GetChildren()
    {
      return children;
    }


    public bool IsPlayerSpeaking()
    {
      return isPlayerSpeaking;
    }

#if UNITY_EDITOR

    public void SetText(string newText)
    {
      if (newText != text)
      {
        Undo.RecordObject(this, "Update Dialogue Node Text");

        text = newText;

        EditorUtility.SetDirty(this);
      }
    }

    public void SetPosition(Vector2 position)
    {
      Undo.RecordObject(this, "Move Dialogue Node");
      rect.position = position;
      EditorUtility.SetDirty(this);

    }

    public void AddChild(string childId)
    {
      Undo.RecordObject(this, "Add Dialogue Node Link");
      children.Add(childId);
      EditorUtility.SetDirty(this);

    }

    public void RemoveChild(string childId)
    {
      Undo.RecordObject(this, "Remove Dialogue Node Link");
      children.Remove(childId);
      EditorUtility.SetDirty(this);

    }

    public void SetIsPlayerSpeaking(bool newIsPlayerSpeaking)
    {
      Undo.RecordObject(this, "Change Dialogue Node Speak");

      isPlayerSpeaking = newIsPlayerSpeaking;
      EditorUtility.SetDirty(this);


    }
#endif
  }
}