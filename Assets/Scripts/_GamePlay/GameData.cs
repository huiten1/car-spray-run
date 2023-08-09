using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentLevel;
    public float roadWidth;
    public float sideMovementSpeed;
    public int playerGold;
    public GameData()
    {
        currentLevel = 1;
        roadWidth = 7;
        sideMovementSpeed = 5;
        playerGold = 0;
    }
}