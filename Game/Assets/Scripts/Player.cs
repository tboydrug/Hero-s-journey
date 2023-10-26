using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public float currentHP;
    public float maxHP;
    public float currentStamina;
    public float maxStamina;
    public float damage;
    public float piercingDamage;
    public float autoAttackSpeed;   
    public float arrowSpeed;
    public float multiShotSpeed;
    public int multiShotCount;

    public Image hpBar;
    public Image staminaBar;
    public Sprite[] bowState;
    public SpriteRenderer spawnPoint;
    public GameObject projectilePrefab;
    private SpriteRenderer bow;

    private Coroutine attackCoroutine;

    private void Start()
    {
        maxHP = currentHP;
        maxStamina = currentStamina;

        bow = transform.GetChild(0).GetComponent<SpriteRenderer>();

        UpdateHPBar();
        UpdateStaminaBar();
        attackCoroutine = StartCoroutine(AutoAttack(autoAttackSpeed,1, arrowSpeed, damage, true));
    }
    public void TakeDamage(float _damage)
    {
        currentHP -= _damage;
        UpdateHPBar();
        StartCoroutine(GameManager.instance.UpdateGame());
        Debug.Log(currentHP);
    }
    private IEnumerator AutoAttack(float _attackSpeed, int _count, float _arrowSpeed, float _damage, bool destroyable)
    {
        _count--;
        UpdateStaminaBar();

        if (_count == 0) 
        {
            _count = 1;
            _attackSpeed = autoAttackSpeed; 
            currentStamina++;

            GameManager.instance.UpdateButtons();
            UpdateStaminaBar();
        }

        GameObject _projectile = Instantiate(projectilePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        Projectile _projectileComponent = _projectile.GetComponent<Projectile>();
        _projectileComponent.damage = _damage;
        _projectileComponent.destroyable = destroyable;

        if (!destroyable) { _projectileComponent.piercingEffect.SetActive(true); }

        bow.sprite = bowState[0];
        spawnPoint.enabled = false;

        yield return new WaitForSeconds(_attackSpeed);

        bow.sprite = bowState[1];
        spawnPoint.enabled = true;

        yield return new WaitForSeconds(0.1f);

        attackCoroutine = StartCoroutine(AutoAttack(_attackSpeed, _count, _arrowSpeed, damage, true));
    }
    public void UseAccurateShot(float staminaCost)
    {
        if (!GameManager.instance.accurateShotCooldown)
        {
            if (currentStamina >= staminaCost)
            {
                currentStamina -= staminaCost;

                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                }

                attackCoroutine = StartCoroutine(AutoAttack(autoAttackSpeed, 1, arrowSpeed, damage * 2, true));
            }
            GameManager.instance.UpdateButtons();
            StartCoroutine(GameManager.instance.AccurateShotCooldown(staminaCost));
        }
    }
    public void UseMultiShot(float staminaCost)
    {
        if (!GameManager.instance.multiShotCooldown)
        {
            if (currentStamina >= staminaCost)
            {
                currentStamina -= staminaCost;
                int count = multiShotCount;

                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                }

                attackCoroutine = StartCoroutine(AutoAttack(multiShotSpeed, count, arrowSpeed, damage, true));
            }
            GameManager.instance.UpdateButtons();
            StartCoroutine(GameManager.instance.MultiShotCooldown(staminaCost));
        }
    }
    public void UsePiercingShot(float staminaCost)
    {
        if (!GameManager.instance.piercingShotCooldown)
        {
            if (currentStamina >= staminaCost)
            {
                currentStamina -= staminaCost;

                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                }

                attackCoroutine = StartCoroutine(AutoAttack(autoAttackSpeed, 1, arrowSpeed*2, piercingDamage, false));
            }
            GameManager.instance.UpdateButtons();
            StartCoroutine(GameManager.instance.PiercingShotCooldown(staminaCost));
        }
    }
    public void UpdateHPBar()
    {
        hpBar.fillAmount = currentHP / maxHP;
    }
    public void UpdateStaminaBar()
    {
        staminaBar.fillAmount = currentStamina / maxStamina;
    }
}
