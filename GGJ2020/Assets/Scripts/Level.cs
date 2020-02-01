using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    

    //Level parameters
    private int _NPieces = 10;
    private int _NRobots = 3;

    //Object containers
    public Queue<Piece> _pieces;
    public Queue<Robot> _robots;

    //Object references to instantiate objects
    public GameObject _robotPrefab;
    public GameObject _piecePrefab;
    public GameObject belt1, belt2;

    // Start is called before the first frame update. The Awake is called even before Start.
    void Start()
    {
        Sprite[] sprites = { Resources.Load<Sprite>("Sprites/leg_l"), Resources.Load<Sprite>("Sprites/leg_r"), Resources.Load<Sprite>("Sprites/arm_l"), Resources.Load<Sprite>("Sprites/arm_r") };
        SpriteRenderer spriteR;

        //Initialize data structures
        _pieces = new Queue<Piece>();
        _robots = new Queue<Robot>();

        //Initialize robots (will initialize its own pieces)
        for(int i = 0; i<_NRobots;i++)
        {
            if (Instantiate(_robotPrefab).TryGetComponent(out Robot r))
            {
                r.Initialize();
                _robots.Enqueue(r);                                          //Enter the robot to the data structure.                          //Enter the robot to the data structure.
                foreach (Piece piece in r.GetPieces())
                {
                    _pieces.Enqueue(piece);                                     //Add the robot's pieces to the data structure.
                }
            }
        }
        
        //Initialize aditional pieces to add complexity
        for (int i=_pieces.Count; i<_NPieces; i++)
        {
            if (Instantiate(_piecePrefab).TryGetComponent(out Piece p)) {
                p.Initialize();
                _pieces.Enqueue(p);
            }
        }

        foreach(Piece p in _pieces)
        {
            spriteR = p.transform.GetChild(0).GetComponent<SpriteRenderer>();
            switch (p.GetComponent<Piece>()._type)
            {
                case Piece.pieceType.leg_l:
                    spriteR.sprite = sprites[0];
                    break;
                case Piece.pieceType.leg_r:
                    spriteR.sprite = sprites[1];
                    break;
                case Piece.pieceType.arm_l:
                    spriteR.sprite = sprites[2];
                    break;
                case Piece.pieceType.arm_r:
                    spriteR.sprite = sprites[3];
                    break;
             }
        }

        //Shuffle pieces

        //Initialize the belts
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Queue<Piece> GetPiecesQueue()
    {
        return _pieces;
    }
}
