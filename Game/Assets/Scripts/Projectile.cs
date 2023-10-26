using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float damage;
    public bool destroyable;
    public GameObject piercingEffect;

    private void Start()
    {
        Destroy(gameObject,3);
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                GameManager.instance.HitEffect(enemy.transform);
            }
            if (destroyable)
            {
                Destroy(gameObject);
            }
        }
    }
}
