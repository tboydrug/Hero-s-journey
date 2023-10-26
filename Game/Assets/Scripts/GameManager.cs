using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.SearchService;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    public List<Enemy> enemies;
    public Image[] stars;
    public Sprite filledStar;
    public int filledStars;

    public Transform endGamePanel;
    private Transform victoryRibbon;
    private Transform defeatRibbon;

    public Button accurateShotBtn;  public float accurateCost;      private Image accurateShotIcon;  public bool accurateShotCooldown;
    public Button multiShotBtn;     public float multiShotCost;     private Image multiShotIcon;     public bool multiShotCooldown;
    public Button piercingShotBtn;  public float piercingShotCost;  private Image piercingShotIcon;  public bool piercingShotCooldown;

    public GameObject hitEffect;

    private void Awake()
    {
        instance = this;

        Time.timeScale = 1;

        victoryRibbon = endGamePanel.GetChild(1);
        defeatRibbon = endGamePanel.GetChild(2);

        accurateShotIcon = accurateShotBtn.GetComponent<Image>();
        multiShotIcon = multiShotBtn.GetComponent<Image>();
        piercingShotIcon = piercingShotBtn.GetComponent<Image>();
    }

    public IEnumerator UpdateGame()
    {
        yield return new WaitForSeconds(0.1f);

        enemies.RemoveAll(enemy => enemy.currentHP == 0);

        if (player.currentHP <= 0)
        {
            Time.timeScale = 0;
            endGamePanel.localScale = Vector3.one;
            defeatRibbon.gameObject.SetActive(true);
        }
        if (enemies.Count == 0)
        {
            Time.timeScale = 0;
            endGamePanel.localScale = Vector3.one;
            victoryRibbon.gameObject.SetActive(true);
            UpdateStars();
        }
    }
    public IEnumerator AccurateShotCooldown(float cooldownTime)
    {
        accurateShotCooldown = true;
        float cooldownTimer = 0;

        while (cooldownTimer < cooldownTime)
        {
            float fillAmount = cooldownTimer / cooldownTime;
            accurateShotIcon.fillAmount = fillAmount;

            cooldownTimer += Time.deltaTime;
            yield return null;
        }

        accurateShotCooldown = false;
        accurateShotIcon.fillAmount = 1;
    }
    public IEnumerator MultiShotCooldown(float cooldownTime)
    {
        multiShotCooldown = true;
        float cooldownTimer = 0;

        while (cooldownTimer < cooldownTime)
        {
            float fillAmount = cooldownTimer / cooldownTime;
            multiShotIcon.fillAmount = fillAmount;

            cooldownTimer += Time.deltaTime;
            yield return null;
        }

        multiShotCooldown = false;
        multiShotIcon.fillAmount = 1;
    }
    public IEnumerator PiercingShotCooldown(float cooldownTime)
    {
        piercingShotCooldown = true;
        float cooldownTimer = 0;

        while (cooldownTimer < cooldownTime)
        {
            float fillAmount = cooldownTimer / cooldownTime;
            piercingShotIcon.fillAmount = fillAmount;

            cooldownTimer += Time.deltaTime;
            yield return null;
        }

        piercingShotCooldown = false;
        piercingShotIcon.fillAmount = 1;
    }
    public void UpdateButtons()
    {
        if (player.currentStamina >= accurateCost)
        {
            accurateShotBtn.interactable = true;
        }
        else if (player.currentStamina >= multiShotCost) 
        {
            multiShotBtn.interactable = true;
        }
        else if (player.currentStamina >= piercingShotCost)
        {
            piercingShotBtn.interactable = true;
        }
        else
        {
            accurateShotBtn.interactable = false;
            multiShotBtn.interactable = false;
            piercingShotBtn.interactable = false;
        }
    }
    public void UpdateStars()
    {
        filledStars++;

        for (int i = 0; i < filledStars; i++)
        {
            stars[i].sprite = filledStar;
        }
    }
    public void HitEffect(Transform target)
    {
        GameObject _hitEffect = Instantiate(hitEffect, target.position, Quaternion.identity);
        Destroy(_hitEffect, 1);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
