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
        List<GameObject> background = Resources.LoadAll<GameObject>("Background").ToList();
        float w = 0.64f;
        float h = 0.64f;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var bgTile = Instantiate(background[0], new Vector3(transform.position.x + i * w, transform.position.y + j * h, 0.1f), Quaternion.identity) as GameObject;
                board[i, j] = null;
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
