using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField] public static int maxPieces = 4;
    public int _numPieces;
    public GameObject[] _pieces;

    [SerializeField] public GameObject _piecePrefab;

    private void Start()
    {
        _numPieces = Random.Range(1, maxPieces);
        _pieces = new GameObject[_numPieces];

        for(int i=0; i < _numPieces; i++)
        {
            _pieces[i] = new GameObject();
            _pieces[i].AddComponent<Piece>();
            Debug.Log("Belt instantiated");
        }
    }
}
