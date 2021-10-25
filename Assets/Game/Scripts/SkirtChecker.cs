using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class SkirtChecker : MonoBehaviour
{
    [SerializeField] float decreseValue;
    [SerializeField] GameObject Ring;
    [SerializeField] List<Color> Colors;
    [SerializeField] Transform ringPostion;

    void OnEnable()
    {
        Events.OnGameReset+=Reset;
    }

    void OnDisable()
    {
        Events.OnGameReset-=Reset;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Over"))
        {
            if (transform.localScale.x > 100)
            {
                MMVibrationManager.Haptic(HapticTypes.MediumImpact, false, true, this); 
                transform.localScale -= new Vector3(decreseValue, decreseValue, 0);
                PlayerController.tempOffset -= 0.1f;
                PlayerController.tempOffset = Mathf.Clamp(PlayerController.tempOffset,0.1f,1.5f);
                GetComponent<Renderer>().materials[1].mainTextureScale = new Vector2( PlayerController.tempOffset, 1);
                GenerateRing();
               
            }
        }
    }

    public void Reset(){
        GetComponent<Renderer>().materials[1].mainTextureScale = new Vector2( 0.1f, 1);
    }


    public void GenerateRing()
    {
        GameObject g = Instantiate(Ring);
        g.GetComponent<Renderer>().material.color = Colors[PlayerController.colorIndex];
        if (PlayerController.colorIndex != 0)
        {
            PlayerController.colorIndex -= 1;
        }
                
        g.transform.SetParent(transform);
        g.transform.localPosition = ringPostion.localPosition;
        g.transform.localRotation = ringPostion.localRotation;
        g.transform.localScale=new Vector3(ringPostion.localScale.x,ringPostion.localScale.y*PlayerController.tempOffset,ringPostion.localScale.z);
        g.transform.SetParent(null);
        Destroy(g,1f);
    }
    
    
}
