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
        bool left = Input.GetButtonDown("Left");
        bool right = Input.GetButtonDown("Right");
        bool down = Input.GetButtonDown("Down");

        Vector3 offset = Vector3.zero;
        offset.x += left ? -0.64f : 0.0f;
        offset.x += right ? 0.64f : 0.0f;
        offset.y += down ? -0.64f : 0.0f;

        bool counterClock = Input.GetButtonDown("Counter Clockwise");
        bool clock = Input.GetButtonDown("Clockwise");

        float rotation = counterClock ? 90.0f : 0.0f;
        rotation += clock ? -90.0f : 0.0f;

        MoveAlloy(offset, rotation);
	}

    public void DropAlloy()
    {
        MoveAlloy(new Vector3(0.0f, -0.64f), 0.0f);
    }

    private void MoveAlloy(Vector3 offset, float degrees)
    {
        // pretend translate the alloy first
        currentAlloy.transform.Translate(offset);
        currentAlloy.transform.Rotate(0.0f, 0.0f, degrees);
        var ingots = currentAlloy.GetComponentsInChildren<Transform>().Where(t => t.tag == "Ingot").ToArray();

        int[] x = new int[ingots.Count()];
        int[] y = new int[ingots.Count()];
          
        // save the new positions
        for (int i = 0; i < ingots.Count(); i++)
        {
            Vector3 newPos = ingots[i].transform.position;
            x[i] = Convert.ToInt32(newPos.x / 0.64f);
            y[i] = Convert.ToInt32(newPos.y / 0.64f);
        }

        // move the alloy back
        currentAlloy.transform.Rotate(0.0f, 0.0f, -degrees);
        currentAlloy.transform.Translate(-offset);

        bool lastline = false;
        bool willCollide = false;

        for (int i = 0; i < x.Length; i++)
        {
            // out of bounds, don't update
            if (x[i] < 0 || x[i] >= gameBoard.width)
            {
                return;
            }

            // below the board, need new alloy
            if (y[i] < 0)
            {
                lastline = true;
                continue;
            }

            // this piece is not on the board, don't check it
            if (y[i] >= gameBoard.height)
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

        // get the current ingots, not the moved ones
        ingots = currentAlloy.GetComponentsInChildren<Transform>().Where(t => t.tag == "Ingot").ToArray();
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

        currentAlloy.transform.Translate(offset);
        currentAlloy.transform.Rotate(0.0f, 0.0f, degrees);
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
