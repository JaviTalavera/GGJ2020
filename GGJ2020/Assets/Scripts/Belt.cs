using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : MonoBehaviour
{
    //Belt properties
    public float _speed;
    const int nPiecesPerBelt = 3;

    //Pieces variables
    public Transform _initPos;
    public float _initOffset;

    //Containers
    private GameObject[] _pieces;

    //References
    private Level _level;

    private void Start()
    {
        //Find level reference
        _level = GameObject.FindGameObjectsWithTag("LevelManager")[0].GetComponent<Level>();

        //Initialize data structures
        _pieces = new GameObject[3];

        //Pick the first 3 pieces.
        for(int i=0; i < nPiecesPerBelt; i++)
        {
            _pieces[i] = _level._pieces.Dequeue();
            _pieces[i].transform.position = _initPos.position + new Vector3(0, 0, _initOffset * i); //Set initial pos.
                                                                                                    //CAUTION: I think transform is read only variable. The assignment won't modify 
                                                                                                    //piece's transform because in fact it's a copy of it. TAKE A LOOK AT IT WHEN DEBUGGING.
            _pieces[i].transform.SetParent(transform);
            _pieces[i].gameObject.SetActive(true);
        }
        
    }

    void Update()
    {
        transform.position += Vector3.right * _speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision between Belt and border");
        if(collision.gameObject.CompareTag("BeltBorder"))        //Si el elemento con el que colisiona es el tope. (por si acaso)
        {
            GameObject tempPiece;

            for (int i = 0; i < nPiecesPerBelt; i++)
            {
                //Hide pieces
                _pieces[i].gameObject.SetActive(false);

                //Store it into the Level queue (or not)
                //Yet to be implemented. For the moment all the pieces reentry the queue.
                _level._pieces.Enqueue(_pieces[i]);

                //Pick new piece
                tempPiece = _pieces[i];
                _pieces[i] = _level._pieces.Dequeue();

                _pieces[i].transform.position = tempPiece.transform.position;       //Locate
                _pieces[i].transform.SetParent(transform);                          //Link to the parent
                _pieces[i].gameObject.SetActive(true);                              //Show
            }

            //Relocate the belt (and its pieces) to it's initial position 
            transform.position = _initPos.position;
        }
    }
}

//Nota:
    //Recordar que en el ontrigger, al hacer enqueue de las piezas hay que comprobar que ninguna ha sido usada sobre algún robot.
    //La comprobación se hace en función de un bit que se activa cuando una pieza es usada con un robot. (Como sugerencia)