using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

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

      yield return fader.FadeOut(fadeOutTime);

      SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
      
      wrapper.Save();

      yield return SceneManager.LoadSceneAsync(sceneToLoad);

      wrapper.Load();

      Portal otherPortal = GetOtherPortal();

      UpdatePlayer(otherPortal);

      wrapper.Save();

      yield return new WaitForSeconds(fadeWaitTime);
      yield return fader.FadeIn(fadeInTime);

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