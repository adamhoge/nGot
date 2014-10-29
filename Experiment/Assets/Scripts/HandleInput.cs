using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class HandleInput : MonoBehaviour 
{
    GameObject currentAlloy = null;
    SpawnAlloy spawner = null;
    Vector3 pos;
    CreateGameBoard gameBoard = null;

    private float dt;

    public float timeIncrements;
    public float inputRate;

    void Awake()
    {
        spawner = this.GetComponentInChildren<SpawnAlloy>();
        gameBoard = GetComponent<CreateGameBoard>();

        foreach (Transform item in transform)
        {
            if (item.name == "AlloySpawner")
            {
                pos = item.position;
            }
        }
    }

	// Use this for initialization
	void Start ()
    {
        dt = Time.time;
        MakeNewAlloy();
        //InvokeRepeating("DropAlloy", 0.5f, timeIncrements);
	}

    bool print = false;
	// Update is called once per frame
	void Update ()
    {
        Vector3 offset = Vector3.zero;
        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");

        if (dx > 0.0f)
        {
            dx = 0.64f;
        }
        else if (dx < 0.0f)
        {
            dx = -0.64f;
        }

        if (dy >= 0.0f)
        {
            dy = 0.0f;
        }
        else
        {
            dy = -0.64f;
        }

        offset.x += dx;
        offset.y += dy;
        if (Time.time - dt >= inputRate)
        {
            dt = Time.time;
            MoveAlloy(offset);
        }
	}

    public void DropAlloy()
    {
        MoveAlloy(new Vector3(0.0f, -0.64f));
    }

    private void MoveAlloy(Vector3 offset)
    {
        var ingots = currentAlloy.GetComponentsInChildren<Transform>().Where(t => t.tag == "Ingot").ToArray();

        int[] x = new int[ingots.Count()];
        int[] y = new int[ingots.Count()];
          
        bool lastline = false;
        bool willCollide = false;

        for (int i = 0; i < ingots.Count(); i++)
        {
            Vector3 newPos = ingots[i].transform.position + offset;
            x[i] = Convert.ToInt32(newPos.x / 0.64f);
            y[i] = Convert.ToInt32(newPos.y / 0.64f);

            // out of bounds, don't update
            if (x[i] < 0 || x[i] >= gameBoard.width)
            {
                return;
            }

            // below the board, need new piece
            if (y[i] < 0)
            {
                lastline = true;
            }
        }
        
        for (int i = 0; i < ingots.Count(); i++)
        {
            // this piece is not on the board, don't check it
            if (y[i] >= gameBoard.height || y[i] < 0)
            {
                continue;
            }
            // otherwise check for a piece where we want to move
            if (gameBoard.board[x[i], y[i]] != null)
            {
                willCollide = true;
            }
        }

        Debug.Log(MakeGameBoard());

        if (lastline || willCollide)
        {
            MakeNewAlloy();
            for (int i = 0; i < ingots.Count(); i++)
            {
                int m = Convert.ToInt32(ingots[i].transform.position.x / 0.64f);
                int n = Convert.ToInt32(ingots[i].transform.position.y / 0.64f);
                if (n < gameBoard.height && n >= 0 && m < gameBoard.width && m >= 0)
                {
                    gameBoard.board[m, n] = ingots[i].gameObject;
                }
            }
            return;
        }

        Vector3 vec = new Vector3(currentAlloy.transform.position.x + offset.x, currentAlloy.transform.position.y + offset.y, currentAlloy.transform.position.z);
        currentAlloy.transform.position = vec;
    }

    private void MakeNewAlloy()
    {
        currentAlloy = spawner.NewAlloy();
        currentAlloy = Instantiate(currentAlloy, pos, Quaternion.identity) as GameObject;
        currentAlloy.GetComponent<AlloyLayout>().SetupAlloy();
    }

    private string MakeGameBoard()
    {
        string board = "0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 " + Environment.NewLine;
        for (int i = 0; i < gameBoard.height; i++)
        {
            board += string.Format("{0,3}", i);
            for (int j = 0; j < gameBoard.width; j++)
            {
                board += gameBoard.board[j, i] == null ? ". " : "x ";
            }
            board += Environment.NewLine;
        }
        return board;
    }
}
