using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Robot : MonoBehaviour
{
    //General info
    [SerializeField] public static int maxPieces = 4;

    //Pieces' variables
    private int _numPieces;
    private int _piecesTypeMask;                    //Stores a 1 in the 1st position if the leg_l is needed, 1 in the 2nd if leg_r is needed...
                                                    //There can (and surely will) be needed both of them --> 00011.

    public Transform _arm_l; 
    public Transform _arm_r;
    public Transform _leg_l;
    public Transform _leg_r;
    public GameObject _conn_arm_l;
    public GameObject _conn_arm_r;
    public GameObject _conn_leg_l;
    public GameObject _conn_leg_r;
    public ParticleSystem _part_arm_l;
    public ParticleSystem _part_arm_r;
    public ParticleSystem _part_leg_l;
    public ParticleSystem _part_leg_r;

    private int _repairedPieces = 0;

    //Container
    private Piece[] _pieces;        //Stores the characteristics of the pieces the robot will need in order to be repaired.
    private int[] _piecesTypes;
    
    //Reference to a generic piece
    [SerializeField] public GameObject _piecePrefab;

    public void Initialize()
    {
        //Start data structures 
        _piecesTypes = new int[5];

        //Set number of needed pieces
        _numPieces = Random.Range(1, maxPieces);

        //Create those pieces. This piece will be used in two ways: 
        //      First, as the container of the characteristics the neeeded piece must have to repair the robot. (Stored in the robot)
        //      Second, as the piece that fits in the robot. It will be added to the game data structure (queue); and will appear 
        //      in any moment in the belt. (Stored in the Level queue)
        _pieces = new Piece[_numPieces];


        //Set pieces' type. (Caution, you must ensure the pieces aren't for the same body part.)
        _piecesTypes = GenerateTypes(_numPieces);

        for (int i = 0; i < _numPieces; i++)
        {
            if (Instantiate(_piecePrefab).TryGetComponent(out Piece pieza))
            {
                pieza.Initialize(_piecesTypes[i]);
                _pieces[i] = pieza;
                switch(_pieces[i]._type)
                {
                    case Piece.pieceType.arm_l:
                        {
                            _arm_l.gameObject.SetActive(false);
                            PintaConnector(_conn_arm_l, _pieces[i]);
                            _conn_arm_l.SetActive(true);
                            _part_arm_l.gameObject.SetActive(true);
                            break;
                        }
                    case Piece.pieceType.arm_r:
                        {
                            _arm_r.gameObject.SetActive(false);
                            PintaConnector(_conn_arm_r, _pieces[i]);
                            _conn_arm_r.SetActive(true);
                            _part_arm_r.gameObject.SetActive(true);
                            break;
                        }
                    case Piece.pieceType.leg_l:
                        {
                            _leg_l.gameObject.SetActive(false);
                            PintaConnector(_conn_leg_l, _pieces[i]);
                            _conn_leg_l.SetActive(true);
                            _part_leg_l.gameObject.SetActive(true);
                            break;
                        }
                    case Piece.pieceType.leg_r:
                        {
                            _leg_r.gameObject.SetActive(false);
                            PintaConnector(_conn_leg_r, _pieces[i]);
                            _conn_leg_r.SetActive(true);
                            _part_leg_r.gameObject.SetActive(true);
                            break;
                        }
                }
            }
        }
    }

    public void PintaConnector(GameObject connector, Piece piece)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i < piece._numberConectors)
            {
                if (connector.transform.GetChild(i).TryGetComponent(out SpriteRenderer spriteRenderer))
                {
                    spriteRenderer.color = piece._colors[i];      //Asign color to component's spriteRenderer. 
                                                            //(Must be a sprite. If not, pick the right component; not SpriteRenderer)
                };
            }
            else
                connector.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public Rigidbody2D GetHeadRigidBody()
    {
        return transform.GetComponent<Rigidbody2D>();
    }

    //Getters and Setters
    public Piece[] GetPieces()
    {
        return _pieces;
    }


    //Functional methods

    /*
     *  Encapsulates createMask and traduceMask to simplify the understanding in main code.
     */
    public int[] GenerateTypes(int numPieces) 
    {
        int mask = CreateMask(numPieces);
        int[] types = new int[4];
        types = TraduceMask(mask);

        return types;
    }

    /*
    *  Creates a random mask without repeating a number.
    */
    private int CreateMask(int numPieces)
    {
        int mask = 0;

        for (int i = 0; i < numPieces; i++)
        {
            int randNum;

            do
            {
                randNum = Random.Range(0, 5);
            } while ((1 << randNum & mask) == 1);  //Mientras que el componente ya esté pillao, generar otro.
                                                    //Esto es para evitar que se generen dos cabezas. 
                                                    //& es and pero bit a bit.

            switch (randNum)
            {
                case 0:
                    mask += 1;       //00001
                    break;
                case 1:
                    mask += 2;       //00010
                    break;
                case 2:
                    mask += 4;       //00100
                    break;
                case 3:
                    mask += 8;       //01000
                    break;
             }
        }

        return mask;
    }

    /*
     *  Traduces the mask to enum integers.
     */
    private int[] TraduceMask(int mask)
    {
        int[] result = new int[8];
        int r_index = 0;

        for(int i = 0; i < 8; i++)
        {
            if(mask%2 == 1)  //Si el último bit es 1...
            {
                result[r_index] = i;
                r_index++;
            }

            mask=mask >> 1; //En cada iteración se translada una posición para analizar el siguiente bit.
        }

        return result;
    }

    //Piece check methods
    public bool checkCorrectPiece(Piece piece)
    {
        for(int i = 0; i< _numPieces; i++)
        {
            if (_pieces[i] == piece)
            {
                return true;
            }
        }
 
        return false;
    }

    public bool IsRepaired() => _pieces.Length == _repairedPieces;

    public void Repair(Piece p)
    {
        var particulas = GameObject.FindWithTag("Particulas").GetComponent<ParticleSystem>();
        switch (p._type)
        {
            case Piece.pieceType.arm_l:
                {
                    _arm_l.gameObject.SetActive(true);
                    particulas.transform.position = _arm_l.position;
                    particulas.Play();
                    _part_arm_l.Stop();
                    _part_arm_l.gameObject.SetActive(false);
                    _conn_arm_l.SetActive(false);
                    break;
                }
            case Piece.pieceType.arm_r:
                {
                    _arm_r.gameObject.SetActive(true);
                    particulas.transform.position = _arm_r.position;
                    particulas.Play();
                    _part_arm_r.Stop();
                    _part_arm_r.gameObject.SetActive(false);
                    _conn_arm_r.SetActive(false);
                    break;
                }
            case Piece.pieceType.leg_r:
                {
                    _leg_r.gameObject.SetActive(true);
                    particulas.transform.position = _leg_r.position;
                    particulas.Play();
                    _part_leg_r.Stop();
                    _part_leg_r.gameObject.SetActive(false);
                    _conn_leg_r.SetActive(false);
                    break;
                }
            case Piece.pieceType.leg_l:
                {
                    _leg_l.gameObject.SetActive(true);
                    particulas.transform.position = _leg_l.position;
                    particulas.Play();
                    _part_leg_l.Stop();
                    _part_leg_l.gameObject.SetActive(false);
                    _conn_leg_l.SetActive(false);
                    break;
                }
        }
        _repairedPieces++;
        if (IsRepaired())
        {
            Debug.Log("REPARADO");
            GameObject.FindWithTag("LevelManager").GetComponent<Level>().RobotRepaired();
        }
        else
        {
            Debug.Log(_repairedPieces + "/" + _pieces.Length);
        }
    }
}
