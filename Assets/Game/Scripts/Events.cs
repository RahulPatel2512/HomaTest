using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Events : MonoBehaviour {
    //events

    //GameRestrt Event
    public static Action OnGameReset = delegate { };
    //NextLvl Event
    public static Action OnNextLevel = delegate { };    
    //Level setup with level number
    public static event Action<int> OnLevelSetup;
    //Gamefinish Event
    public static event Action OnGameFinish;
    //GameOver Event
    public static event Action OnGameOver;

    public static void LevelReset()
    {
        OnGameReset();
    }

    public static void LevelSetup(int _levelnumber){
        OnLevelSetup(_levelnumber);
    }
    
    public static void GameFinish(){
        OnGameFinish();
    }

    public static void GameOver(){
        OnGameOver();
    }

    public static void Nextlevel(){
        OnNextLevel();
    }
  
}