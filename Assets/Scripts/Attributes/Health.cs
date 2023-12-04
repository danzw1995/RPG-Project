using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
  public class Health : MonoBehaviour, ISaveable
  {
    [SerializeField] private float healthPoints = 100f;
    private bool isDead = false;

    private BaseStats baseStats;

    private void Awake()
    {
      baseStats = GetComponent<BaseStats>();
    }

    private void Start()
    {
      healthPoints = baseStats.GetHealth();
    }

    public bool IsDead()
    {
      return isDead;
    }

    public void TakeDamage(float damage)
    {
      healthPoints = Mathf.Max(healthPoints - damage, 0);
      print(healthPoints);
      if (healthPoints == 0)
      {
        Die();
      }
    }

    public float GetHealthAge()
    {
      return healthPoints / baseStats.GetHealth() * 100;
    }

    public void Die()
    {
      if (isDead) return;
      isDead = true;
      GetComponent<Animator>().SetTrigger("die");
      GetComponent<ActionScheduler>().CancelCurrentAction();
    }

    public object CaptureState()
    {
      return healthPoints;
    }

    public void RestoreState(object state)
    {
      healthPoints = (float)state;
      if (healthPoints == 0)
      {
        Die();
      }
    }
  }
}
