using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class SpawnAlloy : MonoBehaviour
{

	//GameObject alloy = null;
	List<GameObject> alloys = new List<GameObject>();
	Vector3 spawnPos;

	// Use this for initialization
	void Start ()
	{
		//alloy = Resources.Load<GameObject> ("Prefabs/Alloys/tShape") as GameObject;
		GameObject[] spawners = GameObject.FindGameObjectsWithTag ("Spawner");
		spawnPos = spawners[0].transform.position;
		InvokeRepeating ("SpawnAlloys", 0.0f, 3.0f);
	}

	void SpawnAlloys ()
	{
		alloys.Add((GameObject) Instantiate (Resources.Load<GameObject> ("Prefabs/Alloys/tShape"), spawnPos, Quaternion.identity));
	}
}
