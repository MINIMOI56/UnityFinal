using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;
    void Start(){
        EventManager.CloseDoorEvent += CloseDoor;
    }

    /// <summary>
    /// Ferme la porte avec l'évènement
    /// </summary>
    private void CloseDoor(){
        animator.SetTrigger("Start");
        Debug.Log("Door is closing");
    }
}
