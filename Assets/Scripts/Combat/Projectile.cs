using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;


namespace RPG.Combat
{
  public class Projectile : MonoBehaviour
  {
    [SerializeField] private float speed = 1f;
    [SerializeField] private bool isHoming = true;

    private Health target;
    private float damage = 0f;

    // Update is called once per frame
    private void Update()
    {
      if (target == null) return;

      if (isHoming && !target.IsDead())
      {
        transform.LookAt(GetAimLocation());
      }

      transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.GetComponent<Health>() == target && !target.IsDead())
      {
        target.TakeDamage(this.damage);
        Destroy(gameObject);
      }
    }

    public void SetTarget(Health target, float damage)
    {
      this.target = target;
      this.damage = damage;
      transform.LookAt(GetAimLocation());
    }

    private Vector3 GetAimLocation()
    {
      CapsuleCollider collider = target.GetComponent<CapsuleCollider>();
      if (collider == null)
      {
        return target.transform.position;
      }
      return target.transform.position + Vector3.up * collider.height / 2f;
    }
  }

}

