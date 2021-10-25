using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MoreMountains.NiceVibrations;

public class LevelFailScreen : GenericS<LevelFailScreen>
{
     [SerializeField] Button Retry_btn;
  
    private void OnEnable() {
        Retry_btn.onClick.AddListener(RetryFunc);
        Events.OnGameOver+=GameOver;
    }
    private void OnDisable()
    {
        Events.OnGameOver-=GameOver;
    }

    //Retrybtn click function
    public void RetryFunc(){
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        UIController.Instance.HideThisScreen(ScreenType.LevelFailScreen);
        Events.LevelReset();
        Helper.Execute(this,()=>{
            Events.LevelSetup(GameManager.Instance.LevelNumber);
            UIController.Instance.ShowThisScreen(ScreenType.GamePlayScreen);
            GamePlayScreen.Instance.StartGame();
        },0.5f);
    }

    //Game OverFunc
    public void GameOver(){
        UIController.Instance.HideThisScreen(ScreenType.GamePlayScreen);
        Helper.Execute(this,()=>{
            UIController.Instance.ShowThisScreen(ScreenType.LevelFailScreen);
        },0.5f);
    }
}
