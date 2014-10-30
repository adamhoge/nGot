using UnityEngine;
using System.Collections;

public class InputControl : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey ("left"))
		{
			Debug.Log("Pressed Left");
		}
		if (Input.GetKey("right"))
		{
			Debug.Log ("Pressed Right");
		}
		if (Input.GetButton ("Jump"))
		{
			Debug.Log("Pressed Jump");
		}
	}
}
