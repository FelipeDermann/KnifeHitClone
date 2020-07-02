using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", order = 1)]
public class Level : ScriptableObject
{
    public Difficulty difficultyLevel;
    public bool isBoss;
    [Range(1, 13)]
    public int amountOfKnives;
    public GameObject targetToSpawn;
    public AnimationClip targetRotationAnimation;
}
