using GameDevTV.Saving;
using GameDevTV.Utils;
using RPG.Stats;
using UnityEngine;


namespace RPG.Attributes
{
  public class Mana : MonoBehaviour, ISaveable
  {
    private LazyValue<float> mana;

    private BaseStats baseStats;

    private void Awake()
    {
      baseStats = GetComponent<BaseStats>();
      mana = new LazyValue<float>(GetMaxMana);
    }

    private void Update()
    {
      if (mana.value < GetMaxMana())
      {
        mana.value += GetMaxManaRegenerate() * Time.deltaTime;
        if (mana.value > GetMaxMana())
        {
          mana.value = GetMaxMana();
        }
      }
    }

    public float GetMana()
    {
      return mana.value;
    }

    public float GetMaxMana()
    {
      return baseStats.GetStat(Stat.Mana);
    }

    public float GetMaxManaRegenerate()
    {
      return baseStats.GetStat(Stat.ManaRegenerate);

    }

    public bool UseMana(float manaToUse)
    {
      if (mana.value < manaToUse)
      {
        return false;
      }
      mana.value -= manaToUse;
      return true;
    }

    public object CaptureState()
    {
      return mana.value;
    }

    public void RestoreState(object state)
    {
      mana.value = (float)state;
    }
  }
}