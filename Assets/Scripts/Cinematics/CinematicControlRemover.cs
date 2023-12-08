using UnityEngine;
using UnityEngine.Playables;

using RPG.Core;
using RPG.Control;


namespace RPG.Cinematics
{
  public class CinematicControlRemover : MonoBehaviour
  {
    private PlayableDirector director;

    private GameObject player;
    private void Start()
    {
      director = GetComponent<PlayableDirector>();
      player = GameObject.FindWithTag("Player");
      
    }

    private void OnEnable() 
    {
      if (director != null)
      {
        director.played += DisableControl;
        director.stopped += EnableControl;
      }
    }

     private void OnDisable() 
    {
      if (director != null)
      {
        director.played -= DisableControl;
        director.stopped -= EnableControl;
      }
    }

    // 播放场景动画时禁用角色移动、控制
    private void DisableControl(PlayableDirector aDirector)
    {
      player.GetComponent<ActionScheduler>().CancelCurrentAction();
      player.GetComponent<PlayerController>().enabled = false;
    }

    // 播放完恢复控制
    private void EnableControl(PlayableDirector aDirector)
    {
      player.GetComponent<PlayerController>().enabled = true;

    }
  }
}
