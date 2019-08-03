using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectionSquare : Singleton<SelectionSquare>
{
    public RectTransform selectionRectTransfrom;

    public Color defaultSelectorColor { get; private set; } 

    private Image _selectionImage;
    public Image selectionImage
    {
        get
        {
            if (_selectionImage == null)
            {
                _selectionImage = selectionRectTransfrom.GetComponent<Image>();
            }
            return _selectionImage;
        }
    }

    public bool isSelectorActive
    {
        get { return selectionRectTransfrom.gameObject.activeSelf; }
    }

    // Use this for initialization
    void Start()
    {
        HideSelector();
        defaultSelectorColor = selectionImage.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DrawSelector(Vector3 selectionStartPos, Vector3 selectionEndPos)
    {
        if (!isSelectorActive)
        {
            DisplaySelector();
        }

        //Get the middle position of the square
        Vector3 middle = (selectionStartPos + selectionEndPos) / 2f;

        //Set the middle position of the GUI square
        selectionRectTransfrom.position = middle;

        //Change the size of the square
        float sizeX = Mathf.Abs(selectionStartPos.x - selectionEndPos.x);
        float sizeY = Mathf.Abs(selectionStartPos.y - selectionEndPos.y);

        //Set the size of the square
        selectionRectTransfrom.sizeDelta = new Vector2(sizeX, sizeY);
    }

    public void DisplaySelector()
    {
        selectionRectTransfrom.gameObject.SetActive(true);
    }

    public void HideSelector()
    {
        selectionRectTransfrom.gameObject.SetActive(false);
    }

    public void SetSelectionColorToDefault()
    {
        UpdateSelectionColor(defaultSelectorColor);
    }

    public void UpdateSelectionColor(Color newColor)
    {
        selectionImage.color = newColor;
    }
}
