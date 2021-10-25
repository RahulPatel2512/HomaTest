using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Animator playerAnimator;
    [SerializeField] float speed;
    [SerializeField] float slideSpeed;
    [SerializeField] Transform ragDOllParent;
    [SerializeField] Transform scurt;
    [SerializeField] bool startFly;
    [SerializeField] float flyFactor;
    [SerializeField] float scaleUpFactor;
    [SerializeField] Renderer skirtRand;
    [SerializeField] GameObject DressParticle;
    public GameObject wincam;
    [SerializeField] GameObject playerCam;
    [SerializeField] GameObject Confatty;
    public static int colorIndex;
    [SerializeField]private SkirtChecker skirtChecker;
    [SerializeField] GameObject rainParticle;
    [SerializeField] float FlySpeed;
    [SerializeField] float tempVelocity;
    [SerializeField] bool RechedFinish;
    [SerializeField] GameObject HeartEmoji;
    [SerializeField] bool isStop=true;
    private Vector2 startTouch=Vector2.zero;
    private Vector2 EnddTouch=Vector2.zero;
    private Vector2 DeltaTouch=Vector2.zero;
    float pathlength;
    public static float tempOffset = 0.1f;
    [SerializeField] float tempFactor = 0.0f;

    private void Awake()
    {
        DeactivateRagdoll();
    }    

    void OnEnable()
    {
        pathlength=GameManager.Instance.C_lvl.EndPose.position.z;
        playerCam=GameObject.FindWithTag("P_camera").gameObject;
        rainParticle=playerCam.transform.GetChild(0).gameObject;
    }

    void Update()
    {

        if (isStop)
        {
            startTouch = Input.mousePosition;
            EnddTouch = Vector2.zero;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            startTouch = Input.mousePosition;
            EnddTouch = Vector2.zero;
        }

        if (Input.GetMouseButton(0))
        {
            EnddTouch = Input.mousePosition;
            DeltaTouch = startTouch - EnddTouch;
        }    
        startTouch = EnddTouch;
        player.position = new Vector3 (Mathf.Clamp(Mathf.Lerp(player.position.x, player.position.x - (DeltaTouch.x*0.015f) , slideSpeed*Time.deltaTime), -4.1f, 4.1f),player.position.y, player.position.z+Time.deltaTime*speed);
        GamePlayScreen.Instance.Progress=Helper.Map(15.23f,pathlength,0,1,player.position.z);
        if (startFly)
        {
            startFlying();        
        }
        
    }
    
    public void StartPlayerMovement()
    {
        isStop=false;
        playerAnimator.SetTrigger("walk");
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Positive"))
        {
            MMVibrationManager.Haptic(HapticTypes.MediumImpact, false, true, this); 
            GameObject g = Instantiate(DressParticle);
            g.transform.position = other.transform.position;
            Destroy(other.gameObject);
           scurt.DOScale(scurt.transform.localScale+new Vector3(scaleUpFactor,scaleUpFactor,0f),0.2f);
           DOTween.To(()=> tempOffset, x=> tempOffset = x, tempOffset+0.1f, 0.2f).OnUpdate(OnOffsetChnage);
           if (colorIndex == 9)
           {
               colorIndex = 0;
           }
           else
           {
               colorIndex += 1;
           }
         
        }


        if (other.CompareTag("StageEnd"))
        {
            tempFactor = 0;
            playerAnimator.SetTrigger("flot");
            startFly = true;
            StartVelocity();
            rainParticle.SetActive(true);
        }
        if (other.CompareTag("StageStart"))
        {
             if (startFly)
             {
                StopVelocity();
                startFly = false;
                GetComponent<Rigidbody>().useGravity = true; 
            }
           
        }
        if (other.CompareTag("Respawn"))
        {
            isStop = true;
            ActivateRagdoll();
        }
        if (other.CompareTag("Finish"))
        {
            RechedFinish = true;
            startFly = true;
            StartVelocity();
            playerAnimator.SetTrigger("flot");
            rainParticle.SetActive(true);

        }

        
        if (other.CompareTag("FinishBlocks"))
        {      
            if (scurt.localScale.x <= 100)
            {
                rainParticle.SetActive(false);
                isStop = true;
                other.GetComponent<WinCube>().setForWin();
            }
            else
            {
                tempFactor = 0;
                scurt.localScale-=new Vector3(15,15,0);
                tempOffset -= 0.1f;
                tempOffset=  Mathf.Clamp(tempOffset, 0.1f, 1.5f);
                skirtRand.materials[1].mainTextureScale = new Vector2(tempOffset, 1);
                skirtChecker.GenerateRing();
            }
           
        }

        if (other.CompareTag("LastBlock"))
        {
            scurt.localScale=new Vector3(100,100,100);
            rainParticle.SetActive(false);
            isStop = true;
            other.GetComponent<WinCube>().setForWin();
        }
    }


    public void winSetUp()
    {
        rainParticle.SetActive(false);
        wincam.transform.SetParent(null);
        wincam.SetActive(true);
        playerCam.SetActive(false);
        Helper.Execute(this,()=>OnConfetty(),1.7f);
    }

    public void OnConfetty()
    {
    
        MMVibrationManager.Haptic(HapticTypes.Success, false, true, this); 
        Confatty.SetActive(true);
    }

    public void heartImojiPlay()
    {
        HeartEmoji.SetActive(true);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (isStop)
        {
            return;
        }
        if (other.collider.CompareTag("surface"))
        {
            tempFactor = 0;
            rainParticle.SetActive(false);
            playerAnimator.SetTrigger("walk");
        }
        
        if (other.collider.CompareTag("Over"))
        {
            isStop = true;
           Debug.Log("GameOver");
           ActivateRagdoll();
        }
        
       
    }

    public void OnOffsetChnage()
    {
        skirtRand.materials[1].mainTextureScale = new Vector2(tempOffset, 1);
    }    
    public void startFlying()
    {
        tempFactor += flyFactor;
        GetComponent<Rigidbody>().velocity = transform.forward*tempVelocity;

        if (RechedFinish)
        {
            return;
        }
        
        if (tempFactor % 15 == 0)
        {
           
            tempFactor = 0;
            scurt.localScale-=new Vector3(15,15,0);
            tempOffset -= 0.1f;
            tempOffset=  Mathf.Clamp(tempOffset, 0.1f, 1.5f);
            skirtRand.materials[1].mainTextureScale = new Vector2(tempOffset, 1);
            skirtChecker.GenerateRing();
        }
        
        if (scurt.localScale.x <= 100)
        {
            GetComponent<Rigidbody>().useGravity = true;
            startFly = false;
            StopVelocity();
            tempFactor = 0.0f;
        }
    }


    public void StopVelocity()
    {
        DOTween.To(() => tempVelocity, x => tempVelocity = x, 0, 0.3f).OnUpdate(StopCOmplite).SetEase(Ease.InCubic).SetUpdate(UpdateType.Fixed);
    }


    public void StartVelocity()
    {
        rainParticle.SetActive(true);
        tempVelocity = 0;
        DOTween.To(() => tempVelocity, x => tempVelocity = x, FlySpeed, 0.3f).SetUpdate(UpdateType.Fixed); //.OnUpdate(OnOffsetChnage);
    }
    
    void StopCOmplite()
    {
        rainParticle.SetActive(false);
        GetComponent<Rigidbody>().velocity=Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity=Vector3.zero;
        
    }
   
    public void DeactivateRagdoll(){
    
        foreach(Rigidbody bone in 
            ragDOllParent.GetComponentsInChildren<Rigidbody>()){
            bone.isKinematic = true;
            bone.detectCollisions = false;
        }
        foreach (CharacterJoint joint in 
            ragDOllParent.GetComponentsInChildren<CharacterJoint>()) {
            joint.enableProjection = true;
        }
        foreach(Collider col in 
            ragDOllParent.GetComponentsInChildren<Collider>()){
            col.enabled = false;
        }

        ragDOllParent.GetComponent<Rigidbody>().isKinematic = false;
        ragDOllParent.GetComponent<Rigidbody>().detectCollisions = true;
        scurt.GetComponent<Collider>().enabled = true;
    }
    
    public void ActivateRagdoll()
    {
       
        MMVibrationManager.Haptic(HapticTypes.Failure, false, true, this); 
        GetComponent<Animator>().enabled = false;
         GetComponent<Collider>().enabled
            = false;
         GetComponent<Rigidbody>().isKinematic
             = true;
        foreach (Rigidbody bone in
            ragDOllParent.GetComponentsInChildren<Rigidbody>())
        {
    
            bone.isKinematic = false;
            bone.detectCollisions = true;
    
        }
    
        foreach (Collider col in ragDOllParent.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }
        Events.GameOver();
    }
}
