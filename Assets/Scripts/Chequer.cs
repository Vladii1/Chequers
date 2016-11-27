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
        transform.position = gameController.IsThisMovePossible(index, moveStartPosition, isWhite);
    }

   

}
