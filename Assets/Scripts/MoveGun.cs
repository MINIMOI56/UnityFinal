using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGun : MonoBehaviour
{
    public float amount = 0.05f;
    public float maxAmount = 0.15f;
    public float smoothAmount = 5f;

    Vector3 defPos;

    void Start()
    {
        defPos = transform.localPosition;
    }

    void Update()
    {
        // Fait bouger l'arme en fonction de ou la souris va
        float movementX = -Input.GetAxis("Mouse X") * amount;
        float movementY = -Input.GetAxis("Mouse Y") * amount;

        movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
        movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + defPos, Time.deltaTime * smoothAmount);

        // Animation de l'arme pour quel ne soit pas statique
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Sin(Time.time) * 0.01f + defPos.y, transform.localPosition.z);
        
    }
}
