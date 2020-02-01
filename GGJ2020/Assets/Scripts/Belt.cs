﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : MonoBehaviour
{
    //Belt properties
    public float _speed;
    const int nPiecesPerBelt = 3;
    public Transform _otherBelt;

    //Pieces variables
    public Transform _initPos;
    public float _initOffset;

    //Containers
    private Piece[] _pieces;

    //References
    private Level _level;

    private void Start()
    {
        //Find level reference
        _level = GameObject.FindGameObjectWithTag("LevelManager")?.GetComponent<Level>();

        //Initialize data structures
        _pieces = new Piece[3];

        //Pick the first 3 pieces.
        for (int i = 0; i < nPiecesPerBelt; i++)
        {
            Piece p = _level.GetPiecesQueue().Dequeue();
            _pieces[i] = p;
            _pieces[i].transform.position = _initPos.position + new Vector3(0, 0, _initOffset * i); //Set initial pos.
                                                                                                   
            _pieces[i].transform.SetParent(transform);
            _pieces[i].gameObject.SetActive(true);
        }

    }

    void FixedUpdate()
    {
        transform.position += Vector3.right * _speed * Time.deltaTime;
    }

    public void Refresh() //Llamado desde el trigger
    {
        Piece tempPiece;

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
        transform.position = _initPos.position;
    }
}

//Nota:
    //Recordar que en el ontrigger, al hacer enqueue de las piezas hay que comprobar que ninguna ha sido usada sobre algún robot.
    //La comprobación se hace en función de un bit que se activa cuando una pieza es usada con un robot. (Como sugerencia)