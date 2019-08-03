using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRestarter : MonoBehaviour
{
    public string keyToRestart = "r";

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
            RestartCurrentScene();
        }
    }

    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(GameOverseer.Instance.currentLevelName);
    }
}
