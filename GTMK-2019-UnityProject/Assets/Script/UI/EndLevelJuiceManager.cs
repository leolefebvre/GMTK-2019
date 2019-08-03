using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelJuiceManager : Singleton<EndLevelJuiceManager>
{
    public GameObject victoryAnimation;
    public GameObject victorySound;

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
        victoryAnimation.SetActive(true);
        victorySound.SetActive(true);
    }
}
