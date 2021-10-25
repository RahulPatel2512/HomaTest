using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GamePlayScreen : GenericS<GamePlayScreen>
{
    [SerializeField] GameObject Taptostart;
    [SerializeField]  Image progressImage;
    public Text levelText;

    float _progress;
    public float Progress{
        get{
            return _progress;
        }
        set{
            _progress=value;
            progressImage.fillAmount=_progress;
        }
    }
    
    private void OnEnable() {
        Events.OnGameReset+=Reset;
    }

    private void OnDisable()
    {
        Events.OnGameReset-=Reset;
    }

    void Reset()
    {
        Progress=0;
    }

    //StartFunc
    public void StartGame(){
        StartCoroutine(GameStrart(Taptostart,()=>{
            GameManager.Instance.StartGame();
        }));
    }

    //GameStart Coroutine
    IEnumerator GameStrart(GameObject obj,Action action){
        obj.SetActive(true);
        while(true){
            if(Input.GetMouseButtonDown(0) && !IsPointerOverUIObject() && obj.activeInHierarchy){
                break;
            }
            yield return null;
        }
        action?.Invoke();
        obj.SetActive(false);
    }

    //Raycast in UI
    public static bool IsPointerOverUIObject () {
        PointerEventData eventDataCurrentPosition = new PointerEventData (EventSystem.current);
        eventDataCurrentPosition.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult> ();
        EventSystem.current.RaycastAll (eventDataCurrentPosition, results);
        return results.Count > 0;
    }

}
