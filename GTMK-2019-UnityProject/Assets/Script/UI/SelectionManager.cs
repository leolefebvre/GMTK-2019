using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    public BoxCollider2D DetectionCollider;
    public int colliderCount;

    [Header("SelectionSquare")]
    private Vector3 selectionStartPos;
    private Vector3 selectionEndPos;

    [Header("Timing")]
    public float timeBeforeSelectorActivation = 0.1f;

    public float timeOnStartClick;
    
    public List<PieceController> selectedPieces;

    public bool isHoldingDown = false;

    public bool isSelectorActive
    {
        get { return SelectionSquare.Instance.isSelectorActive; }
    }
    


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelection();
    }

    public void UpdateSelection()
    {
        //mouse Click

        if (Input.GetMouseButtonDown(0))
        {
            timeOnStartClick = Time.time;
            selectionStartPos = Input.mousePosition;
        }

        //Release the mouse button
        if (Input.GetMouseButtonUp(0))
        {
            SelectionSquare.Instance.HideSelector();
            isHoldingDown = false;
        }

        //Holding down the mouse button
        if (Input.GetMouseButton(0))
        {
            if (Time.time - timeOnStartClick > timeBeforeSelectorActivation)
            {
                isHoldingDown = true;
            }
        }

        //  Draw visual selector and update collider
        if (isHoldingDown)
        {
            selectionEndPos = Input.mousePosition;
            SelectionSquare.Instance.DrawSelector(selectionStartPos, selectionEndPos);
            UpdateDetectionCollider();
        }
    }


    public void UpdateDetectionCollider()
    {
        Vector3 startPosWorld, endPosWorld;
        startPosWorld = Camera.main.ScreenToWorldPoint(selectionStartPos);
        endPosWorld = Camera.main.ScreenToWorldPoint(selectionEndPos);

        //move the colider to fit the selection in the world
        //Get the middle position of the square
        Vector2 middle = new Vector2((startPosWorld.x + endPosWorld.x) / 2f, (startPosWorld.y + endPosWorld.y) / 2f);

        //Set the middle position of the GUI square
        DetectionCollider.offset = middle;

        //Change the size of the square
        float sizeX = Mathf.Abs(startPosWorld.x - endPosWorld.x);
        float sizeY = Mathf.Abs(startPosWorld.y - endPosWorld.y);

        //Set the size of the square
        DetectionCollider.size = new Vector2(sizeX, sizeY);
    }

    public void AddToSelectedList(PieceController pieceToAdd)
    {
        if(selectedPieces.Contains(pieceToAdd))
        {
            return;
        }

        selectedPieces.Add(pieceToAdd);

        // if this is the first piece selected, set border color to this piece
        if(selectedPieces.Count == 1)
        {
            SelectionSquare.Instance.UpdateSelectionColor(pieceToAdd.currentColor);
        }
    }

    public void RemoveToSelectedList(PieceController pieceToRemove)
    {
        if (selectedPieces.Contains(pieceToRemove))
        {
            selectedPieces.Remove(pieceToRemove);
        }

        // if there is nothing selected, back to default color
        if (selectedPieces.Count == 0)
        {
            SelectionSquare.Instance.SetSelectionColorToDefault();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hey!");
    }

}