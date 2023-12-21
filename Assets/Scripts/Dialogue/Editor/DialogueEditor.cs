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

    [NonSerialized]
    private GUIStyle nodeStyle;

    [NonSerialized]
    private DialogueNode draggingNode = null;


    [NonSerialized]
    private DialogueNode creatingNode = null;

    [NonSerialized]
    private DialogueNode deletingNode = null;

    [NonSerialized]
    private DialogueNode linkingParentNode = null;


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

        if (creatingNode != null)
        {
          Undo.RecordObject(selectedDialogue, "Add Node");
          selectedDialogue.CreateNode(creatingNode);
          creatingNode = null;
        }

        if (deletingNode != null)
        {
          Undo.RecordObject(selectedDialogue, "Delete Node");

          selectedDialogue.DeleteNode(deletingNode);
          deletingNode = null;
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
      string newText = EditorGUILayout.TextField(node.text);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(selectedDialogue, "Update Dialog");
        node.text = newText;
      }

      foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
      {
        EditorGUILayout.LabelField(childNode.text);
      }

      GUILayout.BeginHorizontal();

      if (GUILayout.Button("+"))
      {
        creatingNode = node;
      }

      DrawLinkingNode(node);

      if (GUILayout.Button("-"))
      {
        deletingNode = node;
      }
      GUILayout.EndHorizontal();

      GUILayout.EndArea();
    }

    private void DrawLinkingNode(DialogueNode node)
    {
      if (linkingParentNode == null)
      {
        if (GUILayout.Button("link"))
        {
          linkingParentNode = node;
        }
      }
      else if (linkingParentNode.uniqueID == node.uniqueID)
      {
        if (GUILayout.Button("cancel"))
        {
          linkingParentNode = null;
        }
      }
      else if (linkingParentNode.children.Contains(node.uniqueID))
      {
        if (GUILayout.Button("unlink"))
        {
          Undo.RecordObject(selectedDialogue, "UnLink Dialogue node");
          linkingParentNode.children.Remove(node.uniqueID);
          linkingParentNode = null;
        }
      }
      else
      {
        if (GUILayout.Button("child"))
        {
          Undo.RecordObject(selectedDialogue, "Link Dialogue node");
          linkingParentNode.children.Add(node.uniqueID);
          linkingParentNode = null;
        }
      }
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