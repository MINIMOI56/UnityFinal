using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameScript : MonoBehaviour
{
    private int round;
    private int nbMaxEnemies;
    private int nbEnemiesKilled;
    public GameObject[] enemies;
    public GameObject[] spawnPoints;
    public Transform player;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI nameText;

    void Start()
    {
        EventManager.EnemyKilledEvent += EnemyKilled;
        EventManager.StartGameEvent += StartGame;

        nameText.text = Name.playerName;
    }

    void Update()
    {
        roundText.text = "Round: " + round;
    }

    public void StartGame()
    {
        round = 1;
        roundText.text = "Round: " + round;
        roundText.enabled = true;
        nbMaxEnemies = 5;
        nbEnemiesKilled = 0;
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < nbMaxEnemies; i++)
        {
            int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            int randomEnemy = Random.Range(0, enemies.Length);
            GameObject robot = Instantiate(enemies[randomEnemy], spawnPoints[randomSpawnPoint].transform.position, Quaternion.identity);
            robot.GetComponent<RobotAI>().target = player;
        }
    }

    public void EnemyKilled()
    {
        nbEnemiesKilled++;
        if (nbEnemiesKilled == nbMaxEnemies)
        {
            round++;
            nbMaxEnemies += 5;
            nbEnemiesKilled = 0;
            SpawnEnemies();
        }
    }
}
