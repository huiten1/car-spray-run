using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class GameData
{
    public int currentLevel;
    public float roadWidth;
    public float obstacleSpeed;
    public float playerSpeed;
    [FormerlySerializedAs("sensitivity")] public float runnerSensitivity;
    public float sprayGunSpeed;
    public float inkDrainSpeed;
    public float bulletSpeed;
    public int playerGold;
    public Color color;
    public GameData()
    {
        currentLevel = 0;
        roadWidth = 7;
        obstacleSpeed = 0.7f;
        playerSpeed = 4f;
        bulletSpeed = 35f;
        runnerSensitivity = 7;
        playerGold = 0;
        sprayGunSpeed = 0.5f;
        inkDrainSpeed = 0.3f;
        color = new Color(1f, 42f / 255f, 170f / 255f);
    }
}