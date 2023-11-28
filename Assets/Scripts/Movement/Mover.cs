using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;

namespace RPG.Movement
{
  public class Mover : MonoBehaviour, IAction, ISaveable
  {
    [SerializeField] private Transform target;
    [SerializeField] private float maxSpeed = 6f;
    private Ray lastRay;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private ActionScheduler actionScheduler;
    private Health health;
    // Start is called before the first frame update

    private void Awake()
    {
      navMeshAgent = GetComponent<NavMeshAgent>();
      animator = GetComponent<Animator>();

      actionScheduler = GetComponent<ActionScheduler>();
      health = GetComponent<Health>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      navMeshAgent.enabled = !health.IsDead();
      UpdateAnimator();

    }

    public void StartMoveAction(Vector3 destination, float speedFraction)
    {
      actionScheduler.StartAction(this);
      MoveTo(destination, speedFraction);
    }


    public void MoveTo(Vector3 destination, float speedFraction)
    {
      navMeshAgent.destination = destination;
      navMeshAgent.speed = maxSpeed * speedFraction;
      navMeshAgent.isStopped = false;

    }

    public void Cancel()
    {
      navMeshAgent.isStopped = true;
    }

    private void UpdateAnimator()
    {
      Vector3 velocity = navMeshAgent.velocity;
      Vector3 localVelocity = transform.InverseTransformDirection(velocity);
      float speed = localVelocity.z;
      animator.SetFloat("forwardSpeed", speed);
    }

    public object CaptureState()
    {
      return new SerializableVector3(transform.position);

    }

    public void RestoreState(object state)
    {
      SerializableVector3 position = (SerializableVector3)state;
      navMeshAgent.enabled = false;
      transform.position = position.ToVector();
      navMeshAgent.enabled = true;

    }
  }

}
