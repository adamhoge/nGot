using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AlloyLayout : MonoBehaviour
{
    public static readonly string[] AlloyTypes = { "Square", "Line", "L", "J", "S", "Z", "T" };
    public static readonly Dictionary<string, List<Vector3>> Positions = new Dictionary<string, List<Vector3>>()
        {
            { "Square", new List<Vector3>()
                {
                    new Vector3(0.0f, 0.0f),
                    new Vector3(0.64f, 0.0f),
                    new Vector3(0.0f, 0.64f),
                    new Vector3(0.64f, 0.64f)
                }
            },
            { "Line", new List<Vector3>()
                {
                    new Vector3(0.0f, -0.64f),
                    new Vector3(0.0f, 0.0f),
                    new Vector3(0.0f, 0.64f),
                    new Vector3(0.0f, 1.28f),
                }
            },
            { "L", new List<Vector3>()
                {
                    new Vector3(0.0f, 0.0f),
                    new Vector3(0.64f, 0.0f),
                    new Vector3(0.0f, 0.64f),
                    new Vector3(0.0f, 1.28f)
                }
            },
            { "J", new List<Vector3>()
                {
                    new Vector3(0.0f, 0.0f),
                    new Vector3(-0.64f, 0.0f),
                    new Vector3(0.0f, 0.64f),
                    new Vector3(0.0f, 1.28f)
                }
            },
            { "S", new List<Vector3>()
                {
                    new Vector3(0.0f, 0.0f),
                    new Vector3(-0.64f, 0.0f),
                    new Vector3(0.0f, 0.64f),
                    new Vector3(0.64f, 0.64f)
                }
            },
            { "Z", new List<Vector3>()
                {
                    new Vector3(0.0f, 0.0f),
                    new Vector3(0.64f, 0.0f),
                    new Vector3(0.0f, 0.64f),
                    new Vector3(-0.64f, 0.64f)
                }
            },
            { "T", new List<Vector3>()
                {
                    new Vector3(0.0f, 0.0f),
                    new Vector3(0.0f, -0.64f),
                    new Vector3(-0.64f, 0.0f),
                    new Vector3(0.64f, 0.0f)
                }
            }
        };

    private string _myType = string.Empty;
    public string Type { get { return _myType; } }

    private GameObject[] _myIngots = new GameObject[4];
    public IEnumerable<GameObject> MyIngots { get { return _myIngots; } }

    List<GameObject> ingots = new List<GameObject>();

    public void Awake()
    {
        ingots = Resources.LoadAll<GameObject>("Ingots").ToList();
    }

	// Use this for initialization
	public void Start ()
    {
	
	}
	
	// Update is called once per frame
	public void Update ()
    {
	
	}

    public void SetupAlloy ()
    {
        // choose a type of piece
        _myType = AlloyTypes[Random.Range(0, AlloyTypes.Length)];
        // choose an ingot type
        var ingot = ingots[Random.Range(0, ingots.Count)];
        // create all the ingots
        for (int i = 0; i < _myIngots.Length; i++)
        {
            Vector3 pos = new Vector3(Positions[_myType][i].x + this.transform.position.x,
                                      Positions[_myType][i].y + this.transform.position.y);
            _myIngots[i] = Instantiate(ingot, pos, Quaternion.identity) as GameObject;
            _myIngots[i].transform.parent = this.gameObject.transform;
        }
    }
}