using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float currentHP;
    public float maxHP;
    public float moveSpeed;
    public float damage;

    public Image hpBar;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        maxHP = currentHP;
        damage = currentHP;
        UpdateHPBar();
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(float _damage)
    {
        currentHP -= _damage;
        damage = currentHP;
        anim.SetTrigger("col");
        UpdateHPBar();

        if (currentHP < 0.1f)
        {
            Destroy(gameObject);
            StartCoroutine(GameManager.instance.UpdateGame());
            GameManager.instance.UpdateStars();
        }
    }

    public void UpdateHPBar()
    {
        hpBar.fillAmount = currentHP/maxHP;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
            StartCoroutine(GameManager.instance.UpdateGame());
        }
    }
}
