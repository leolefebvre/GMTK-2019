using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : Singleton<SelectionManager>
{
    public bool useCorrectionHandling = true;
    public BoxCollider2D detectionCollider;
    public int colliderCount;

    [Header("SelectionSquare")]
    public Vector3 selectionStartPos;
    private Vector3 selectionEndPos;

    [Header("Timing")]
    public float timeBeforeSelectorActivation = 0.1f;

    public float timeOnStartClick;
    
    public List<PieceController> selectedPieces;

    public List<PieceController> piecesThatWereSelectedAtTheSameTime;
    public List<float> closenessList;
    public List<PieceController> sortedPieceList;

    public List<PieceController> beforeReordering;

    public bool isHoldingDown = false;

    public bool isSelectorActive
    {
        get { return SelectionSquare.Instance.isSelectorActive; }
    }
    public bool isDetectionColliderActive
    {
        get { return detectionCollider.gameObject.activeSelf; }
    }

    private float timeLastPieceAdded = 0f;

    // Start is called before the first frame update
    void Start()
    {
        DeactivateDetectionColider();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelection();

        if(Input.GetKeyDown("b"))
        {
            Debug.Break();
        }
    }

    public void UpdateSelection()
    {
        //mouse Click

        if (Input.GetMouseButtonDown(0))
        {
            //check if the mouse is over a UI element before clicking, if so GTFO
            if(EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            HandleOnClick();
        }

        //Release the mouse button
        if (Input.GetMouseButtonUp(0))
        {
            HandleRelease();
        }

        /*

        //Holding down the mouse button
        if (Input.GetMouseButton(0))
        {
            if (Time.time - timeOnStartClick > timeBeforeSelectorActivation)
            {
                isHoldingDown = true;
            }
        } */

        //  Draw visual selector and update collider
        if (isHoldingDown)
        {
            selectionEndPos = Input.mousePosition;
            SelectionSquare.Instance.DrawSelector(selectionStartPos, selectionEndPos);
            UpdateDetectionCollider();
        }
    }

    public void HandleOnClick()
    {
        timeOnStartClick = Time.time;
        selectionStartPos = Input.mousePosition;
        isHoldingDown = true;
        GameOverseer.Instance.resetButton.interactable = false;
        GameOverseer.Instance.DisableQuitButton();
    }

    public void HandleRelease()
    {
        SelectionSquare.Instance.HideSelector();
        isHoldingDown = false;

        if (selectedPieces.Count > 1)
        {
            SelectionReleaseManager.Instance.HandleRelease(new List<PieceController>(selectedPieces), selectedPieces[0].currentColor);
        }
        
        DeactivateDetectionColider();
        RemoveAllSelectedInList();
        GameOverseer.Instance.resetButton.interactable = true;
        GameOverseer.Instance.EnableQuitButton();
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
        detectionCollider.offset = middle;

        //Change the size of the square
        float sizeX = Mathf.Abs(startPosWorld.x - endPosWorld.x);
        float sizeY = Mathf.Abs(startPosWorld.y - endPosWorld.y);

        //Set the size of the square
        detectionCollider.size = new Vector2(sizeX, sizeY);
        
        // activate the collider after changing it's shape if it's not active
        if (!isDetectionColliderActive)
        {
            ActivateDetectionColider();
        }
    }
    
    public void AddToSelectedList(PieceController pieceToAdd)
    {
        if(selectedPieces.Contains(pieceToAdd))
        {
            return;
        }

        if(timeLastPieceAdded == Time.time)
        {
            //it means that two pieces were selected at the same time, in this case we check which one is in the front and keep this one
            Debug.Log("Two at the same time");
        }

        selectedPieces.Add(pieceToAdd);
        pieceToAdd.selectionOrder = selectedPieces.Count - 1;

        // if this is the first piece selected, set border color to this piece
        if(selectedPieces.Count == 1)
        {
            SelectionSquare.Instance.UpdateSelectionColor(pieceToAdd.currentColor);
        }
        else
        {
            if(useCorrectionHandling)
            {
                SelectionCorrectionHandling(pieceToAdd);
            }
        }

        SoundManager.Instance.PlaySelectionSound(selectedPieces.Count - 1, true);
    }

    public void RemoveToSelectedList(PieceController pieceToRemove)
    {
        if (selectedPieces.Contains(pieceToRemove))
        {
            pieceToRemove.PlaySelectAnimation();
            SoundManager.Instance.PlaySelectionSound(selectedPieces.Count - 1, true);

            //pieceToRemove.selectionOrder = int.MaxValue;

            selectedPieces.Remove(pieceToRemove);
        }

        // we update the color in case the first selection changed, or in case there is no more piece to update.
        UpdateSelectionColor();
    }

    public void UpdateSelectionColor()
    {
        if (selectedPieces.Count == 0)
        {
            SelectionSquare.Instance.SetSelectionColorToDefault();
        }
        else
        {
            SelectionSquare.Instance.UpdateSelectionColor(selectedPieces[0].currentColor);
        }
    }

    public void SelectionCorrectionHandling(PieceController currentPiece)
    {
        if(selectedPieces.Count <= 1)
        {
            // no check to be done here
            return;
        }
        piecesThatWereSelectedAtTheSameTime.Clear();

        piecesThatWereSelectedAtTheSameTime = selectedPieces.FindAll(x => x.timeOnSelect == currentPiece.timeOnSelect);

        if(piecesThatWereSelectedAtTheSameTime.Count <= 1)
        {
            return;
        }

        //Reorder by closeness to the origin point;
        Vector3 startPosWorld;
        startPosWorld = Camera.main.ScreenToWorldPoint(selectionStartPos);
        closenessList.Clear();

        foreach (PieceController piece in piecesThatWereSelectedAtTheSameTime)
        {
            closenessList.Add(Vector3.Distance(piece.pieceCollider.bounds.ClosestPoint(startPosWorld), startPosWorld));
        }

        sortedPieceList = new List<PieceController>(piecesThatWereSelectedAtTheSameTime);
        sortedPieceList.Sort((x, y) => x.distanceFromStartSelection.CompareTo(y.distanceFromStartSelection));


        // add new order to affected pieces
        for (int i = 0; i < piecesThatWereSelectedAtTheSameTime.Count; i++)
        {
            piecesThatWereSelectedAtTheSameTime[i].newSelectionOrder = sortedPieceList[i].selectionOrder;
        }

        // update selected pieces selectionOrder
        foreach (PieceController piece in piecesThatWereSelectedAtTheSameTime)
        {
            piece.selectionOrder = piece.newSelectionOrder;
        }

        beforeReordering = new List<PieceController>(selectedPieces);

        // reorder the selection list
        Debug.Log("Reordering " + listToString(selectedPieces));

        selectedPieces.Sort((x, y) => x.selectionOrder.CompareTo(y.selectionOrder));
        UpdateSelectionColor();
    }

    public string listToString(List<PieceController> pieceControllers)
    {
        string returnString = "";

        foreach (PieceController piece in pieceControllers)
        {
            returnString += piece.name + " - ";
        }

        return returnString;
    }

    public void RemoveAllSelectedInList()
    {
        selectedPieces.Clear();
    }

    void DeactivateDetectionColider()
    {
        detectionCollider.gameObject.SetActive(false);
    }

    void ActivateDetectionColider()
    {
        detectionCollider.gameObject.SetActive(true);
    }
}