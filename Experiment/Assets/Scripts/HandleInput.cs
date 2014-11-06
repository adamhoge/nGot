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
    GameObject camera;
    CreateGameBoard gameBoard = null;

    public float timeIncrements;

    private enum Direction { Up, Down, Left, Right };

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
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        if (camera == null)
        {
            // do something for no camera
            // should never happen
        }
        MakeNewAlloy();
        InvokeRepeating("DropAlloy", 0.5f, timeIncrements);
	}

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Quit"))
        {
            Application.Quit();
        }
        bool left = Input.GetButtonDown("Left");
        bool right = Input.GetButtonDown("Right");
        bool down = Input.GetButtonDown("Down");
        bool up = Input.GetButtonDown("Up");

        bool counterClockwise = Input.GetButtonDown("Counter Clockwise");
        bool clockwise = Input.GetButtonDown("Clockwise");

        if (left)
            MoveAlloy(currentAlloy, Direction.Left);
        if (right)
            MoveAlloy(currentAlloy, Direction.Right);
        if (down)
            MoveAlloy(currentAlloy, Direction.Down);
        if (up)
            MoveAlloy(currentAlloy, Direction.Up);

        if (counterClockwise)
            RotateAlloy(currentAlloy, -90.0f);
        if (clockwise)
            RotateAlloy(currentAlloy, 90.0f);

        bool quit = false;
        // assume piece is in a valid position
        var ingots = currentAlloy.GetComponentsInChildren<Transform>().Where(t => t.tag == "Ingot");
        if (Map(Stops, ingots, gameBoard, false))
        {
            quit = SaveCurrent(ingots);
            // delete rows
            MakeNewAlloy();
        }

        Debug.Log(MakeGameBoard());
        if (quit)
        {
            Application.LoadLevel("main");
        }
	}

    public void DropAlloy()
    {
        MoveAlloy(currentAlloy, Direction.Down);
    }

    private void RotateAlloy(GameObject alloy, float degrees)
    {
        alloy.transform.Rotate(0.0f, 0.0f, degrees);
        var ingots = alloy.GetComponentsInChildren<Transform>().Where(t => t.tag == "Ingot");
        if (Map(OutOfBounds, ingots, gameBoard, false) ||
            Map(Collides, ingots, gameBoard, false))
        {
            alloy.transform.Rotate(0.0f, 0.0f, -degrees);
        }
    }

    private void MoveAlloy(GameObject alloy, Direction d)
    {
        // what direction are is the current alloy up vector facing?
        Vector3 offset = Vector3.zero;
        int up = Convert.ToInt32(Vector3.Dot(camera.transform.up, currentAlloy.transform.up));
        int right = Convert.ToInt32(Vector3.Dot(camera.transform.right, currentAlloy.transform.up));

        // normal
        if (up == 1)
        {
            switch (d)
            {
                case Direction.Up:
                    offset.y = 0.64f;
                    break;
                case Direction.Down:
                    offset.y = -0.64f;
                    break;
                case Direction.Left:
                    offset.x = -0.64f;
                    break;
                case Direction.Right:
                    offset.x = 0.64f;
                    break;
            }
        }
        // 180 degress, upside down
        else if (up == -1)
        {
            switch (d)
            {
                case Direction.Up:
                    offset.y = -0.64f;
                    break;
                case Direction.Down:
                    offset.y = 0.64f;
                    break;
                case Direction.Left:
                    offset.x = 0.64f;
                    break;
                case Direction.Right:
                    offset.x = -0.64f;
                    break;
            }
        }
        // 90 degrees, right
        else if (right == 1)
        {
            switch (d)
            {
                case Direction.Up:
                    offset.x = -0.64f;
                    break;
                case Direction.Down:
                    offset.x = 0.64f;
                    break;
                case Direction.Left:
                    offset.y = -0.64f;
                    break;
                case Direction.Right:
                    offset.y = 0.64f;
                    break;
            }
        }
        // -90 degrees, left
        else if (right == -1)
        {
            switch (d)
            {
                case Direction.Up:
                    offset.x = 0.64f;
                    break;
                case Direction.Down:
                    offset.x = -0.64f;
                    break;
                case Direction.Left:
                    offset.y = 0.64f;
                    break;
                case Direction.Right:
                    offset.y = -0.64f;
                    break;
            }
        }
        alloy.transform.Translate(offset);
        var ingots = alloy.GetComponentsInChildren<Transform>().Where(t => t.tag == "Ingot");
        if (Map(OutOfBounds, ingots, gameBoard) ||
            Map(Collides, ingots, gameBoard))
        {
            alloy.transform.Translate(-offset);
        }
    }

    private TResult Map<TResult>(Func<Transform, CreateGameBoard, TResult> f, IEnumerable<Transform> ingots, CreateGameBoard board, TResult initial) where TResult : IEquatable<TResult>
    {
        foreach (var ingot in ingots)
        {
            TResult temp = f(ingot, board);
            if (!initial.Equals(temp))
            {
                return temp;
            }
        }
        return initial;
    }

    private bool Map(Func<Transform, CreateGameBoard, bool> f, IEnumerable<Transform> ingots, CreateGameBoard board)
    {
        return Map<bool>(f, ingots, board, false);
    }

    private bool OutOfBounds(Transform ingot, CreateGameBoard board)
    {
        int x = Convert.ToInt32(ingot.transform.position.x / 0.64f);
        int y = Convert.ToInt32(ingot.transform.position.y / 0.64f);
        //Debug.Log(string.Format("{0}, {1}", x, y));
        if (x < 0 || x >= board.width)
        {
            return true;
        }
        if (y < 0)
        {
            return true;
        }
        return false;
    }

    private bool Collides(Transform ingot, CreateGameBoard board)
    {
        int x = Convert.ToInt32(ingot.transform.position.x / 0.64f);
        int y = Convert.ToInt32(ingot.transform.position.y / 0.64f);
        if ((x >= 0 && x < board.width) &&
            (y >= 0 && y < board.height) && 
            (board.board[x, y] != null))
        {
            return true;
        }
        return false;
    }

    // assume the ingot is on the board
    private bool Stops(Transform ingot, CreateGameBoard board)
    {
        int x = Convert.ToInt32(ingot.transform.position.x / 0.64f);
        int y = Convert.ToInt32(ingot.transform.position.y / 0.64f) - 1;
        // last line
        if (y < 0)
        {
            return true;
        }
        // above the board
        if (y >= board.height)
        {
            return false;
        }
        // piece below
        if (board.board[x, y] != null)
        {
            return true;
        }
        return false; 
    }

    private bool SaveCurrent(IEnumerable<Transform> pieces)
    {
        bool done = false;
        foreach (var piece in pieces)
        {
            int x = Convert.ToInt32(piece.position.x / 0.64f);
            int y = Convert.ToInt32(piece.position.y / 0.64f);
            if (x < 0 || x >= gameBoard.width || y < 0)
            {
                continue;
            }
            if (y >= gameBoard.height)
            {
                done = true;
                continue;
            }
            gameBoard.board[x, y] = piece.gameObject;
        }
        return done;
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
