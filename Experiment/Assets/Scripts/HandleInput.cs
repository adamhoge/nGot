using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class HandleInput : MonoBehaviour 
{
    GameObject currentAlloy = null;
    SpawnAlloy spawner = null;
    Vector3 pos;
    Vector3 cameraUp;
    CreateGameBoard gameBoard = null;

    public float timeIncrements;

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
        //var camera = GameObject.FindGameObjectWithTag("Main Camera");
        //if (camera != null)
        //{
        //    cameraUp = camera.transform.up;
        //}

        MakeNewAlloy();
        //InvokeRepeating("DropAlloy", 0.5f, timeIncrements);
	}

	// Update is called once per frame
	void Update ()
    {
        bool left = Input.GetButtonDown("Left");
        bool right = Input.GetButtonDown("Right");
        bool down = Input.GetButtonDown("Down");

        float x = 0.0f, y = 0.0f;
        x += left ? -0.64f : 0.0f;
        x += right ? 0.64f : 0.0f;
        y += down ? -0.64f : 0.0f;

        bool counterClockwise = Input.GetButtonDown("Counter Clockwise");
        bool clockwise = Input.GetButtonDown("Clockwise");

        float rotation = counterClockwise ? 90.0f : 0.0f;
        rotation += clockwise ? -90.0f : 0.0f;

        // get the individual ingots from the alloy
        var ingots = currentAlloy.GetComponentsInChildren<Transform>().Where(t => t.tag == "Ingot").ToArray();

        int[] xs = new int[ingots.Count()];
        int[] ys = new int[ingots.Count()];

        // save the positions
        for (int i = 0; i < ingots.Count(); i++)
        {
            Vector3 newPos = ingots[i].transform.position;
            xs[i] = Convert.ToInt32(newPos.x / 0.64f);
            ys[i] = Convert.ToInt32(newPos.y / 0.64f);
        }

        if (WillStop(xs, ys))
        {
            SaveCurrent(ingots);
            MakeNewAlloy();
            return;
        }

        if (left || right || down)
        {
            MoveAlloy(x, y);
        }

        if (clockwise || counterClockwise)
        {
            RotateAlloy(rotation);
        }

        Debug.Log(MakeGameBoard());
	}

    public void DropAlloy()
    {
        MoveAlloy(0.0f, -0.64f);
    }

    private void RotateAlloy(float degrees)
    {
        currentAlloy.transform.Rotate(0.0f, 0.0f, degrees);
        var ingots = currentAlloy.GetComponentsInChildren<Transform>().Where(t => t.tag == "Ingot").ToArray();

        int[] xs = new int[ingots.Count()];
        int[] ys = new int[ingots.Count()];

        // save the positions
        for (int i = 0; i < ingots.Count(); i++)
        {
            Vector3 newPos = ingots[i].transform.position;
            xs[i] = Convert.ToInt32(newPos.x / 0.64f);
            ys[i] = Convert.ToInt32(newPos.y / 0.64f);
            // don't update out of bounds
            if (xs[i] < 0 || xs[i] >= gameBoard.width || ys[i] <= 0)
            {
                currentAlloy.transform.Rotate(0.0f, 0.0f, -degrees);
                return;
            }
        }

        // rotate back on collision
        if (Collides(xs, ys))
        {
            currentAlloy.transform.Rotate(0.0f, 0.0f, -degrees);
        }
    }

    private void MoveAlloy(float x, float y)
    {
        // what direction are we facing?
        Vector3 offset = Vector3.zero;
        offset.x += x;
        offset.y += y;
        //float cos = Vector3.Dot(cameraUp, currentAlloy.transform.up);
        //Debug.Log(cos.ToString());

        var ingots = currentAlloy.GetComponentsInChildren<Transform>().Where(t => t.tag == "Ingot").ToArray();

        int[] xs = new int[ingots.Count()];
        int[] ys = new int[ingots.Count()];

        // save the positions
        for (int i = 0; i < ingots.Count(); i++)
        {
            Vector3 newPos = ingots[i].transform.position + offset;
            xs[i] = Convert.ToInt32(newPos.x / 0.64f);
            ys[i] = Convert.ToInt32(newPos.y / 0.64f);
            // don't update out of bounds
            if (xs[i] < 0 || xs[i] >= gameBoard.width)
            {
                return;
            }
        }

        if (!Collides(xs, ys))
        {
            currentAlloy.transform.Translate(offset);
        }
    }

    private bool WillStop(int[] xs, int[] ys)
    {
        bool lastline = false;
        for (int i = 0; i < xs.Length && i < ys.Length; ++i)
        {
            if (ys[i] == 0)
            {
                lastline = true;
            }

            if (ys[i] < 1 || ys[i] >= gameBoard.height + 1 ||
                xs[i] < 0 || xs[i] >= gameBoard.width)
            {
                continue;
            }

            if (gameBoard.board[xs[i], ys[i] - 1] != null)
            {
                lastline = true;
            }
        }

        return lastline;
    }

    private bool Collides(int[] xs, int[] ys)
    {
        bool willCollide = false;
        for (int i = 0; i < xs.Length && i < ys.Length; i++)
        {
            // out of bounds, don't update
            if (xs[i] < 0 || xs[i] >= gameBoard.width)
            {
                continue;
            }

            // out of bounds, don't update
            if (ys[i] < 0 || ys[i] >= gameBoard.height)
            {
                continue;
            }

            // otherwise check for a piece where we want to move
            if (gameBoard.board[xs[i], ys[i]] != null)
            {
                willCollide = true;
            }
        }
        return willCollide;
    }

    private void SaveCurrent(IEnumerable<Transform> pieces)
    {
        foreach (var piece in pieces)
        {
            int x = Convert.ToInt32(piece.position.x / 0.64f);
            int y = Convert.ToInt32(piece.position.y / 0.64f);
            if (x < 0 || x > gameBoard.width || y < 0 || y > gameBoard.height)
            {
                continue;
            }
            gameBoard.board[x, y] = piece.gameObject;
        }
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
