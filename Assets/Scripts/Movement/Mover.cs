using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement {
  public class Mover : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Ray lastRay;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    // Start is called before the first frame update

    private void Awake() {
      navMeshAgent = GetComponent<NavMeshAgent>();
      animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
   
      UpdateAnimator();

    }


    public void MoveTo(Vector3 destination) {
      navMeshAgent.destination = destination;

    }

    private void UpdateAnimator() {
      Vector3 velocity = navMeshAgent.velocity;
      Vector3 localVelocity = transform.InverseTransformDirection(velocity);
      float speed = localVelocity.z;
      animator.SetFloat("forwardSpeed", speed);
    }
}

} 
