using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerFollower : GenericS<PlayerFollower>
{
    [SerializeField] Transform follow_C;
    [SerializeField] CinemachineVirtualCamera vcam;
    public Transform target;
    [SerializeField] float smothVal = 0.3f;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] bool useDamp = true;

    void OnEnable()
    {
        Events.OnGameReset+=Reset;
    }
    void OnDisable()
    {
        Events.OnGameReset-=Reset;
    }

    private void Reset()
    {
        target=null;
        vcam.Follow = null;
        follow_C.gameObject.SetActive(true);
        transform.position=new Vector3(0,-1.4f,15.23f);
        follow_C.transform.position=new Vector3(0,7.02f,2.04f);
        follow_C.transform.eulerAngles=new Vector3(23.75f,0,0);
        vcam.Follow = transform;
    }


    void FixedUpdate()
    {
        if(target==null){
            return;
        }else{
            DoUpdate();
        }
    }

    void DoUpdate()
    {
        if (useDamp){
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smothVal);
        }
        else
        {
            transform.position = target.transform.position; // + offset;
        }
    }

}