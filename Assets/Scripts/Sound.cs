using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    //va chercher le slider
    public Slider son;


    //Change le volume de la musique laide du slider
    void Update()
    {
        GetComponent<AudioSource>().volume = son.value;
    }
}
