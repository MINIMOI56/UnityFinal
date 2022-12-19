using UnityEngine;
using System.Collections;

public class LineFade : MonoBehaviour
{
	public Color color;
	public float speed = 10f;
	LineRenderer lr;

	void Start ()
	{
		lr = GetComponent<LineRenderer> ();
	}

	void Update ()
	{
		color.a = Mathf.Lerp (color.a, 0, Time.deltaTime * speed);

		lr.startColor = color;
		lr.endColor = color;
	}
}

