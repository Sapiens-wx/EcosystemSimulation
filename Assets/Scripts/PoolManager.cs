using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager:MonoBehaviour{
    [SerializeField] GameObject objPrefab;

    public static PoolManager inst;
    public ObjectPool<FSM> pool;
    void Awake(){
        inst=this;
        pool=new ObjectPool<FSM>(CreateFunc, OnGet, OnRelease, null, true, 10, 1000);
    }
    FSM CreateFunc(){
        GameObject obj=Instantiate(objPrefab);
        obj.SetActive(false);
        return obj.GetComponent<FSM>();
    }
    void OnGet(FSM fsm){
        fsm.gameObject.SetActive(true);
    }
    void OnRelease(FSM fsm){
        fsm.curState.OnExit();
        fsm.gameObject.SetActive(false);
        fsm.prevStateType=StateType.None;
        fsm.curState=null;
        fsm.curStateType=StateType.None;
        fsm.nextState=null;
        fsm.nextStateType=StateType.None;
        fsm.spr.sprite=null;
    }
}