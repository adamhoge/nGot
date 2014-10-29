using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CreateGameBoard : MonoBehaviour {

    public int width;
    public int height;

    public GameObject[,] board;

    void Awake()
    {
        board = new GameObject[width, height];
    }

	// Use this for initialization
	void Start ()
    {
        List<GameObject> ingots = Resources.LoadAll<GameObject>("Ingots").ToList();
        //var collider = ingots[0].GetComponent<BoxCollider2D>();
        //float w = collider.size.x;
        //float h = collider.size.y;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //board[i, j] = Instantiate(ingots[Random.Range(0, ingots.Count)], new Vector3(transform.position.x + i*w, transform.position.y + j*h, transform.position.z), Quaternion.identity) as GameObject;
                board[i, j] = null;
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
