using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public Transform objetASuivre;

    void Update()
    {
        transform.position = objetASuivre.position;
    }
}
