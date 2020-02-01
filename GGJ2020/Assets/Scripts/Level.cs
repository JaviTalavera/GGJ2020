using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    //Level parameters
    private int _NPieces = 10;
    private int _NRobots = 3;

    //Object containers
    public Queue<GameObject> _pieces;
    public Queue<GameObject> _robots;

    //Object references to instantiate objects
    public GameObject _robotPrefab;
    public GameObject _piecePrefab;
    public GameObject belt1, belt2;

    // Start is called before the first frame update. The Awake is called even before Start.
    void Start()
    {
        
        //Initialize data structures
        _pieces = new Queue<GameObject>();
        _robots = new Queue<GameObject>();

        //Initialize robots (will initialize its own pieces)
        for(int i = 0; i<_NRobots;i++)
        {
            GameObject r = Instantiate(_robotPrefab);
            r.GetComponent<Robot>().Initialize();
            _robots.Enqueue(r);                                             //Enter the robot to the data structure.
            foreach (GameObject piece in r.GetComponent<Robot>().GetPieces())   
            {
                _pieces.Enqueue(piece);                                     //Add the robot's pieces to the data structure.
            }
        }
        
        //Initialize aditional pieces to add complexity
        for (int i=_pieces.Count; i<_NPieces; i++)
        {
            GameObject p = Instantiate(_piecePrefab);
            p.GetComponent<Piece>().Initialize();
            _pieces.Enqueue(p);
        }

        belt1.GetComponent<Belt>().Initialize();
        belt2.GetComponent<Belt>().Initialize();

        //Shuffle pieces

        //Initialize the belts
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Queue<GameObject> GetPiecesQueue()
    {
        return _pieces;
    }
}
