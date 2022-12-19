using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public GameObject door;
    public GameObject player;
    public GameObject gameManager;
    public static event Action StartGameEvent;
    public static event Action CloseDoorEvent;
    public static event Action EnemyKilledEvent;

    /// <summary>
    /// Déclenche l'évènement de fermeture de porte
    /// </summary>
    public static void StartGame()
    {
        StartGameEvent?.Invoke();
        CloseDoorEvent?.Invoke();
    }

    /// <summary>
    /// Déclenche l'évènement de mort d'un ennemi
    /// </summary>
    public static void EnemyKilled()
    {
        EnemyKilledEvent?.Invoke();
    }
}
