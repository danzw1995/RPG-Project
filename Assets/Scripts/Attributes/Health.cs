using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
  public class Health : MonoBehaviour, ISaveable
  {
    private float healthPoints = -1f;
    private bool isDead = false;

    private BaseStats baseStats;

    private void Awake()
    {
      baseStats = GetComponent<BaseStats>();
    }

    private void Start()
    {
      if (healthPoints < 0)
      {
        healthPoints = baseStats.GetStat(Stat.Health);

      }
    }

    public bool IsDead()
    {
      return isDead;
    }

    public void TakeDamage(float damage, GameObject instigator)
    {
      healthPoints = Mathf.Max(healthPoints - damage, 0);
      print(healthPoints);
      if (healthPoints == 0)
      {
        Die();
        AwardExperience(instigator);
      }
    }

    public float GetHealthAge()
    {
      return healthPoints / baseStats.GetStat(Stat.Health) * 100;
    }

    public void Die()
    {
      if (isDead) return;
      isDead = true;
      GetComponent<Animator>().SetTrigger("die");
      GetComponent<ActionScheduler>().CancelCurrentAction();
    }

    public void AwardExperience(GameObject instigator)
    {
      Experience experience = instigator.GetComponent<Experience>();
      if (experience == null) return;
      experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
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
