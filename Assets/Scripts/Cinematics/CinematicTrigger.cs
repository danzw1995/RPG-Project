using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
  public class CinematicTrigger : MonoBehaviour
  {
    private bool isPlayed = false;
    private void OnTriggerEnter(Collider other)
    {
      if (isPlayed == false && other.gameObject.tag == "Player")
      {
        isPlayed = true;
        GetComponent<PlayableDirector>().Play();

      }
    }
  }

}
