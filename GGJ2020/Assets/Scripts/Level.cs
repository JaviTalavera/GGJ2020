using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    

    //Level parameters
    private int _NPieces = 10;
    private int _NRobots = 3;

    //Object containers
    public Queue<Piece> _pieces;
    public Queue<GameObject> _robots;

    //Object references to instantiate objects
    public GameObject _robotPrefab;
    public GameObject _piecePrefab;

    // Start is called before the first frame update. The Awake is called even before Start.
    void Start()
    {
        Sprite[] sprites = { Resources.Load<Sprite>("Sprites/leg_l"), Resources.Load<Sprite>("Sprites/leg_r"), Resources.Load<Sprite>("Sprites/arm_l"), Resources.Load<Sprite>("Sprites/arm_r"), Resources.Load<Sprite>("Sprites/head") };
        SpriteRenderer spriteR;

        //Initialize data structures
        _pieces = new Queue<Piece>();
        _robots = new Queue<GameObject>();

        //Initialize robots (will initialize it's own pieces)
        for(int i = 0; i<1;i++)
        {
            var r = Instantiate(_robotPrefab).GetComponent<Robot>();
            r.Intialize();
            _robots.Enqueue(r.gameObject);                                             //Enter the robot to the data structure.
            foreach (Piece piece in r.GetPieces())   
            {
                _pieces.Enqueue(piece);                                     //Add the robot's pieces to the data structure.
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
                case Piece.pieceType.head:
                    spriteR.sprite = sprites[4];
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
}
