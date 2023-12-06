using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
  public class Health : MonoBehaviour, ISaveable
  {
    [SerializeField] private int regenHealthPercent = 70;
    private float healthPoints = -1f;
    private bool isDead = false;

    private BaseStats baseStats;

    private void Awake()
    {
      baseStats = GetComponent<BaseStats>();
      baseStats.onLevelUp += regenHealth;
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
      print(gameObject.name +  " take damage " + damage);
      healthPoints = Mathf.Max(healthPoints - damage, 0);
      print(healthPoints);
      if (healthPoints == 0)
      {
        Die();
        AwardExperience(instigator);
      }
    }

    public float GetHealthPoints()
    {
      return healthPoints;
    }

    public float GetMaxHealthPoints()
    {
      return baseStats.GetStat(Stat.Health);
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

    private void regenHealth ()
    {
      float points = baseStats.GetStat(Stat.Health) * regenHealthPercent / 100;
      healthPoints = Mathf.Max(points, healthPoints);
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
