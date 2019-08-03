using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverseer : Singleton<GameOverseer>
{
    [SerializeField]
    private List<PieceController> allPieces;

    public bool isPieceListEmpty
    {
        get { return allPieces.Count == 0; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetAllPiecesInLevel()
    {
        PieceController[] allPiecesFond = FindObjectsOfType<PieceController>();

        foreach (PieceController currentPiece in allPiecesFond)
        {
            addPiecetoList(currentPiece);
        }
    }

    public void addPiecetoList(PieceController pieceToAdd)
    {
        if (allPieces.Contains(pieceToAdd))
        {
            return;
        }

        allPieces.Add(pieceToAdd);
    }

    public void CheckWinCondition()
    {
        if(isPieceListEmpty)
        {
            GetAllPiecesInLevel();
        }

        foreach (PieceController currentPieceControler in allPieces)
        {
            if(currentPieceControler.isInWinColor == false)
            {
                return;
            }
        }
        
        LaunchVictory();
    }

    public void LaunchVictory()
    {
        EndLevelJuiceManager.Instance.LaunchVictoryJuice();
    }
}
