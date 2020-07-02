using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    EASY,
    MEDIUM,
    HARD
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Levels")]
    public Level[] levelList;
    public Level[] bossLevels;

    [Header("Current Level")]
    public Level currentLevel;
    public int amountOfKnivesToWin;
    public GameObject currentTarget;

    [Header("Knife")]
    public Knife currentKnife;
    public Transform knifeTargetPoint;
    public Transform knifeOriginPoint;
    public Transform targetOriginPoint;
    public Transform raycastOriginPoint;

    [Tooltip("Time in seconds for knife to reach its destination point")]
    public float knifeTimeToReachTarget;

    [Header("Player Stats")]
    public int score;
    public int apples;
    public int numberOfLevelsCleared;
    public int totalLevelsCleared;
    public Difficulty currentDifficulty;

    bool isPlaying;
    bool onBossLevel;
    public List<Knife> knivesOnScene;
    public List<Level> levelsToChooseFrom;
    Target currentTargetScript;

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

    void Start()
    {
        ChooseLevel();
    }

    public void GetNextKnife()
    {
        if (amountOfKnivesToWin <= 0)
        {
            EndLevel();
            return;
        }

        currentKnife = KnifePool.Instance.SpawnKnife().GetComponent<Knife>();
        knivesOnScene.Add(currentKnife);
    }

    public void GetNextTarget()
    {
        currentTarget = Instantiate(currentLevel.targetToSpawn, targetOriginPoint.position, transform.rotation);
        currentTargetScript = currentTarget.GetComponent<Target>();
        currentTargetScript.StartRotation(currentLevel.targetRotationAnimation);
    }

    void EndLevel()
    {
        isPlaying = false;
        foreach (Knife knifeOnList in knivesOnScene)
        {
            knifeOnList.LevelCleared();
        }

        currentTargetScript.EndLevel();
        numberOfLevelsCleared += 1;
        totalLevelsCleared += 1;

        if (currentLevel.isBoss) ChangeDifficulty();

        Invoke(nameof(ChooseLevel), 1);
    }

    void Update()
    {
        ThrowKnife();
    }

    void ThrowKnife()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0)) return;
        if (!isPlaying) return;

        currentKnife.Throw();
        amountOfKnivesToWin -= 1;
        GameInterface.Instance.UseOneDagger();
    }

    void ChangeDifficulty()
    {
        switch (currentDifficulty)
        {
            case Difficulty.EASY:
                currentDifficulty = Difficulty.MEDIUM;
                break;
            case Difficulty.MEDIUM:
                currentDifficulty = Difficulty.HARD;
                break;
            case Difficulty.HARD:
                currentDifficulty = Difficulty.HARD;
                break;
        }

        numberOfLevelsCleared = 0;
    }

    void ChooseLevel()
    {
        knivesOnScene.Clear();

        if (numberOfLevelsCleared == 3) GetBossLevel();
        else GetLevel();

        amountOfKnivesToWin = currentLevel.amountOfKnives;
        GameInterface.Instance.StartKnivesHUD(amountOfKnivesToWin);

        GetNextKnife();
        GetNextTarget();

        isPlaying = true;
    }

    void GetLevel()
    {
        levelsToChooseFrom.Clear();
        foreach (Level level in levelList)
        {
            if (level.difficultyLevel == currentDifficulty) levelsToChooseFrom.Add(level);
        }

        int randomIndex = Random.Range(0, levelsToChooseFrom.Count);

        currentLevel = levelsToChooseFrom[randomIndex];
    }

    void GetBossLevel()
    {
        levelsToChooseFrom.Clear();
        foreach (Level level in bossLevels)
        {
            if (level.difficultyLevel == currentDifficulty) levelsToChooseFrom.Add(level);
        }

        int randomIndex = Random.Range(0, levelsToChooseFrom.Count);

        currentLevel = levelsToChooseFrom[randomIndex];
    }

    public void IncreaseScore()
    {
        score += 1;
        GameInterface.Instance.UpdateScore();
    }

    public void IncreaseApples()
    {
        apples += 2;
        GameInterface.Instance.UpdateApples();
    }

    public void RestartStats()
    {
        score = 0;
        GameInterface.Instance.UpdateScore();
    }

    public void GameOver()
    {
        isPlaying = false;
    }

    public void Restart()
    {
        foreach (Knife knifeOnList in knivesOnScene)
        {
            knifeOnList.Deactivate();
        }

        Destroy(currentTarget);

        currentDifficulty = Difficulty.EASY;
        numberOfLevelsCleared = 0;
        totalLevelsCleared = 0;

        ChooseLevel();
        RestartStats();

        isPlaying = true;
    }
}
