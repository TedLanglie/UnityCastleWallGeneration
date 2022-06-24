using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWalls : MonoBehaviour
{
    [Header("Wall Prefabs")]
    [SerializeField] GameObject wallTopPrefab;
    [SerializeField] GameObject wallBottomPrefab;
    [SerializeField] GameObject wallMidPrefab;
    [Header("Gate Prefabs")]
    [SerializeField] GameObject gateTopPrefab;
    [SerializeField] GameObject gateBottomPrefab;
    [SerializeField] GameObject gateMidPrefab;
    [Header("Corner Prefabs")]
    [SerializeField] GameObject cornerTopPrefab;
    [SerializeField] GameObject cornerBottomPrefab;
    [SerializeField] GameObject cornerMidPrefab;
    [Header("Calculators")]
    const float heightDifference = 4; // height differential between prefabs
    const float widthDifference = 4; // width differential between prefabs
    [Header("Input Parameters")]
    private int _height = 1; // default
    private int _size = 3; // default

    void Start()
    {
        Vector3 inputPosition = this.transform.position; // can change this to make it spawn somewhere else
        // on Start create a default castle
        GenerateCaste(_height, _size, inputPosition);
    }

    public void changeHeight(int amount)
    {
        _height+=amount;
        // validation
        if(_height < 1)
        {
            _height = 1;
        }

        Regenerate();
    }

    public void changeSize(int amount)
    {
        _size+=amount;
        // validation
        if(_size < 1)
        {
            _size = 1;
        }

        Regenerate();
    }

    void Regenerate()
    {
        // destroy all objs with "TAG"
        ClearCastle();
        // generate castle
        Vector3 inputPosition = this.transform.position; // can change this to make it spawn somewhere else
        GenerateCaste(_height, _size, inputPosition);
    }

    void ClearCastle()
    {
        // Find all objects with tag "CastlePart" and put them into array.
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("CastlePart"); 
        // Loop through array and destroy all of the objects  
        foreach (GameObject castlePart in taggedObjects) {
	    Destroy(castlePart);
        }
    }

    void GenerateCaste(int height, int size, Vector3 startingPosition)
    {
        // == POINTER VARIABLES TO ITERATE THROUGH PLACEMENTS
        Vector3 nextPositionOfPlacement = startingPosition;
        Quaternion nextRotationOfPlacement = Quaternion.identity;

        // Generate gate at given position
        GenerateGate(0, startingPosition);
        // Build gate walls given [size] beside gate
        GenerateGateWalls(0);

        // BUILD REST OF CASTLE
        GenerateCorner('x', true);
        GenerateWall('z', false, 90);
        GenerateCorner('z', false);
        GenerateWall('x', false, 180);
        GenerateCorner('x', false);
        GenerateWall('z', true, 270);
        GenerateCorner('z', true);

        /*
            | ----
            |
                == MAIN HELPER FUNCTIONS
                vvvvvvvvvvvvvvvvvvvvvvvv
            |
            |
        */

        /*
        |  Parameters:
            -axis : either 'x' or 'z'. dictates which axis it moves along
            -isPositive : decides if its moving positive or negative along the axis

        | Purpose:
            Generate a corner piece of a wall using the parameters given, at the next position of placement
        */
        void GenerateCorner(char axis, bool isPositive)
        {
            nextPositionOfPlacement.y = 0; // reset height
            // move over based on parameters
            if(axis == 'x')
            {
                if(isPositive == true) nextPositionOfPlacement.x += widthDifference; // move over
                else nextPositionOfPlacement.x -= widthDifference; // move over
            } else {
                if(isPositive == true) nextPositionOfPlacement.z += widthDifference; // move over
                else nextPositionOfPlacement.z -= widthDifference; // move over
            }
            Instantiate(cornerBottomPrefab, nextPositionOfPlacement, nextRotationOfPlacement);
            for(int i = 1; i <= height; i++)
            {
                nextPositionOfPlacement.y += heightDifference;
                Instantiate(cornerMidPrefab, nextPositionOfPlacement, nextRotationOfPlacement);
            }
            // spawn top of corner
            nextPositionOfPlacement.y += heightDifference;
            Instantiate(cornerTopPrefab, nextPositionOfPlacement, nextRotationOfPlacement);
        }

        /*
        |  Parameters:
            -axis : either 'x' or 'z'. dictates which axis it moves along
            -isPositive : decides if its moving positive or negative along the axis
            -rotationDegree: decides how much to rotate the wall along its Y axis

        | Purpose:
            Generate a wall piece of a wall using the parameters given, at the next position of placement
        */
        void GenerateWall(char axis, bool isPositive, int rotationDegree)
        {
            // set rotation
            nextRotationOfPlacement = Quaternion.Euler(0, rotationDegree, 0);
            // build [size] amount of walls
            for(int i = 1; i < size + 1; i++)
            {
                nextPositionOfPlacement.y = 0; // reset height
                if(axis == 'x')
                {
                    if(isPositive == true) nextPositionOfPlacement.x += widthDifference; // move over
                    else nextPositionOfPlacement.x -= widthDifference; // move over
                } else {
                    if(isPositive == true) nextPositionOfPlacement.z += widthDifference; // move over
                    else nextPositionOfPlacement.z -= widthDifference; // move over
                }

                Instantiate(wallBottomPrefab, nextPositionOfPlacement, nextRotationOfPlacement); // spawn bottom
                // spawn middle
                for(int x = 1; x <= height; x++)
                {
                    nextPositionOfPlacement.y += heightDifference;
                    Instantiate(wallMidPrefab, nextPositionOfPlacement, nextRotationOfPlacement);
                }
                // spawn top of wall
                nextPositionOfPlacement.y += heightDifference;
                Instantiate(wallTopPrefab, nextPositionOfPlacement, nextRotationOfPlacement);
            }
        }

        /*
        |  Parameters:
            -rotationDegree: decides how much to rotate the gate along its Y axis
            -position: decides where the gate is placed

        | Purpose:
            Generate a gate piece using the parameters given, at a given position
        */
        void GenerateGate(int rotationDegree, Vector3 position)
        {
            // set Rotation
            nextRotationOfPlacement = Quaternion.Euler(0, rotationDegree, 0);
            // spawn gate at starting position
            Instantiate(gateBottomPrefab, position, nextRotationOfPlacement);
            // loop through middle walls of gate
            for(int i = 1; i <= height; i++)
            {
                position.y += heightDifference;
                Instantiate(gateMidPrefab, position, nextRotationOfPlacement);
            }
            // spawn top of gate
            position.y += heightDifference;
            Instantiate(gateTopPrefab, position, nextRotationOfPlacement);
        }
        
        /*
        |  Parameters:
            -rotationDegree: decides how much to rotate the wall along its Y axis

        | Purpose:
            Generate walls beside the gate.
        */
        void GenerateGateWalls(int rotationDegree)
        {
            nextRotationOfPlacement = Quaternion.Euler(0, rotationDegree, 0);
            // finish side of gate wall
            for(int i = 1; i < size/2 + 1; i++)
            {
                nextPositionOfPlacement.y = 0; // reset height
                nextPositionOfPlacement.x -= widthDifference; // move over
                Instantiate(wallBottomPrefab, nextPositionOfPlacement, nextRotationOfPlacement); // spawn bottom piece
                // spawn middle
                for(int x = 1; x <= height; x++)
                {
                    nextPositionOfPlacement.y += heightDifference;
                    Instantiate(wallMidPrefab, nextPositionOfPlacement, nextRotationOfPlacement);
                }
                // spawn top of wall
                nextPositionOfPlacement.y += heightDifference;
                Instantiate(wallTopPrefab, nextPositionOfPlacement, nextRotationOfPlacement);
            }
            nextPositionOfPlacement.x = startingPosition.x; // reset back to middle
            for(int i = 1; i < size/2 + 1; i++)
            {
                nextPositionOfPlacement.y = 0; // reset height
                nextPositionOfPlacement.x += widthDifference; // move over
                Instantiate(wallBottomPrefab, nextPositionOfPlacement, nextRotationOfPlacement); // spawn bottom piece
                // spawn middle piece
                for(int x = 1; x <= height; x++)
                {
                    nextPositionOfPlacement.y += heightDifference;
                    Instantiate(wallMidPrefab, nextPositionOfPlacement, nextRotationOfPlacement);
                }
                // spawn top of wall
                nextPositionOfPlacement.y += heightDifference;
                Instantiate(wallTopPrefab, nextPositionOfPlacement, nextRotationOfPlacement);
            }
        }
    }
}
