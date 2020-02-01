using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    //General info
    [SerializeField] public static int maxPieces = 4;

    //Pieces' variables
    private int _numPieces;
    private int _piecesTypeMask;                    //Stores a 1 in the 1st position if the leg_l is needed, 1 in the 2nd if leg_r is needed...
                                        //There can (and surely will) be needed both of them --> 00011.

    //Container
    private Piece[] _pieces;        //Stores the characteristics of the pieces the robot will need in order to be repaired.
    private int[] _piecesTypes;
    
    //Reference to a generic piece
    [SerializeField] public GameObject _piecePrefab;

    public void Intialize()
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
            }
        }
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
        int[] types = new int[5];
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
                case 4:
                    mask += 16;      //10000
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
}
