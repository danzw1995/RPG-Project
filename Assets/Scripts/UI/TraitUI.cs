
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraitUI : MonoBehaviour
{

  private TraitStore traitStore;

  [SerializeField] private TextMeshProUGUI unassignPointsText = null;
  [SerializeField] private Button confirmButton = null;

  private void Awake()
  {
    traitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();
  }
  // Start is called before the first frame update
  void Start()
  {
    confirmButton.onClick.AddListener(traitStore.Commit);
  }

  // Update is called once per frame
  void Update()
  {
    unassignPointsText.text = traitStore.GetUnassignPoints().ToString();
    confirmButton.interactable = !traitStore.IsEmptyStagePoints();
  }
}
