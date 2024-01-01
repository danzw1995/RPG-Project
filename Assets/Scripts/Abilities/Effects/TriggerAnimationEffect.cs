using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Filters
{
  [CreateAssetMenu(fileName = "Trigger Animation Effect", menuName = "Abilities/Effects/Trigger Animation", order = 0)]
  public class TriggerAnimationEffect : EffectStrategy
  {
    [SerializeField] private string animationTriggerName;

    public override void StartEffect(AbilityData data, Action finished)
    {
      Animator animator = data.GetUser().GetComponent<Animator>();
      animator.SetTrigger(animationTriggerName);

      finished();
    }
  }
}