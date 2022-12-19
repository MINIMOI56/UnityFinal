using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Name : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public static string playerName;

    public void PlayGame()
    {
        playerName = nameText.text;
        SceneManager.LoadScene("Map1");
    }
}
