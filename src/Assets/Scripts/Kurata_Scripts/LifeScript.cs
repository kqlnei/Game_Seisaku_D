using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeScript : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public Image[] healthImages;
    public GameObject gameOverUI;

    public AudioClip gameover;
    AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            ShowGameOverScreen();
        }
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < healthImages.Length; i++)
        {
            if (i < currentHealth)
            {
                healthImages[i].enabled = true; // �̗͂�����ꍇ�AImage��\��
            }
            else
            {
                healthImages[i].enabled = false; // �̗͂��Ȃ��ꍇ�AImage���\��
            }
        }
    }

    void ShowGameOverScreen()
    {
        gameOverUI.SetActive(true); // �Q�[���I�[�o�[UI��\��

        audioSource.PlayOneShot(gameover);

        StartCoroutine(ReloadSceneAfterDelay(3.0f)); // 3�b�҂��Ă���V�[���������[�h
    }

    IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // �V�[�����ēǂݍ���
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}