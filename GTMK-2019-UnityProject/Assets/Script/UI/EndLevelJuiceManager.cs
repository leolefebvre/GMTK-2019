using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelJuiceManager : Singleton<EndLevelJuiceManager>
{
    public GameObject victoryAnimatorGameObject;
    public GameObject victorySound;

    public AnimationClip victoryAnimationClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchVictoryJuice()
    {
        victoryAnimatorGameObject.SetActive(true);
        victorySound.SetActive(true);

        StartCoroutine(WaitToAddVictoryJuice());
    }

    public void HideAndResetVictoryJuice()
    {
        victoryAnimatorGameObject.SetActive(false);
        victorySound.SetActive(false);
    }

    IEnumerator WaitToAddVictoryJuice()
    {
        yield return new WaitForSeconds(victoryAnimationClip.length);

        HideAndResetVictoryJuice();
    }
}
