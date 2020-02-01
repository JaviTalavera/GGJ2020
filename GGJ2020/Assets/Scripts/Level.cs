using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    

    //Level parameters
    private int _NPieces = 10;
    private int _NRobots = 4;

    //Object containers
    public Queue<Piece> _pieces;
    public Queue<Robot> _robots;
    public Transform _spawnPoint;

    public HookController[] _hooks;

    //Object references to instantiate objects
    public GameObject _robotPrefab;
    public GameObject _piecePrefab;
    public Belt belt1, belt2;

    // Start is called before the first frame update. The Awake is called even before Start.
    void Start()
    {
        GameObject[] sprites = { Resources.Load<GameObject>("Leg_l"), Resources.Load<GameObject>("Leg_r"), Resources.Load<GameObject>("Arm_l"), Resources.Load<GameObject>("Arm_r") };

        //Initialize data structures
        _pieces = new Queue<Piece>();
        _robots = new Queue<Robot>();

        //Initialize robots (will initialize its own pieces)
        for(int i = 0; i<_NRobots;i++)
        {
            if (Instantiate(_robotPrefab, i < _hooks.Length ? _hooks[i].transform.position : _spawnPoint.position, Quaternion.identity).TryGetComponent(out Robot r))
            {
                r.Initialize();
                if (i < _hooks.Length)
                    _hooks[i].SetRobot(r);
                else
                {
                    _robots.Enqueue(r);
                    r.gameObject.SetActive(false);
                }                                        //Enter the robot to the data structure.                          //Enter the robot to the data structure.
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
            //spriteR = p.transform.GetChild(0).GetComponent<SpriteRenderer>();
            switch (p.GetComponent<Piece>()._type)
            {
                case Piece.pieceType.leg_l:
                    var go_legl = Instantiate(sprites[0], 3 * Vector3.up + Vector3.left, sprites[0].transform.rotation);
                    go_legl.transform.SetParent(p.transform);
                    break;
                case Piece.pieceType.leg_r:
                    var go_legr = Instantiate(sprites[1], 3 * Vector3.up + Vector3.left, sprites[0].transform.rotation);
                    go_legr.transform.SetParent(p.transform);
                    break;
                case Piece.pieceType.arm_l:
                    var go_arml = Instantiate(sprites[2], 3 * Vector3.up + Vector3.left, sprites[0].transform.rotation);
                    go_arml.transform.SetParent(p.transform);
                    break;
                case Piece.pieceType.arm_r:
                    var go_armr = Instantiate(sprites[3], 3 * Vector3.up + Vector3.left, sprites[0].transform.rotation);
                    go_armr.transform.SetParent(p.transform);
                    break;
             }
        }

        //Shuffle pieces

        //Initialize the belts
        belt1.Initialize(); belt2.Initialize();
    }

    public Queue<Robot> GetRobotsQueue() => _robots;


    public Queue<Piece> GetPiecesQueue() => _pieces;
}
