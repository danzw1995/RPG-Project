using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RPG.Dialogue
{
  public class PlayerConversant : MonoBehaviour
  {
    [SerializeField] private string playerName = "";
    private Dialogue currentDialogue;
    private AIConversant currentAIConversant;

    private bool isChoosing = false;
    private DialogueNode currentNode;

    public event Action OnConversationUpdated;
    public void StartDialogue(AIConversant aIConversant, Dialogue dialogue)
    {
      currentAIConversant = aIConversant;
      currentDialogue = dialogue;
      currentNode = currentDialogue.GetRootNode();
      TriggerOnEnterAction();
      OnConversationUpdated();

    }

    public void Quit()
    {
      TriggerOnExitAction();
      currentDialogue = null;
      currentAIConversant = null;
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
        TriggerOnExitAction();
        OnConversationUpdated();
        return;
      }

      isChoosing = false;

      DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
      TriggerOnExitAction();
      currentNode = children[0];
      TriggerOnEnterAction();
      OnConversationUpdated();

    }

    public bool HasNext()
    {
      return currentDialogue.GetAllChildren(currentNode).Count() > 0;
    }

    public void SelectChoice(DialogueNode choiceNode)
    {
      currentNode = choiceNode;
      TriggerOnEnterAction();
      Next();
    }

    private void TriggerOnEnterAction()
    {
      if (currentNode != null)
      {
        TriggerAction(currentNode.GetOnEnterAction());
      }
    }

    private void TriggerOnExitAction()
    {
      if (currentNode != null)
      {
        TriggerAction(currentNode.GetOnExitAction());
      }
    }

    private void TriggerAction(string action)
    {
      if (action == "") return;


      foreach (DialogueTrigger dialogueTrigger in currentAIConversant.GetComponents<DialogueTrigger>())
      {
        dialogueTrigger.OnTrigger(action);
      }
    }

    public string GetCurrentConversantName()
    {
      return isChoosing ? playerName : currentAIConversant.GetConversantName();
    }
  }
}

