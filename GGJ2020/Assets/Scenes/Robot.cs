using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public static int maxPieces;
    public int _numPieces;
    public Belt[] _pieces;

    private void Start()
    {
        _numPieces = Random.Range(1, maxPieces);
        _pieces = new Belt[_numPieces];
    }
}
