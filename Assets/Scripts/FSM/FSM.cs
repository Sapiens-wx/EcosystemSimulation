using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public StatesInfo info;
    [HideInInspector] public int id;
    [HideInInspector] public SpriteRenderer spr;
    [HideInInspector] public Rigidbody2D rgb;
    [HideInInspector] public IState curState;
    [HideInInspector] public StateType curStateType;
    [HideInInspector] public StateType prevStateType;
    [HideInInspector] public StateType nextStateType;
    [HideInInspector] public IState nextState;
    public void Init(){
        spr=GetComponent<SpriteRenderer>();
        rgb=GetComponent<Rigidbody2D>();
        curStateType=StateType.None;
    }
    void OnDestroy(){
        GameManager.inst.objs.Remove(this);
    }
    void Start(){
        spr=GetComponent<SpriteRenderer>();
        rgb=GetComponent<Rigidbody2D>();
    }
    void FixedUpdate(){
        if(nextState!=null){
            if(curState!=null)
                curState.OnExit();
            curState=nextState;
            curStateType=nextStateType;
            nextState=null;
            curState.OnEnter();
        }
        if(curState!=null) curState.OnUpdate();
    }
    public void SwitchTo(StateType type){
        nextState=CreateState(type);
        prevStateType=curStateType;
        nextStateType=type;
    }
    IState CreateState(StateType type){
        switch(type){
            case StateType.WhiteHole:
                return null;
            case StateType.Nebula:
                return new Nebula(this);
            case StateType.Asteroid:
                return null;
            case StateType.Moon:
                return new Moon(this);
            case StateType.Planet:
                return new Planet(this);
            case StateType.Star:
                return new Star(this);
            case StateType.BlackHole:
                return new BlackHole(this);
            default: return null;
        }
    }
    void OnCollisionEnter2D(Collision2D collision){
        if(curState!=null){
            curState.OnCollisionEnter2D(collision);
        }
    }
    void OnTriggerEnter2D(Collider2D collider){
        if(curState!=null){
            curState.OnTriggerEnter2D(collider);
        }
    }
}
