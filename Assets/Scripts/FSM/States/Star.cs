using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Star : IState
{
    List<Coroutine> coros;
    float[] childDistances;
    List<Planet> childPlanets;
    public FSM annihilateObj;
    public Star(FSM fsm): base(fsm){
        childDistances=new float[fsm.info.star.childMaxCount];
        float gDist=fsm.info.star.gravityDistance;
        float gap=(gDist-fsm.info.star.scale)/(childDistances.Length+1);
        for(int i=childDistances.Length-1;i>=0;--i){
            childDistances[i]=gDist-gap-gap*i;
        }
    }
    public override void OnEnter()
    {
        InitOnEnter(fsm.info.star);
        childPlanets=new List<Planet>();
        coros=new List<Coroutine>();
        coros.Add(fsm.StartCoroutine(DetectCollision()));
    }

    public override void OnExit()
    {
        base.OnExit();
        foreach(Coroutine c in coros){
            if(c==null) continue;
            fsm.StopCoroutine(c);
        }
    }

    public override void OnUpdate()
    {
        //update planets
        for(int i=0;i<childPlanets.Count;++i){
            Vector2 offset=childPlanets[i].fsm.transform.position-fsm.transform.position;
            offset.Normalize();
            offset*=childDistances[i];
            childPlanets[i].fsm.transform.position=fsm.transform.position+(Vector3)Utilities.RotateVec2(offset, fsm.info.star.rotateSpeed);
        }
    }
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        FSM f=collision.gameObject.GetComponent<FSM>();
        switch(f.curStateType){
            case StateType.Star:
                Star star=f.curState as Star;
                if(star.annihilateObj!=null) break;
                annihilateObj=star.fsm;
                f.StartCoroutine(star.DieAndDestroy());
                RemoveChildPlanets();
                fsm.SwitchTo(StateType.BlackHole);
                break;
        }
    }
    void RemoveChildPlanets(){
        for(int i=childPlanets.Count-1;i>=0;--i){
            childPlanets[i].Star=null;
        }
    }
    void AddToPlanet(Planet planet){
        if(planet.Star!=null) return;
        planet.childIndex=childPlanets.Count;
        planet.Star=this;
        childPlanets.Add(planet);
    }
    public void RemovePlanetFromChild(int idx){
        childPlanets[idx]=childPlanets[^1];
        childPlanets[idx].childIndex=idx;
        childPlanets.RemoveAt(childPlanets.Count-1);
    }
    IEnumerator DetectCollision(){
        WaitForSeconds wait=new WaitForSeconds(.1f);
        while(true){
            List<FSM> objs=GameManager.inst.ObjCloserThan(fsm, fsm.info.star.gravityDistance);
            foreach(FSM f in objs){
                switch(f.curStateType){
                    case StateType.Star:
                        f.rgb.AddForce(fsm.info.star.gravity*(fsm.transform.position-f.transform.position));
                        break;
                    case StateType.Planet:
                        if(childPlanets.Count>=childDistances.Length) break;
                        Planet planet=(Planet)f.curState;
                        if(planet!=null && planet.Star==null)
                            AddToPlanet(planet);
                        break;
                }
            }
            yield return wait;
        }
    }
}