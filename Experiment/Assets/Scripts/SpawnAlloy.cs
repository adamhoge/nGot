using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SpawnAlloy : MonoBehaviour
{
    public List<GameObject> alloys = new List<GameObject>();
    public void Awake()
    {
        alloys = Resources.LoadAll<GameObject>("Alloys").ToList();
    }

	// Use this for initialization
	public void Start ()
    {
	}
	
	// Update is called once per frame
	public void Update ()
    {
	}

    public GameObject NewAlloy()
    {
        return alloys[Random.Range(0, alloys.Count)];
    }
}