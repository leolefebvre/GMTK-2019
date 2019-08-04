using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelJuiceManager : Singleton<EndLevelJuiceManager>
{
    public GameObject victoryAnimatorGameObject;
    public AudioSource VictorySoundSource;

    public AnimationClip victoryAnimationClip;

    public Image victoryStripe1;
    public Image victoryStripe2;
    public Image victoryStripe3;
    public Image victoryStripe4;
    public Image victoryStripe5;

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
        if(LevelColors.Instance != null)
        {
            victoryStripe1.color = copyColorWithoutAlpha(LevelColors.Instance.color1);
            victoryStripe2.color = copyColorWithoutAlpha(LevelColors.Instance.color2);
            victoryStripe3.color = copyColorWithoutAlpha(LevelColors.Instance.color3);
            victoryStripe4.color = copyColorWithoutAlpha(LevelColors.Instance.color4);
            victoryStripe5.color = copyColorWithoutAlpha(LevelColors.Instance.color5);
        }

        victoryAnimatorGameObject.SetActive(true);
        VictorySoundSource.Play();

        StartCoroutine(WaitToAddVictoryJuice());
    }

    public void HideAndResetVictoryJuice()
    {
        victoryAnimatorGameObject.SetActive(false);
    }

    IEnumerator WaitToAddVictoryJuice()
    {
        yield return new WaitForSeconds(victoryAnimationClip.length);

        HideAndResetVictoryJuice();
    }

    Color copyColorWithoutAlpha(Color newColor)
    {
        return new Color(newColor.r, newColor.g, newColor.b, 1);
    }
}
