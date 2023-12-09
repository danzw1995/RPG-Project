using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
  public class Health : MonoBehaviour, ISaveable
  {
    [SerializeField] private int regentHealthPercent = 70;
    [SerializeField] private TakeDamageEvent takeDamage = null;
    [SerializeField] private UnityEvent onDie = null;
    private bool isDead = false;

    private BaseStats baseStats;
    private LazyValue<float> healthPoints;

    [System.Serializable]
    private class TakeDamageEvent : UnityEvent<float> {}
  

    private void Awake()
    {
      baseStats = GetComponent<BaseStats>();

      healthPoints = new LazyValue<float>(GetInitHealthPoint);
    }

    private void Start()
    {

      healthPoints.ForceInit();
    }

    private void OnEnable() 
    {
      baseStats.onLevelUp += regentHealth;

    }

    private void OnDisable() 
    {
      baseStats.onLevelUp -= regentHealth;

    }

    private float GetInitHealthPoint()
    {
      return baseStats.GetStat(Stat.Health);
    }

    public bool IsDead()
    {
      return isDead;
    }

    public void TakeDamage(float damage, GameObject instigator)
    {
      print(instigator.name +  " took damage " + damage);
      healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
      takeDamage.Invoke(damage);
      if (healthPoints.value == 0)
      {
        onDie.Invoke();
        Die();
        AwardExperience(instigator);
      }
    }

    public float GetHealthPoints()
    {
      return healthPoints.value;
    }

    public float GetMaxHealthPoints()
    {
      return baseStats.GetStat(Stat.Health);
    }

    public float GetHealthPercent()
    {
      return GetHealthFranction() * 100;
    }

    public float GetHealthFranction()
    {
      return healthPoints.value / baseStats.GetStat(Stat.Health);
    }

    public void Die()
    {
      if (isDead) return;
      isDead = true;
      GetComponent<Animator>().SetTrigger("die");
      GetComponent<ActionScheduler>().CancelCurrentAction();
    }

    private void regentHealth ()
    {
      float points = baseStats.GetStat(Stat.Health) * regentHealthPercent / 100;
      healthPoints.value = Mathf.Max(points, healthPoints.value);
    }

    public void AwardExperience(GameObject instigator)
    {
      Experience experience = instigator.GetComponent<Experience>();
      if (experience == null) return;
      experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
    }

    public object CaptureState()
    {
      return healthPoints.value;
    }

    public void RestoreState(object state)
    {
      healthPoints.value = (float)state;
      if (healthPoints.value == 0)
      {
        Die();
      }
    }
  }
}
