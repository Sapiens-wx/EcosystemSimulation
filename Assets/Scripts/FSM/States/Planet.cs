using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : IState
{
    Star star;
    /// <summary>
    /// index in the star.childPlanets list
    /// </summary>
    public int childIndex;
    public Star Star{
        get=>star;
        set{
            if(value!=null && roamCoro!=null){
                fsm.StopCoroutine(roamCoro);
                roamCoro=null;
            }else if(value==null && roamCoro==null){
                roamCoro = fsm.StartCoroutine(Roam(fsm.info.planet));
            }
            if(star!=null){
                if(value!=null)
                    Debug.LogError("star!=null and value!=null");
                star.RemovePlanetFromChild(childIndex);
            }
            star=value;
        }
    }
    float[] childDistances;
    List<Moon> childMoons;
    Coroutine detColCoro;
    public Planet(FSM fsm): base(fsm){
        childMoons=new List<Moon>();
        childDistances=new float[fsm.info.planet.childMaxCount];
        float gDist=fsm.info.planet.gravityDistance;
        float gap=(gDist-fsm.info.planet.scale)/(childDistances.Length+1);
        for(int i=childDistances.Length-1;i>=0;--i){
            childDistances[i]=gDist-gap-gap*i;
        }
    }
    public override void OnEnter()
    {
        InitOnEnter(fsm.info.planet);
        detColCoro=fsm.StartCoroutine(DetectCollision());
    }

    public override void OnExit()
    {
        base.OnExit();
        if(detColCoro!=null) fsm.StopCoroutine(detColCoro);
        if(star!=null){
            Star=null;
        }
    }

    public override void OnUpdate()
    {
        //update planets
        for(int i=0;i<childMoons.Count;++i){
            Vector2 offset=childMoons[i].fsm.transform.position-fsm.transform.position;
            offset.Normalize();
            offset*=childDistances[i];
            childMoons[i].fsm.transform.position=fsm.transform.position+(Vector3)Utilities.RotateVec2(offset, fsm.info.planet.rotateSpeed);
        }
    }
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        FSM f=collision.gameObject.GetComponent<FSM>();
        switch(f.curStateType){
        }
    }
    void RemoveChildPlanets(){
        Debug.LogError("function not implemented");
    }
    void AddToPlanet(Moon moon){
        if(moon.Planet!=null) return;
        moon.childIndex=childMoons.Count;
        moon.Planet=this;
        childMoons.Add(moon);
    }
    public void RemoveMoonFromChild(int idx){
        childMoons[idx]=childMoons[^1];
        childMoons[idx].childIndex=idx;
        childMoons.RemoveAt(childMoons.Count-1);
    }
    IEnumerator DetectCollision(){
        WaitForSeconds wait=new WaitForSeconds(.1f);
        while(true){
            List<FSM> objs=GameManager.inst.ObjCloserThan(fsm, fsm.info.planet.gravityDistance);
            foreach(FSM f in objs){
                switch(f.curStateType){
                    //case StateType.Planet:
                    //    f.rgb.AddForce(fsm.info.planet.gravity*(fsm.transform.position-f.transform.position));
                    //    break;
                    case StateType.Moon:
                        if(childMoons.Count>=childDistances.Length) break;
                        Moon moon=(Moon)f.curState;
                        if(moon!=null && moon.Planet==null)
                            AddToPlanet(moon);
                        break;
                }
            }
            yield return wait;
        }
    }
}