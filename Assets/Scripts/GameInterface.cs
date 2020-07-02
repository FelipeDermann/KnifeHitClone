using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameInterface : MonoBehaviour
{
    public static GameInterface Instance;

    [Header("Knife HUD")]
    public Color iconOn;
    public Color iconOff;
    public RectTransform firstIconPos;
    public Vector3 iconsOffset;
    public List<Image> knifeIcons;
    public List<Image> activeKnifeIcons;

    [Header("HUD")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI applesText;

    Animator anim;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void UpdateScore()
    {
        scoreText.text = GameManager.Instance.score.ToString();
    }

    public void UpdateApples()
    {
        applesText.text = GameManager.Instance.apples.ToString();
    }

    public void GameOver()
    {
        anim.SetTrigger("GameOver");
    }

    public void StartKnivesHUD(int _numberOfKnives)
    {
        activeKnifeIcons.Clear();

        RectTransform previousIconPos = firstIconPos; 

        for (int i = 0; i < knifeIcons.Count; i++)
        {
            if (i <= _numberOfKnives-1)
            {
                knifeIcons[i].rectTransform.position = previousIconPos.position + iconsOffset;
                knifeIcons[i].color = iconOn;
                knifeIcons[i].gameObject.SetActive(true);

                previousIconPos = knifeIcons[i].rectTransform;

                activeKnifeIcons.Add(knifeIcons[i]);
            }
            else
            {
                knifeIcons[i].gameObject.SetActive(false);
            }
        }
    }

    public void UseOneDagger()
    {
        if (activeKnifeIcons.Count > 1)
        {
            activeKnifeIcons[activeKnifeIcons.Count - 1].color = iconOff;
            activeKnifeIcons.RemoveAt(activeKnifeIcons.Count - 1);
        }
        else activeKnifeIcons[0].color = iconOff;
    }

    public void RestartGame()
    {

        anim.SetTrigger("Restart");
        GameManager.Instance.Restart();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
