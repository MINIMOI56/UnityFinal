using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameScript : MonoBehaviour
{
    private int round;
    private int nbMaxEnemies;
    private int nbEnemiesKilled;

    [Header("Enemies")]
    public GameObject[] enemies;

    [Header("Spawn Points")]
    public GameObject[] spawnPoints;

    [Header("Player")]
    public Transform player;

    [Header("UI")]
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

    /// <summary>
    /// Démarre le jeu
    /// </summary>
    public void StartGame()
    {
        round = 1;
        roundText.text = "Round: " + round;
        roundText.enabled = true;
        nbMaxEnemies = 5;
        nbEnemiesKilled = 0;
        SpawnEnemies();
    }

    /// <summary>
    /// Fait apparaitre les ennemis aléatoirement sur les points de spawn choisis aléatoirement et cible le joueur.
    /// </summary>
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

    /// <summary>
    /// Lorsque tout les ennemis sont tués, on passe à la manche suivante en augmentant 
    /// le nombre d'ennemis à spawn. On réinitialise le nombre d'ennemis tués et on relance la réapparition des ennemis.
    /// </summary>
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
