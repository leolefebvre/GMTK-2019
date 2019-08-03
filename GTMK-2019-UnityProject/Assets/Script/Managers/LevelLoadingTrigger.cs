using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoadingTrigger : MonoBehaviour
{
    public void TriggerLoadLevel()
    {
        GameOverseer.Instance.LoadNextLevel();
    }
}
