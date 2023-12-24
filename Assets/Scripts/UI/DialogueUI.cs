using System.Collections;
using System.Collections.Generic;
using RPG.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
  public class DialogueUI : MonoBehaviour
  {
    private PlayerConversant playerConversant;

    [SerializeField] private TextMeshProUGUI speaker = null;

    [SerializeField] private TextMeshProUGUI aiText = null;

    [SerializeField] private Button nextButton = null;

    [SerializeField] private Button quitButton = null;

    [SerializeField] private Transform choicesRoot = null;

    [SerializeField] private GameObject choicePrefab = null;

    [SerializeField] private GameObject aiResponse = null;

    private void Awake()
    {
      playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
    }

    private void Start()
    {
      nextButton.onClick.AddListener(() => playerConversant.Next());
      quitButton.onClick.AddListener(() => playerConversant.Quit());
      playerConversant.OnConversationUpdated += UpdateUI;
      UpdateUI();
    }

    private void UpdateUI()
    {
      bool isActive = playerConversant.IsActive();
      gameObject.SetActive(isActive);
      if (!isActive)
      {
        return;
      }

      bool isChoosing = playerConversant.IsChoosing();
      aiResponse.SetActive(!isChoosing);
      choicesRoot.gameObject.SetActive(isChoosing);

      speaker.text = playerConversant.GetCurrentConversantName();

      if (isChoosing)
      {
        BuildChoiceList();
      }
      else
      {

        aiText.text = playerConversant.GetText();
        nextButton.gameObject.SetActive(playerConversant.HasNext());
      }



    }

    private void BuildChoiceList()
    {
      foreach (Transform item in choicesRoot)
      {
        Destroy(item.gameObject);
      }

      foreach (DialogueNode choiceNode in playerConversant.GetChoices())
      {
        GameObject choiceInstance = Instantiate(choicePrefab, choicesRoot);
        TextMeshProUGUI textComponent = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = choiceNode.GetText();

        Button button = choiceInstance.GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
          playerConversant.SelectChoice(choiceNode);
        });
      }
    }
  }

}
