﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverseer : Singleton<GameOverseer>
{
    #region level management parameter
    [Header("level management parameters")]
    public List<string> levelList;
    public int currentLevelIndex = 0;
    public string currentLevelName
    {
        get { return levelList[currentLevelIndex]; }
    }

    #endregion

    #region references parameter

    [Header("references parameter")]
    public Button resetButton;
    public Button quitButton;

    #endregion

    #region Win Condition parameters
    [Header("win parameter")]
    public float victoryAnimationDelay = 1.0f;
    [SerializeField]
    private List<PieceController> allPieces;
    public bool isPieceListEmpty
    {
        get { return allPieces.Count == 0; }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        CheckOtherLevelsOnStart();
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("w"))
        {
            LaunchVictory();
        }
    }

    #region victory management

    public void PopulateAllPiecesInLevelList()
    {
        PieceController[] allPiecesFond = FindObjectsOfType<PieceController>();

        foreach (PieceController currentPiece in allPiecesFond)
        {
            AddPiecetoList(currentPiece);
        }
    }

    public void AddPiecetoList(PieceController pieceToAdd)
    {
        if (allPieces.Contains(pieceToAdd))
        {
            return;
        }

        allPieces.Add(pieceToAdd);
    }

    public void ClearPiecesList()
    {
        allPieces.Clear();
    }

    public void CheckWinCondition()
    {
        if(isPieceListEmpty)
        {
            PopulateAllPiecesInLevelList();
        }

        foreach (PieceController currentPieceControler in allPieces)
        {
            if(currentPieceControler.isInWinColor == false)
            {
                return;
            }
        }

        StartCoroutine(WaitToLaunchVictory());
    }

    IEnumerator WaitToLaunchVictory()
    {
        yield return new WaitForSeconds(victoryAnimationDelay);

        LaunchVictory();
    }

    public void LaunchVictory()
    {
        EndLevelJuiceManager.Instance.LaunchVictoryJuice();
        // The new level loading will be triggered throught the animation
    }

    public List<PieceController> GetAllPiecesInLevel()
    {
        if (isPieceListEmpty)
        {
            PopulateAllPiecesInLevelList();
        }

        return new List<PieceController>(allPieces);
    }

    #endregion

    #region loading management

    // should only work for Editor
    public void CheckOtherLevelsOnStart()
    {
        if(SceneManager.sceneCount > 1)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if(levelList.Contains(SceneManager.GetSceneAt(i).name))
                {
                    currentLevelIndex = levelList.FindIndex(x => x.Equals(SceneManager.GetSceneAt(i).name));
                }
            }
        }
        else
        {
            LoadFirstLevel();
        }
    }

    private void LoadFirstLevel()
    {
        currentLevelIndex = 0;
        SceneManager.LoadScene(currentLevelName);
    }

    public void LoadNextLevel()
    {
        //start fondu au noir

        //load new scene

        if (currentLevelIndex == levelList.Count - 1)
        {
            //Debug.Log("This was the last level");
            return;
        }

        currentLevelIndex++;
        SceneManager.LoadScene(currentLevelName);
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.name + " with mode " + mode);

        SelectionManager.Instance.RemoveAllSelectedInList();
        ClearPiecesList();
    }

    #endregion

    public void ResquestReset()
    {
        //Debug.Log("click on button");
        SceneRestarter.Instance.RestartLevel();
    }

    public void DisableQuitButton()
    {
        if(quitButton != null)
        {
            quitButton.interactable = false;
        }
    }

    public void EnableQuitButton()
    {
        if (quitButton != null)
        {
            quitButton.interactable = true;
        }
    }

    public void RequestQuit()
    {
        Debug.Log("quit");
        Application.Quit();
    }

}
