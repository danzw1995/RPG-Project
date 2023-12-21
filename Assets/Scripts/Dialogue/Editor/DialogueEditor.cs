using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditor.Callbacks;
using System.Diagnostics.Tracing;
using System;


namespace RPG.Dialogue.Editor
{
  public class DialogueEditor : EditorWindow
  {

    private Dialogue selectedDialogue;

    private GUIStyle nodeStyle;

    private DialogueNode draggingNode = null;

    private Vector2 draggingOffset;

    [MenuItem("Window/Dialogue Editor")]
    public static void ShowEditorWindow()
    {
      GetWindow(typeof(DialogueEditor), false, "Dialog Editor11");
    }

    [OnOpenAssetAttribute(1)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
      Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
      if (dialogue != null)
      {
        Debug.Log("open dialogue");
        ShowEditorWindow();
        return true;
      }
      return false;
    }

    private void OnEnable()
    {
      Selection.selectionChanged += OnSelectionChange;

      nodeStyle = new GUIStyle();
      nodeStyle.normal.background = EditorGUIUtility.Load("node3") as Texture2D;
      nodeStyle.normal.textColor = Color.white;
      nodeStyle.padding = new RectOffset(20, 20, 20, 20);
      nodeStyle.border = new RectOffset(12, 12, 12, 12);
    }
    private void OnSelectionChange()
    {
      Dialogue dialogue = Selection.activeObject as Dialogue;
      if (dialogue != null)
      {

        selectedDialogue = dialogue;
        Repaint();

      }

    }

    private void OnGUI()
    {
      if (selectedDialogue == null)
      {
        EditorGUILayout.LabelField("No Dialogue Selected");

      }
      else
      {
        ProcessEvents();

        foreach (DialogueNode node in selectedDialogue.GetAllNodes())
        {

          DrawConnections(node);
        }
        foreach (DialogueNode node in selectedDialogue.GetAllNodes())
        {
          DrawNode(node);

        }
      }
    }


    private void ProcessEvents()
    {
      if (Event.current.type == EventType.MouseDown && draggingNode == null)
      {
        draggingNode = GetNodeAtPoint(Event.current.mousePosition);
        if (draggingNode != null)
        {
          draggingOffset = draggingNode.rect.position - Event.current.mousePosition;
        }
      }
      else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
      {
        Undo.RecordObject(selectedDialogue, "Move Dialogue Node");
        draggingNode.rect.position = Event.current.mousePosition + draggingOffset;
        GUI.changed = true;

      }
      else if (Event.current.type == EventType.MouseUp && draggingNode != null)
      {
        draggingNode = null;
      }

    }


    private void DrawNode(DialogueNode node)
    {
      GUILayout.BeginArea(new Rect(node.rect), nodeStyle);
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.LabelField("Node: ", EditorStyles.whiteLabel);
      string newText = EditorGUILayout.TextField(node.text);
      string newUniqueId = EditorGUILayout.TextField(node.uniqueID);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(selectedDialogue, "Update Dialog");
        node.text = newText;
        node.uniqueID = newUniqueId;
      }

      foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
      {
        EditorGUILayout.LabelField(childNode.text);
      }

      GUILayout.EndArea();
    }


    private void DrawConnections(DialogueNode node)
    {
      Vector3 startPosition = new Vector2(node.rect.xMax, node.rect.center.y);
      foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
      {

        Vector3 endPosition = new Vector2(childNode.rect.xMin, childNode.rect.center.y);

        Vector3 controlOffset = endPosition - startPosition;
        controlOffset.y = 0;
        controlOffset.x *= 0.67f;
        Handles.DrawBezier(startPosition, endPosition, startPosition + controlOffset, endPosition - controlOffset, Color.white, null, 5f);
      }
    }

    private DialogueNode GetNodeAtPoint(Vector2 point)
    {

      DialogueNode foundNode = null;
      foreach (DialogueNode node in selectedDialogue.GetAllNodes())
      {
        if (node.rect.Contains(point))
        {
          foundNode = node;
          break;
        }
      }
      return foundNode;
    }

  }
}