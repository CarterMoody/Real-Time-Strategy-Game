using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GameRTSController : MonoBehaviour
{
    [SerializeField] private Transform selectionAreaTransform;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private List<UnitRTS> selectedUnitRTSList;

    private void Awake()
    {
        selectedUnitRTSList = new List<UnitRTS>();
        selectionAreaTransform.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // Left Mouse Button Pressed
        if (Input.GetMouseButtonDown(0))
        {
            selectionAreaTransform.gameObject.SetActive(true);
            startPosition = UtilsClass.GetMouseWorldPosition();
        }

        // Left Mouse Button Held Down
        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = UtilsClass.GetMouseWorldPosition();
            Vector3 lowerLeft = new Vector3(
                Mathf.Min(startPosition.x, currentMousePosition.x),
                Mathf.Min(startPosition.y, currentMousePosition.y)
            );
            Vector3 upperRight = new Vector3(
                Mathf.Max(startPosition.x, currentMousePosition.x),
                Mathf.Max(startPosition.y, currentMousePosition.y)
            );
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;
        }
        // Left Mouse Button Released
        if (Input.GetMouseButtonUp(0))
        {
            selectionAreaTransform.gameObject.SetActive(false);
            endPosition = UtilsClass.GetMouseWorldPosition();
            //Debug.Log(endPosition + " " + startPosition);
            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, UtilsClass.GetMouseWorldPosition());
            
            // Deselect all Units
            foreach (UnitRTS unitRTS in selectedUnitRTSList)
            {
                unitRTS.SetSelectedVisible(false);
            }
            selectedUnitRTSList.Clear();

            // Select Units within Selection Area
            foreach (Collider2D collider2D in collider2DArray)
            {
                UnitRTS unitRTS = collider2D.GetComponent<UnitRTS>();
                if (unitRTS != null)
                {
                    unitRTS.SetSelectedVisible(true);
                    selectedUnitRTSList.Add(unitRTS);
                }
                //Debug.Log(collider2D);
            }

            //Debug.Log("Units Selected: " + selectedUnitRTSList.Count); 
        }

        // Right Mouse Button Pressed
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 moveToPosition = UtilsClass.GetMouseWorldPosition();

            
            //List<Vector3> targetPositionList = GetPositionListAround(moveToPosition, new float[] { 2f, 4f, 6f, 8f}, new int[] { 5, 10, 20, 40 }); // Create a Ring
            List<Vector3> targetPositionList = GetPositionListAround(moveToPosition, new float[] { 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f}, new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 });   // Create a Line HARD MAX 20 wil move at a time
                                                                                                                                                                                            // Should add a way to handle multiple lines...
            float spacing = 2f;
            float columns = 5;
            List<Vector3> targetPositionListLine = GetPositionListLine(moveToPosition, spacing, columns);

            Debug.Log("printing entire targetPositionList");
            for (int i = 0; i < targetPositionList.Count; i++)
            {
                Debug.Log("targetPositionList[" + i + "]: " + targetPositionList[i]);
            }
            int targetPositionListIndex = 0;
            foreach (UnitRTS unitRTS in selectedUnitRTSList)
            {
                Debug.Log("Count Selected Units: " + selectedUnitRTSList.Count);
                Debug.Log("Unit EXP: " + unitRTS.GetComponent<IUnit>().getEXP());

                Vector3 destination;
                int closestDestinationIndex = FindClosestDestinationIndex(unitRTS, targetPositionList, selectedUnitRTSList.Count);
                Vector3 closestDestination = targetPositionList[closestDestinationIndex];
                Debug.Log("closestDestination: " + closestDestination);
                Debug.Log("targetPositionList[" + targetPositionListIndex + "]: " + targetPositionList[targetPositionListIndex]);
                targetPositionList.RemoveAt(closestDestinationIndex);
                destination = closestDestination;

                //destination = targetPositionList[targetPositionListIndex];
                
                

                Debug.Log("Destination Coords: " + destination);
                DestinationParticleSystemHandler.Instance.SpawnDestinationParticle(destination);

                unitRTS.MoveTo(destination);    // Tell the unit to move to position in list
                
                targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
            }
        }
    }

    // Take in a specific unitRTS so we can calculate the nearest available destination for him to move to
    // Also take in a list of all possible destinations (Includes ALL TOTAL destinations... even more than units selected)
    // Use the countSelectedUnits so that we send him to the first few destinations generated
    //      If countSelectedUnits is not used, then the unit will go to a closer spot for 20th unit or another one.
    private int FindClosestDestinationIndex(UnitRTS unitRTS, List<Vector3> targetPositionList, int countSelectedUnits)
    {
        Vector3 currentPosition = unitRTS.transform.position;
        Vector3 smallestDistance = targetPositionList[0] - currentPosition;     // Get distance to the first possible destination and set as minimum
        int smallestDistanceIndex = 0;
        for (int i = 0; (i < targetPositionList.Count && i < countSelectedUnits) ; i++)
        {
            Vector3 travelDistance = targetPositionList[i] - currentPosition;
            if (travelDistance.magnitude <= smallestDistance.magnitude)
            {
                smallestDistance = travelDistance;
                smallestDistanceIndex = i;
            }
        }
        return smallestDistanceIndex;
    }

    // ringDistanceArray is the distance you want in between target positions. Larger numbers means more widespread. Smaller is more concentrated line of positions
    // ringPositionCount is the amount of target positions (columns) in a given line (row). Larger numbers means more concentrated. Smaller is more widespread, fewer total positions (columns) per line (row).
    private List<Vector3> GetPositionListLine(Vector3 startPosition, float spacing, float columns)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);


        return positionList;
    }  

  
    

    // ringDistanceArray is the distance you want in between target positions. Larger numbers means more widespread. Smaller is more concentrated ring of positions
    // ringPositionCount is the amount of target positions in a given ring. Larger numbers means more concentrated. Smaller is more widespread, fewer total positions per ring.
    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
    {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360f / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPosition + dir * distance;

            // Enforce Minimums
            Vector3 lowerBound = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0));
            Vector3 upperBound = Camera.main.ViewportToWorldPoint(new Vector3(1,1,0));
            float boundaryPadding = 0.5f;
            //Debug.Log("lowerBound: " + lowerBound + "upperBound: " + upperBound);
            float xPos = Mathf.Clamp(position.x, (lowerBound.x + boundaryPadding), (upperBound.x - boundaryPadding));
            float yPos = Mathf.Clamp(position.y, (lowerBound.y + boundaryPadding), (upperBound.y - boundaryPadding));
            position.x = xPos;
            position.y = yPos;
            //position.y = startPosition.y;

            positionList.Add(position);
        }
        return positionList;
    }

    private Vector3 ApplyRotationToVector(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vec;
    }
}
