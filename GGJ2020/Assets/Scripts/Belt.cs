using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Belt : MonoBehaviour
{
    public KeyCode accelerate;

    //Belt properties
    public float _speed;
    public float speedUp = 1;
    int nPiecesPerBelt = 3;
    public Transform _otherBelt;

    //Pieces variables
    public Transform _initPos;
    public float _initOffset;

    //Containers
    private Piece[] _pieces;
    public GameObject [] Spawners;
    public bool _generatePieces = false;

    //References
    public Level _level;

    public void Initialize()
    {      
        //Find level reference
        _level = GameObject.FindGameObjectWithTag("LevelManager")?.GetComponent<Level>();

        //Initialize data structures
        nPiecesPerBelt = Spawners.Length;
        _pieces = new Piece[0];

        _generatePieces = true;
        GetComponentInParent<Animator>().SetTrigger("start");
    }

    void FixedUpdate()
    {
        if (_generatePieces && !GameManager.IsPause)
            transform.position += Vector3.right * _speed * speedUp * Time.fixedDeltaTime;
        if(Input.GetKey(accelerate)) speedUp = 2;
        else speedUp = 1;
    }

    public void Refresh() //Llamado desde el trigger
    {
        if (_pieces.Any())
        {
            foreach (Piece p in _pieces)
            {
                if (p != null && p.transform.parent != null)
                {
                    p.gameObject.SetActive(false);
                    if (!p.HasBeenUsed())
                    {
                        _level.GetPiecesQueue().Enqueue(p);
                    }
                }
            }
        }
        GetNew();
    }

    public void GetNew()
    {
        _pieces = new Piece[nPiecesPerBelt];
        for (int i = 0; i < nPiecesPerBelt; i++)
        {
            if (_level.GetPiecesQueue().Any())
            {
                _pieces[i] = _level.GetPiecesQueue().Dequeue(); //ERROR DE EJECUCION
                _pieces[i].gameObject.transform.position = Spawners[i].transform.position;
                _pieces[i].transform.SetParent(Spawners[i].transform);
                _pieces[i].gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BeltBorder"))
        {
            transform.position = _initPos.position;
            Refresh();
        }
    }

}

//Nota:
    //Recordar que en el ontrigger, al hacer enqueue de las piezas hay que comprobar que ninguna ha sido usada sobre algún robot.
    //La comprobación se hace en función de un bit que se activa cuando una pieza es usada con un robot. (Como sugerencia)