using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
  public class DestroyAfterEffect : MonoBehaviour
  {

    [SerializeField] private GameObject target = null;

    // Update is called once per frame
    void Update()
    {
      if (!GetComponent<ParticleSystem>().IsAlive())
      {
        if (target != null)
        {
          Destroy(target);
        }
        else
        {
          Destroy(gameObject);
        }
      }
    }
  }

}
