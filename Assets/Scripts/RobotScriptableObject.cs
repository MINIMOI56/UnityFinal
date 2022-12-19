using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Robot", menuName = "Robot", order = 0)]

public class RobotScriptableObject : ScriptableObject
{
    [Header("Stats")]
        public float speed;
        public float health;
}
