using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject trigger;
    
    /// <summary>
    /// Déclenche l'évènement de fermeture de porte
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            trigger.SetActive(false);
            EventManager.StartGame();
        }
    }


}
