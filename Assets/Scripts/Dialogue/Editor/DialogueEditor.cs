using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditor.Callbacks;
using System.Diagnostics.Tracing;
using System;
using UnityEngine.UI;


namespace RPG.Dialogue.Editor
{
  public class DialogueEditor : EditorWindow
  {

    private Dialogue selectedDialogue;

    [NonSerialized]
    private GUIStyle nodeStyle;

    [NonSerialized]
    private GUIStyle playerNodeStyle;

    [NonSerialized]
    private DialogueNode draggingNode = null;


    [NonSerialized]
    private DialogueNode creatingNode = null;

    [NonSerialized]
    private DialogueNode deletingNode = null;

    [NonSerialized]
    private DialogueNode linkingParentNode = null;

    [NonSerialized]
    private bool draggingCanvas = false;

    private Vector2 draggingCanvasOffset;

    private Vector2 scrollPosition;

    private const int canvasSise = 4000;

    private const int backgroundSize = 40;


    private Vector2 draggingOffset;

    [MenuItem("Window/Dialogue Editor")]
    public static void ShowEditorWindow()
    {
      GetWindow(typeof(DialogueEditor), false, "Dialog Editor");
    }

    [OnOpenAsset(1)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
      Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
      if (dialogue != null)
      {
        ShowEditorWindow();
        return true;
      }
      return false;
    }

    private void OnEnable()
    {
      Selection.selectionChanged += OnSelectionChange;

      nodeStyle = new GUIStyle();
      nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
      nodeStyle.normal.textColor = Color.white;
      nodeStyle.padding = new RectOffset(20, 20, 20, 20);
      nodeStyle.border = new RectOffset(12, 12, 12, 12);

      playerNodeStyle = new GUIStyle();
      playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
      playerNodeStyle.normal.textColor = Color.white;
      playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
      playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
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

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        Rect canvas = GUILayoutUtility.GetRect(canvasSise, canvasSise);

        Texture2D backgroundResource = Resources.Load("background") as Texture2D;
        Rect textureCoords = new Rect(0, 0, canvasSise / backgroundSize, canvasSise / backgroundSize);
        GUI.DrawTextureWithTexCoords(canvas, backgroundResource, textureCoords);

        foreach (DialogueNode node in selectedDialogue.GetAllNodes())
        {

          DrawConnections(node);
        }
        foreach (DialogueNode node in selectedDialogue.GetAllNodes())
        {
          DrawNode(node);

        }

        GUILayout.EndScrollView();

        if (creatingNode != null)
        {
          selectedDialogue.CreateNode(creatingNode);
          creatingNode = null;
        }

        if (deletingNode != null)
        {

          selectedDialogue.DeleteNode(deletingNode);
          deletingNode = null;
        }
      }
    }


    private void ProcessEvents()
    {
      if (Event.current.type == EventType.MouseDown && draggingNode == null)
      {
        draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
        if (draggingNode != null)
        {
          draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
          Selection.activeObject = draggingNode;
        }
        else
        {
          draggingCanvas = true;
          draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
          Selection.activeObject = selectedDialogue;
        }
      }
      else if (Event.current.type == EventType.MouseDrag)
      {
        if (draggingNode != null)
        {
          draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);
          GUI.changed = true;
        }
        else if (draggingCanvas)
        {
          scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
          GUI.changed = true;
        }
      }
      else if (Event.current.type == EventType.MouseUp)
      {
        if (draggingNode != null)
        {
          draggingNode = null;

        }
        else if (draggingCanvas)
        {
          draggingCanvas = false;
        }
      }

    }


    private void DrawNode(DialogueNode node)
    {

      GUIStyle style = nodeStyle;
      if (node.IsPlayerSpeaking())
      {
        style = playerNodeStyle;
      }

      GUILayout.BeginArea(new Rect(node.GetRect()), style);
      node.SetText(EditorGUILayout.TextField(node.GetText()));

      foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
      {
        EditorGUILayout.LabelField(childNode.GetText());
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
      else if (linkingParentNode == node)
      {
        if (GUILayout.Button("cancel"))
        {
          linkingParentNode = null;
        }
      }
      else if (linkingParentNode.GetChildren().Contains(node.name))
      {
        if (GUILayout.Button("unlink"))
        {
          linkingParentNode.RemoveChild(node.name);
          linkingParentNode = null;
        }
      }
      else
      {
        if (GUILayout.Button("child"))
        {
          linkingParentNode.AddChild(node.name);
          linkingParentNode = null;
        }
      }
    }


    private void DrawConnections(DialogueNode node)
    {
      Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
      foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
      {

        Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);

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
        if (node.GetRect().Contains(point))
        {
          foundNode = node;
          break;
        }
      }
      return foundNode;
    }

  }
}