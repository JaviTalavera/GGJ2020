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

    // Start is called before the first frame update. The Awake is called even before Start.
    void Awake()
    {
        //Initialize robots (will initialize it's own pieces)
        for(int i = 0; i<_NRobots;i++)
        {
            GameObject r = Instantiate(_robotPrefab);
            _robots.Enqueue(r);                                             //Enter the robot to the data structure.
            foreach (GameObject piece in r.GetComponent<Robot>()._pieces)   
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

        //Shuffle pieces

        //Initialize the belts

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
