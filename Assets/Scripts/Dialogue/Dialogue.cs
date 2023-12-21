using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
  [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
  public class Dialogue : ScriptableObject
  {
    [SerializeField] private List<DialogueNode> nodes = null;

    private Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
    private void Awake()
    {
      if (nodes == null)
      {
        nodes = new List<DialogueNode>();
      }

      if (nodes.Count == 0)
      {
        nodes.Add(new DialogueNode());
      }
    }
#endif

    private void OnValidate()
    {
      nodeLookup.Clear();
      foreach (DialogueNode node in GetAllNodes())
      {
        nodeLookup[node.uniqueID] = node;
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

      foreach (string childID in parentNode.children)
      {

        if (nodeLookup.ContainsKey(childID))
        {
          yield return nodeLookup[childID];
        }

      }
    }
  }
}
