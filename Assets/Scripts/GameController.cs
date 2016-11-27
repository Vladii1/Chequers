using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public int fieldSize {get; private set;}
    int fieldsArrayPosX = 0;
    int fieldsArrayPosY = 0;

    public GameObject fieldPrefab;
    GameObject prefabInstantiated;
    Field fieldInstance;

    bool startWithActive;

    public Material fieldActiveMaterial;
    public Material fieldPassiveMaterial;

    public Field[,] fields { get; private set; }

    public GameObject chequerPrefab;

    public Material chequerWhiteMaterial;
    public Material chequerBlackMaterial;

    public Chequer[] chequers { get; private set; }
    int numberOfChequers = 24;
    Chequer chequerInstance;
    int chequersCount =0;

    Chequer movingChequer;
    Vector3 chequerRoundedPosition;
    Vector3 chequerTranslation;


    // Use this for initialization
    void Start()
    {
        fieldSize = 8;
        fields = new Field[fieldSize, fieldSize];
        chequers = new Chequer[numberOfChequers];
        
            for (int ii = 0; ii < fieldSize; ii++)
            {
                for (int iii = 0; iii < fieldSize / 2; iii++)
                {
                    if (startWithActive == true)
                    {
                        fieldInstance = AddToFieldsArray(InstantiateAndSet(fieldPrefab,fieldsArrayPosX, fieldsArrayPosY, fieldActiveMaterial));
                        fieldInstance.isActive = true;
                        fieldsArrayPosX++;
                        fieldInstance = AddToFieldsArray(InstantiateAndSet(fieldPrefab,fieldsArrayPosX, fieldsArrayPosY, fieldPassiveMaterial));
                        fieldInstance.isActive = false;
                        fieldsArrayPosX++;
                    }
                    else if (startWithActive == false)
                    {
                        fieldInstance = AddToFieldsArray(InstantiateAndSet(fieldPrefab,fieldsArrayPosX, fieldsArrayPosY, fieldPassiveMaterial));
                        fieldInstance.isActive = false;
                        fieldsArrayPosX++;
                        fieldInstance = AddToFieldsArray(InstantiateAndSet(fieldPrefab,fieldsArrayPosX, fieldsArrayPosY, fieldActiveMaterial));
                        fieldInstance.isActive = true;
                        fieldsArrayPosX++;
                    }
                }
                startWithActive = !startWithActive;
                fieldsArrayPosX = 0;
                fieldsArrayPosY++;
            }
        foreach (Field field in fields)
        {
            if (field.isActive == true && field.transform.position.y <= 2)
            {
                chequerInstance = AddToChequersArray(InstantiateAndSet(chequerPrefab, 
                Mathf.RoundToInt(field.transform.position.x), Mathf.RoundToInt(field.transform.position.y), chequerWhiteMaterial));
                chequerInstance.isWhite = true;
                SetFieldProperties(field, chequerInstance.isWhite);
            }
            else if (field.isActive == true && field.transform.position.y >= 5)
            {
                chequerInstance = AddToChequersArray(InstantiateAndSet(chequerPrefab,
                Mathf.RoundToInt(field.transform.position.x), Mathf.RoundToInt(field.transform.position.y), chequerBlackMaterial));
                chequerInstance.isWhite = false;
                SetFieldProperties(field, chequerInstance.isWhite);
            }
        }
    }
	
    GameObject InstantiateAndSet(GameObject prefabToInstantiate,int posX,int posY, Material material)
    {
        Vector3 spawnPos = new Vector3(posX, posY, 0);
        prefabInstantiated = Instantiate(prefabToInstantiate,spawnPos,Quaternion.identity);
        prefabInstantiated.GetComponent<SpriteRenderer>().material = material;
        return prefabInstantiated;
    }
    Field AddToFieldsArray(GameObject fieldToAdd)
    {
        fieldInstance = fieldToAdd.GetComponent<Field>();
        fields[fieldsArrayPosX, fieldsArrayPosY] = fieldInstance;
        return fieldInstance;
    }
    Chequer AddToChequersArray (GameObject chequerToAdd)
    {
        chequerInstance = chequerToAdd.GetComponent<Chequer>();
        chequers[chequersCount] = chequerInstance;
        chequerInstance.index = chequersCount;
        chequersCount++;
        return chequerInstance;
    }
    void SetFieldProperties(Field field, bool hasWhite)
    {
        field.isFull = true;
        field.hasWhite = hasWhite;
    }





    public Vector3 IsThisMovePossible(int chequerIndex, Vector3 startPos, bool isWhite)
    {

        movingChequer = FindMovingChequer(chequerIndex);

        chequerRoundedPosition = RoundUpPositionToInt(movingChequer.gameObject.transform.position);
        //print(chequerRoundedPosition);
        int chequerIntPosX = Mathf.RoundToInt(chequerRoundedPosition.x);
        int chequerIntPosY = Mathf.RoundToInt(chequerRoundedPosition.y);

        print(fields.GetLength(0) + " " + fields.GetLength(1));

        if (chequerIntPosX <= fields.GetLength(0) -1  && chequerIntPosY <= fields.GetLength(1) -1 && chequerIntPosX >= 0 && chequerIntPosY >= 0)
        {
            print("tutaj");
            Field targetField = fields[chequerIntPosX, chequerIntPosY];
            if (targetField.isActive == true && targetField.isFull == false)
            {
                chequerTranslation = CheckMovementDistance(chequerRoundedPosition, startPos);
                if (IsOneFieldForwardMovement(chequerTranslation, movingChequer) == true) return chequerRoundedPosition;
                else if (IsTwoFieldForwardMovement(chequerTranslation, movingChequer) == true)
                {
                    if (IsOposingChequerOnTheWay(startPos, chequerRoundedPosition, movingChequer) == true) return chequerRoundedPosition;
                    else return startPos;
                }
                else return startPos;
            }
            else return startPos;
        }
        else return startPos;

        //Field startField = fields[Mathf.RoundToInt(startPos.x), Mathf.RoundToInt(startPos.y)];
    }



    Chequer FindMovingChequer(int chequerIndex)
    {
        Chequer movingChequer = chequers[chequerIndex];
        return movingChequer;
    }

    Vector3 RoundUpPositionToInt(Vector3 chequerPos)
    {
        int roundedX = Mathf.RoundToInt(chequerPos.x);
        int roundedY = Mathf.RoundToInt(chequerPos.y);
        Vector3 returnPos = new Vector3(roundedX, roundedY, chequerPos.z);
        return returnPos;
    }
    
    
    bool IsChequerPositionInGameField(Vector3 chequerPos)
    {
        if (chequerPos.x >= 0 && chequerPos.x <= fieldSize && chequerPos.y >= 0 && chequerPos.y <= fieldSize)
        {
            return true;
        }
        else return false;
    }
    Vector3 CheckMovementDistance(Vector3 currentPosition, Vector3 startPosition)
    {
        Vector3 translation = currentPosition - startPosition;
        return translation;
    }
    bool IsOneFieldForwardMovement(Vector3 translation, Chequer movingChequer) // trzeba będzie dodać sprawdzenie z którym graczem mamy doczynienia i odpowiednie dopasowanie warunku
    {
        if (movingChequer.isWhite == true && translation.x == -1 && translation.y == 1 ||
            movingChequer.isWhite == true && translation.x == 1 && translation.y == 1)
        {
            return true;
        }
        else if (movingChequer.isWhite == false && translation.x == -1 && translation.y == -1 ||
        movingChequer.isWhite == false && translation.x == 1 && translation.y == -1)
        {
            return true;
        }
        else return false;
         
        
    }
    bool IsTwoFieldForwardMovement(Vector3 translation, Chequer movingChequer) // trzeba będzie dodać sprawdzenie z którym graczem mamy doczynienia i odpowiednie dopasowanie warunku
    {
        if (movingChequer.isWhite == true && translation.x == -2 && translation.y == 2 ||
            movingChequer.isWhite == true && translation.x == 2 && translation.y == 2)
        {
            return true;
        }
        else if (movingChequer.isWhite == false && translation.x == -2 && translation.y == -2 ||
        movingChequer.isWhite == false && translation.x == 2 && translation.y == -2)
        {
            return true;
        }
        else return false;
    }
    bool IsOposingChequerOnTheWay(Vector3 startPos, Vector3 targetPos, Chequer movingChequer)
    {
        float betweenPosX = (startPos.x + targetPos.x)/2;
        float betweenPosY = (startPos.y + targetPos.y)/2;
        print(betweenPosX + ", " + betweenPosY);
        Field betweenField = fields[Mathf.RoundToInt(betweenPosX), Mathf.RoundToInt(betweenPosY)];
        //Destroy(betweenField);
        if (betweenField.hasWhite == true && movingChequer.isWhite == false) return true;
        else if (betweenField.hasWhite == false && movingChequer.isWhite == true) return true;
        else return false;
        //print("between.hasWhite" + betweenField.hasWhite);
    }
}
