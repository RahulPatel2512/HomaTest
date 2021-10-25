using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : GenericS<GameManager>
{
    [SerializeField] LevelCreators Alllevels;
    public Levels C_lvl;
    public GameObject Player,C_Player,FinishLine;
    int _levelnumber;
    private GameObject C_levelObj,C_Finish;
    public int LevelNumber{
        get{
            return _levelnumber;
        }
        set{
            _levelnumber=value;
            DataManager.Instance.data.Current_Level = _levelnumber;
            GamePlayScreen.Instance.levelText.text="Level "+(_levelnumber+1);
        }
    }

    void OnEnable()
    {
        Events.OnLevelSetup+=LevelSetUp; 
        Events.OnNextLevel+=NextLevel; 
        Events.OnGameReset+=Reset;
    }

    void OnDisable()
    {
        Events.OnLevelSetup-=LevelSetUp; 
        Events.OnNextLevel-=NextLevel;
        Events.OnGameReset-=Reset;
    }

    void Start()
    {
        LevelNumber=DataManager.Instance.data.Current_Level;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = false;
    }

    //LevelSetup
    public void LevelSetUp(int _lvlnum){
        C_levelObj=Instantiate(Alllevels.Levels[_lvlnum].gameObject);
        C_lvl = C_levelObj.GetComponent<Levels>();
        C_Finish=Instantiate(FinishLine,C_lvl.EndPose.position,Quaternion.identity);
        C_Player=Instantiate(Player,new Vector3(0,-1.4f,15.23f),Quaternion.identity);
        C_Finish.GetComponent<FinishLine>().SetPlayer(C_Player.transform);
        PlayerFollower.Instance.target=C_Player.transform;
    }

    //ResetAll
    public void Reset(){
        C_Player.GetComponent<PlayerController>().wincam.SetActive(false);
        Destroy(C_Player);
        Destroy(C_levelObj);
        Destroy(C_Finish);
    }

    //TaptoStart
    public void StartGame()
    {
        C_Player.GetComponent<PlayerController>().StartPlayerMovement();
    }

    //NextLevel 
    public void NextLevel()
    {
        if(LevelNumber==(Alllevels.Levels.Count-1)){
            LevelNumber=0;   
        }else{
            LevelNumber++;
        }
        Events.LevelSetup(LevelNumber);
    }
    
}
