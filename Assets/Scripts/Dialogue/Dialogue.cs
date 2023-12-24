using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
  [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
  public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
  {
    [SerializeField] private List<DialogueNode> nodes = new List<DialogueNode>();

    [SerializeField] private Vector2 childNodeOffset = new Vector2(300, 0);

    private Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

    private void OnValidate()
    {
      nodeLookup.Clear();
      foreach (DialogueNode node in GetAllNodes())
      {
        nodeLookup[node.name] = node;
      }
    }

    public IEnumerable<DialogueNode> GetAllNodes()
    {
      return nodes;
    }

    public DialogueNode GetRootNode()
    {
      return nodes[0];
    }

    public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
    {

      foreach (string childID in parentNode.GetChildren())
      {

        if (nodeLookup.ContainsKey(childID))
        {
          yield return nodeLookup[childID];
        }

      }
    }

    public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode parentNode)
    {
      foreach (string childID in parentNode.GetChildren())
      {

        if (nodeLookup.ContainsKey(childID))
        {
          DialogueNode childNode = nodeLookup[childID];
          if (childNode.IsPlayerSpeaking())
          {
            yield return childNode;
          }
        }

      }
    }

    internal IEnumerable<DialogueNode> GetAIChildren(DialogueNode parentNode)
    {
      foreach (string childID in parentNode.GetChildren())
      {

        if (nodeLookup.ContainsKey(childID))
        {
          DialogueNode childNode = nodeLookup[childID];
          if (!childNode.IsPlayerSpeaking())
          {
            yield return childNode;
          }
        }

      }
    }

#if UNITY_EDITOR
    private DialogueNode MakeNode(DialogueNode parent)
    {
      DialogueNode node = CreateInstance<DialogueNode>();
      node.name = Guid.NewGuid().ToString();


      if (parent != null)
      {
        parent.AddChild(node.name);
        node.SetIsPlayerSpeaking(!parent.IsPlayerSpeaking());
        node.SetPosition(parent.GetRect().position + childNodeOffset);
      }

      return node;
    }

    public void AddNode(DialogueNode node)
    {

      nodes.Add(node);

      OnValidate();
    }
    public void CreateNode(DialogueNode parentNode)
    {

      DialogueNode node = MakeNode(parentNode);
      Undo.RegisterCreatedObjectUndo(node, "Create Dialogue Node");
      Undo.RecordObject(this, "Add Dialogue Node");

      AddNode(node);
    }


    public void DeleteNode(DialogueNode deletingNode)
    {
      Undo.RecordObject(this, "Delete Dialogue Node");
      nodes.Remove(deletingNode);
      RemoveDanglingChildren(deletingNode);
      Undo.DestroyObjectImmediate(deletingNode);

      OnValidate();
    }


    private void RemoveDanglingChildren(DialogueNode deletingNode)
    {
      foreach (DialogueNode node in nodes)
      {

        node.RemoveChild(deletingNode.name);

      }
    }

#endif

    public void OnBeforeSerialize()
    {

#if UNITY_EDITOR
      if (nodes.Count == 0)
      {
        DialogueNode node = MakeNode(null);
        AddNode(node);
      }
      if (AssetDatabase.GetAssetPath(this) != "")
      {
        foreach (DialogueNode node in GetAllNodes())
        {
          if (AssetDatabase.GetAssetPath(node) == "")
          {
            AssetDatabase.AddObjectToAsset(node, this);

          }
        }
      }
#endif
    }

    public void OnAfterDeserialize()
    {
    }
  }
}
