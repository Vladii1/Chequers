using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chequer : MonoBehaviour {
    public int index;
    Vector3 offset;
    Vector3 screenPoint;

    public Vector3 moveStartPosition;
    
    public GameController gameController;

    public bool isWhite;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void OnMouseDown()
    {
        moveStartPosition = transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint);
        transform.position = currentPosition + offset;

    }

    void OnMouseUp()
    {

        //Vector3 currentPosition = RoundUpPositionToInt();
        //gameController.startPos = moveStartPosition;
        //gameController.dropPos = currentPosition;

        //Vector3 translation = CheckMovementDistance(currentPosition,moveStartPosition);



        transform.position = gameController.IsThisMovePossible(index, moveStartPosition);


    }

    //Vector3 RoundUpPositionToInt()
    //{
    //    int roundedX = Mathf.RoundToInt(transform.position.x);
    //    int roundedY =  Mathf.RoundToInt(transform.position.y);
    //    Vector3 returnPos = new Vector3(roundedX, roundedY, transform.position.z);
    //    return returnPos;
    //}

    //Vector3 CheckMovementDistance(Vector3 currentPosition, Vector3 startPosition)
    //{
    //    Vector3 translation = currentPosition - startPosition;
    //    return translation;
    //}

    //bool IsOneFieldForwardMovement (Vector3 translation) // trzeba będzie dodać sprawdzenie z którym graczem mamy doczynienia i odpowiednie dopasowanie warunku
    //{
    //        if (isWhite == true && translation.x == -1 && translation.y == 1 || 
    //            isWhite == true &&translation.x == 1 && translation.y == 1)
    //        {
    //            return true;
    //        }
    //        else if (isWhite == false && translation.x == -1 && translation.y == -1 ||
    //        isWhite == false && translation.x == 1 && translation.y == -1)
    //        {
    //            return true;
    //        }
    //        else return false;
    //}

    bool IsTwoFieldMovement (Vector3 translation) // trzeba będzie dodać sprawdzenie z którym graczem mamy doczynienia i odpowiednie dopasowanie warunku
    {
        if (translation.x == -2 && translation.x == -2 || translation.x == 2 && translation.y == 2)
        {
            return true;
        }
        else return false;
    }

    //bool IsPositionInGameField()
    //{
    //    if (transform.position.x > 0 && transform.position.x < gameController.fieldSize && transform.position.y > 0 && transform.position.y < gameController.fieldSize)
    //    {
    //        return true;
    //    }
    //    else return false;
    //}

}
