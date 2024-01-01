using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
  public class TraitRowUI : MonoBehaviour
  {
    [SerializeField] private Trait trait;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button plusButton;
    [SerializeField] private TextMeshProUGUI valueText;

    private TraitStore traitStore;


    private void Awake()
    {
      traitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();
    }
    private void Start()
    {
      minusButton.onClick.AddListener(() => Allocate(-1));
      plusButton.onClick.AddListener(() => Allocate(1));


    }

    private void Update()
    {
      minusButton.interactable = traitStore.GetStagedPoints(trait) > 0;
      plusButton.interactable = traitStore.GetUnassignPoints() > 0;

      valueText.text = traitStore.GetProposedPoints(trait).ToString();
    }

    private void Allocate(int value)
    {

      traitStore.AssignPoints(trait, value);

    }
  }
}
