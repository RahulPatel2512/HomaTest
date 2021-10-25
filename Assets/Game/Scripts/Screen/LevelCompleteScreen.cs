using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MoreMountains.NiceVibrations;

public class LevelCompleteScreen : GenericS<LevelCompleteScreen>
{
     [SerializeField] Button Complete_btn;
  
    private void OnEnable() {
        Complete_btn.onClick.AddListener(CompletedFun);
        Events.OnGameFinish+=GameFinish;
    }
    private void OnDisable()
    {
        Events.OnGameFinish-=GameFinish;
    }

    //CompleteBtn Click
    public void CompletedFun(){
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        UIController.Instance.HideThisScreen(ScreenType.LevelCompleteScreen);
        Events.LevelReset();
        Helper.Execute(this,()=>{
            Events.Nextlevel();
            UIController.Instance.ShowThisScreen(ScreenType.GamePlayScreen);
            GamePlayScreen.Instance.StartGame();
        },0.5f);
    }

    //GameFinish than call
    public void GameFinish(){
        UIController.Instance.HideThisScreen(ScreenType.GamePlayScreen);
        Helper.Execute(this,()=>{
            UIController.Instance.ShowThisScreen(ScreenType.LevelCompleteScreen);
        },0.5f);
    }
}
