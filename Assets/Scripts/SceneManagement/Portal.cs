using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Control;

namespace RPG.SceneManagement
{

  public class Portal : MonoBehaviour
  {

    private enum DestinationEnum
    {
      A,
      B,
      C,
      D,
    }

    // 传送的场景index
    [SerializeField] private int sceneToLoad = -1;

    // 落地位置
    [SerializeField] private Transform spawnPoint = null;

    [SerializeField] private float fadeInTime = 1f;
    [SerializeField] private float fadeOutTime = 0.5f;
    [SerializeField] private float fadeWaitTime = 0.5f;

    [SerializeField] private DestinationEnum destination;

    private void OnTriggerEnter(Collider other)
    {

      if (other.tag == "Player")
      {
        StartCoroutine(Transition());
      }

    }

    private IEnumerator Transition()
    {
      if (sceneToLoad < 0)
      {
        Debug.LogError("Scene not to set.");
        yield return null;
      }

      DontDestroyOnLoad(gameObject);

      Fader fader = FindObjectOfType<Fader>();

      // 场景淡出
      yield return fader.FadeOut(fadeOutTime);

      SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

      // 禁用player
      GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = false;
      savingWrapper.Save();

      // 加载场景
      yield return SceneManager.LoadSceneAsync(sceneToLoad);

      PlayerController newPlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
      newPlayerController.enabled = false;
      savingWrapper.Load();
      Portal otherPortal = GetOtherPortal();

      UpdatePlayer(otherPortal);
      savingWrapper.Save();

      yield return new WaitForSeconds(fadeWaitTime);

      // 场景淡入
      fader.FadeIn(fadeInTime);

      newPlayerController.enabled = true;
      print("Scene Loaded");
      Destroy(gameObject);
    }

    private void UpdatePlayer(Portal otherPortal)
    {
      if (otherPortal)
      {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<NavMeshAgent>().enabled = false;
        player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
        player.transform.rotation = otherPortal.spawnPoint.rotation;
        player.GetComponent<NavMeshAgent>().enabled = true;

      }
    }

    private Portal GetOtherPortal()
    {
      foreach (Portal portal in FindObjectsOfType<Portal>())
      {
        if (portal == this)
        {
          continue;
        }

        if (portal.destination != destination)
        {
          continue;
        }
        return portal;
      }
      return null;
    }
  }
}