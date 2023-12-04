using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;


namespace RPG.Combat
{
  public class Projectile : MonoBehaviour
  {
    // 速度
    [SerializeField] private float speed = 1f;
    // 是否追踪
    [SerializeField] private bool isHoming = true;
    // 击中效果
    [SerializeField] private GameObject hitEffect = null;

    // 子弹最大存活时间
    [SerializeField] private float maxLifeTime = 10f;

    [SerializeField] private GameObject[] DestroyOnHit = null;
    [SerializeField] private float lifeAfterImpact = 2f;

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
        speed = 0;
        if (hitEffect != null)
        {
          Instantiate(hitEffect, GetAimLocation(), transform.rotation);
        }
        foreach (GameObject destroyObject in DestroyOnHit)
        {
          Destroy(destroyObject);
        }
        Destroy(gameObject, lifeAfterImpact);
      }
    }

    public void SetTarget(Health target, float damage)
    {
      this.target = target;
      this.damage = damage;
      transform.LookAt(GetAimLocation());
      Destroy(gameObject, maxLifeTime);
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

