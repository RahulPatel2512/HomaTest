using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class WinCube : MonoBehaviour
{
    [SerializeField] Renderer rand;
    [SerializeField] Color _color;
    [SerializeField] TextMeshPro tmp;
    [SerializeField] int bonusValue;
    [SerializeField] Transform refPos;
    public Transform Player;
    [SerializeField] Animator anim;
    [SerializeField] Transform WalkPos;
   
    void Start()
    {
        rand.material.color = _color;
        tmp.text = "x"+bonusValue.ToString();
    }
    public void setForWin()
    {
        Destroy(Player.GetComponent<Rigidbody>());
        Player.DORotate(refPos.eulerAngles, 0.4f);
        anim.SetTrigger("walk");
        Player.GetComponent<Animator>().SetTrigger("idel");
        Player.DOMove(refPos.position, 0.6f).OnComplete(CompliteAnim);
    }
    void SetBoy()
    {
        anim.SetTrigger("idel");
    }
    void CompliteAnim()
    {
        anim.transform.DOLocalMove(WalkPos.localPosition, 1f).OnComplete(SetBoy);
        Player.GetComponent<Animator>().SetTrigger("idel");
        Player.GetComponent<PlayerController>().winSetUp();
        Helper.Execute(this,()=>Dance(),1.9f);
    }
    public void Dance()
    {
        Player.GetComponent<PlayerController>().heartImojiPlay();
        Player.GetComponent<Animator>().SetTrigger("kiss");
        anim.SetTrigger("kiss");
        Helper.Execute(this,()=> Events.GameFinish(),2f);
    }
    
}
