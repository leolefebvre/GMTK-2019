using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionReleaseManager : Singleton<SelectionReleaseManager>
{
    [Header("parameters")]
    public float timeBetweenEffects = 0.1f;
    public float timeBeforeWinCondition = 0.1f;

    [Header("for debugging")]
    public List<PieceController> piecesToRelease;
    public Color releaseColor;

    public int currentPieceToRelease = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleRelease(List<PieceController> selectedPieces, Color colorToUpdate)
    {
        if(selectedPieces.Count < 2)
        {
            return;
        }

        piecesToRelease = new List<PieceController>(selectedPieces);
        releaseColor = colorToUpdate;

        currentPieceToRelease = 0;

        ReleaseNextPiece();
    }

    public void ReleaseNextPiece()
    {
        if(currentPieceToRelease >= piecesToRelease.Count)
        {
            return;
        }

        piecesToRelease[currentPieceToRelease].PlayReleaseAnimation();
        piecesToRelease[currentPieceToRelease].UpdateColor(releaseColor);
        SoundManager.Instance.PlaySelectionSound(currentPieceToRelease);

        currentPieceToRelease++;

        if(currentPieceToRelease == piecesToRelease.Count)
        {
            //this was the last piece to release
            //DO SOMETNHING FDJSKLSKGJ

            piecesToRelease.Clear();

            // launch wait to check win condition
            StartCoroutine(WaitToCheckWinCondition());
            return;
        }

        StartCoroutine(WaitUntilNextRelease());
    }

    IEnumerator WaitUntilNextRelease()
    {
        yield return new WaitForSeconds(timeBetweenEffects);

        ReleaseNextPiece();
    }

    IEnumerator WaitToCheckWinCondition()
    {
        yield return new WaitForSeconds(timeBetweenEffects);
        // check win Condition
        GameOverseer.Instance.CheckWinCondition();
    }
}
