using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RPG.Dialogue
{
  public class PlayerConversant : MonoBehaviour
  {
    [SerializeField]
    private Dialogue testDialogue = null;
    private Dialogue currentDialogue;

    private bool isChoosing = false;
    private DialogueNode currentNode;

    public event Action OnConversationUpdated;
    private IEnumerator Start()
    {
      yield return new WaitForSeconds(2);
      StartDialogue(testDialogue);
    }

    private void StartDialogue(Dialogue dialogue)
    {
      currentDialogue = dialogue;
      currentNode = currentDialogue.GetRootNode();
      OnConversationUpdated();

    }

    public void Quit()
    {
      currentDialogue = null;
      currentNode = null;
      isChoosing = false;
      OnConversationUpdated();
    }

    public bool IsActive()
    {
      return currentDialogue != null;
    }

    public bool IsChoosing()
    {
      return isChoosing;
    }
    public string GetText()
    {
      if (currentDialogue == null)
      {
        return "";
      }
      return currentNode.GetText();
    }

    public IEnumerable<DialogueNode> GetChoices()
    {
      return currentDialogue.GetPlayerChildren(currentNode);
    }

    public void Next()
    {
      int playerResponseNum = currentDialogue.GetPlayerChildren(currentNode).Count();
      if (playerResponseNum > 0)
      {
        isChoosing = true;
        OnConversationUpdated();
        return;
      }

      isChoosing = false;

      DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
      currentNode = children[0];
      OnConversationUpdated();

    }

    public bool HasNext()
    {
      return currentDialogue.GetAllChildren(currentNode).Count() > 0;
    }

    public void SelectChoice(DialogueNode choiceNode)
    {
      currentNode = choiceNode;
      Next();
    }
  }
}

