using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRestarter : Singleton<SceneRestarter>
{
    public string keyToRestart = "r";

    public float timeBetweenRestarts = 0.01f;

    [Header("For Debugging")]
    public List<PieceController> piecesToRestart;
    public int currentPieceToRestart = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (SelectionManager.Instance == null)
        {
            SceneManager.LoadScene("CommonScene", LoadSceneMode.Additive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyToRestart))
        {
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        //SceneManager.LoadScene(GameOverseer.Instance.currentLevelName);

        piecesToRestart = new List<PieceController>(GameOverseer.Instance.GetAllPiecesInLevel());
        currentPieceToRestart = 0;

        RestartNextPiece();
    }

    public void RestartNextPiece()
    {
        if (currentPieceToRestart >= piecesToRestart.Count)
        {
            return;
        }

        piecesToRestart[currentPieceToRestart].PlayRestartAnimation();
        piecesToRestart[currentPieceToRestart].RestartColor();

        currentPieceToRestart++;

        if (currentPieceToRestart == piecesToRestart.Count)
        {
            //this was the last piece to release
            //DO SOMETNHING FDJSKLSKGJ

            piecesToRestart.Clear();

            // check win Condition
            GameOverseer.Instance.CheckWinCondition();
            return;
        }

        StartCoroutine(WaitUntilNextRestart());
    }

    IEnumerator WaitUntilNextRestart()
    {
        yield return new WaitForSeconds(timeBetweenRestarts);

        RestartNextPiece();
    }
}
