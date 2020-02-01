﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : MonoBehaviour
{
    //Belt properties
    public float _speed;
    int nPiecesPerBelt = 3;
    public Transform _otherBelt;

    //Pieces variables
    public Transform _initPos;
    public float _initOffset;

    //Containers
    private Piece[] _pieces;
    public GameObject [] Spawners;

    //References
    public Level _level;

    private void Start()
    {      
        //Find level reference
        //_level = GameObject.FindGameObjectWithTag("LevelManager")?.GetComponent<Level>();

        //Initialize data structures
        nPiecesPerBelt = Spawners.Length;
        _pieces = new Piece[nPiecesPerBelt];

        //Pick the first 3 pieces.
        GetNew();
    }

    void FixedUpdate()
    {
        transform.position += Vector3.right * _speed * Time.fixedDeltaTime;
    }

    public void Refresh() //Llamado desde el trigger
    {
        /*Piece tempPiece;

        for (int i = 0; i < nPiecesPerBelt; i++)
        {
            //Hide pieces
            _pieces[i].gameObject.SetActive(false);

            //Store it into the Level queue (or not)
            //Yet to be implemented. For the moment all the pieces reentry the queue.
            _level.GetPiecesQueue().Enqueue(_pieces[i]);

            //Pick new piece
            tempPiece = _pieces[i];
            _pieces[i] = _level.GetPiecesQueue().Dequeue();

            _pieces[i].transform.position = tempPiece.transform.position;       //Locate
            _pieces[i].transform.SetParent(transform);                          //Link to the parent
            _pieces[i].gameObject.SetActive(true);                              //Show
        }

        //Relocate the belt (and its pieces) to it's initial position 
        transform.position = _initPos.position;*/

        for (int i = 0; i < nPiecesPerBelt; i++)
        {
            _pieces[i].gameObject.SetActive(false);
            _level.GetPiecesQueue().Enqueue(_pieces[i]);
        }
        GetNew();
    }

    public void GetNew()
    {
        for (int i = 0; i < nPiecesPerBelt; i++)
        {
            _pieces[i] = _level.GetPiecesQueue().Dequeue(); //ERROR DE EJECUCION
            _pieces[i].gameObject.SetActive(true);
            _pieces[i].gameObject.transform.position = Spawners[i].transform.position;
            _pieces[i].transform.SetParent(Spawners[i].transform);
            _pieces[i].gameObject.SetActive(true);
        }
    }
}

//Nota:
    //Recordar que en el ontrigger, al hacer enqueue de las piezas hay que comprobar que ninguna ha sido usada sobre algún robot.
    //La comprobación se hace en función de un bit que se activa cuando una pieza es usada con un robot. (Como sugerencia)