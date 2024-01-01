using UnityEngine;
using RPG.Attributes;
using UnityEngine.Events;

namespace RPG.Combat
{
  public class Projectile : MonoBehaviour
  {
    [SerializeField] float speed = 1;
    [SerializeField] bool isHoming = true;
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] float maxLifeTime = 10;
    [SerializeField] GameObject[] destroyOnHit = null;
    [SerializeField] float lifeAfterImpact = 2;
    [SerializeField] UnityEvent onHit;

    Health target = null;
    private Vector3 targetPosition;
    GameObject instigator = null;
    float damage = 0;

    private void Start()
    {
      transform.LookAt(GetAimLocation());
    }

    void Update()
    {
      if (target != null && isHoming && !target.IsDead())
      {
        transform.LookAt(GetAimLocation());
      }
      transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetTarget(Health target, GameObject instigator, float damage)
    {
      SetTarget(instigator, damage, target);
    }
    public void SetTarget(Vector3 targetPosition, GameObject instigator, float damage)
    {
      SetTarget(instigator, damage, null, targetPosition);
    }

    public void SetTarget(GameObject instigator, float damage, Health target = null, Vector3 targetPosition = default)
    {
      this.target = target;
      this.targetPosition = targetPosition;
      this.damage = damage;
      this.instigator = instigator;

      Destroy(gameObject, maxLifeTime);
    }

    private Vector3 GetAimLocation()
    {
      if (target == null)
      {
        return targetPosition;
      }

      CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
      if (targetCapsule == null)
      {
        return target.transform.position;
      }
      return target.transform.position + Vector3.up * targetCapsule.height / 2;
    }

    private void OnTriggerEnter(Collider other)
    {
      Health health = other.GetComponent<Health>();
      if (target != null && health != target) return;
      if (health == null || (target != null && target.IsDead())) return;
      if (health.IsDead()) return;
      if (health.gameObject == instigator) return;
      health.TakeDamage(instigator, damage);

      speed = 0;

      onHit.Invoke();

      if (hitEffect != null)
      {
        Instantiate(hitEffect, GetAimLocation(), transform.rotation);
      }

      foreach (GameObject toDestroy in destroyOnHit)
      {
        Destroy(toDestroy);
      }

      Destroy(gameObject, lifeAfterImpact);

    }

  }

}