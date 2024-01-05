using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using RPG.Attributes;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.AI;


namespace RPG.Control
{
  public class ReSpawner : MonoBehaviour
  {
    [SerializeField] private Transform reSpawnLocation = null;
    [SerializeField] private float delay = 3f;

    [SerializeField] private float fadeTime = 0.2f;

    [SerializeField] private float recoveryHealthPercentage = 40f;
    [SerializeField] private float enemyRecoveryHealthPercentage = 40f;

    private Health health;
    private void Awake()
    {
      health = GetComponent<Health>();
      health.onDie.AddListener(ReSpawn);
    }
    private void Start()
    {
      if (health.IsDead())
      {
        ReSpawn();
      }
    }

    private void ReSpawn()
    {
      StartCoroutine(ReSpawnCoroutine());
    }

    private IEnumerator ReSpawnCoroutine()
    {
      SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
      savingWrapper.Save();
      yield return new WaitForSeconds(delay);
      Fader fader = FindObjectOfType<Fader>();
      yield return fader.FadeOut(fadeTime);
      ReSpawnPlayer();
      ReSpawnEnemy();
      savingWrapper.Save();

      yield return fader.FadeIn(fadeTime);
    }

    private void ReSpawnPlayer()
    {
      Vector3 positionDelta = reSpawnLocation.position - transform.position;

      GetComponent<NavMeshAgent>().Warp(reSpawnLocation.position);
      health.Heal(health.GetMaxHealthPoints() * recoveryHealthPercentage / 100);

      ICinemachineCamera cinemachineCamera = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
      if (cinemachineCamera.Follow == transform)
      {
        cinemachineCamera.OnTargetObjectWarped(transform, positionDelta);
      }
    }

    private void ReSpawnEnemy()
    {
      foreach (AIController aIController in FindObjectsOfType<AIController>())
      {

        Health enemyHeath = aIController.GetComponent<Health>();
        if (enemyHeath != null && !enemyHeath.IsDead())
        {
          aIController.Reset();

          enemyHeath.Heal(enemyHeath.GetMaxHealthPoints() * enemyRecoveryHealthPercentage / 100);
        }
      }
    }
  }

}
