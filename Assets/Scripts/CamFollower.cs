using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamFollower : MonoBehaviour{
    public float spd;
    public int interval;

    int count;
    FSM target;
    Vector3 pos;
    void Start(){
        pos=transform.position;
        count=0;
    }
    void FixedUpdate(){
        if(target==null)
            FindRandFSMToFollow();
        else{
            ++count;
            pos.x=Mathf.Lerp(pos.x, target.transform.position.x, spd);
            pos.y=Mathf.Lerp(pos.y, target.transform.position.y, spd);
            transform.position=pos;
            if(count>=interval){
                count=0;
                FindRandFSMToFollow();
            }
        }
    }
    void FindRandFSMToFollow(){
        if(GameManager.inst.objs.Count==0) return;
        target=GameManager.inst.objs[UnityEngine.Random.Range(0, GameManager.inst.objs.Count)];
    }
}