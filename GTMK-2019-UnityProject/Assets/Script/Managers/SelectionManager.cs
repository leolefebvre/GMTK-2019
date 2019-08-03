using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    public BoxCollider2D detectionCollider;
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
            HandleRelease();
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

        // if this is the first piece selected, set border color to this piece
        if(selectedPieces.Count == 1)
        {
            SelectionSquare.Instance.UpdateSelectionColor(pieceToAdd.currentColor);
        }

        SoundManager.Instance.PlaySelectionSound(selectedPieces.Count - 1);
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