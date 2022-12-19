using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public GameObject door;
    public GameObject player;
    public GameObject gameManager;
    //public static event Action StartGameEvent;
    public static event Action CloseDoorEvent;

    public static void StartCloseDoor()
    {
        Debug.Log("StartCloseDoor");
        CloseDoorEvent?.Invoke();
    }
}
