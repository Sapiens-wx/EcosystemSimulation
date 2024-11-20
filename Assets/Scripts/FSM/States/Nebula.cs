using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nebula : IState
{
    public Nebula(FSM fsm): base(fsm){

    }
    public override void OnEnter()
    {
        InitOnEnter(fsm.info.nebula);
        fsm.rgb.velocity=Vector2.zero;
        fsm.rgb.angularVelocity=0;
        fsm.rgb.GetComponent<Collider2D>().isTrigger=true;
    }

    public override void OnExit()
    {
        base.OnExit();
        fsm.rgb.GetComponent<Collider2D>().isTrigger=false;
    }

    public override void OnUpdate()
    {
    }
    public override void OnTriggerEnter2D(Collider2D collider) {
        FSM f=collider.GetComponent<FSM>();
        switch(f.curStateType){
            case StateType.Moon:
                Moon moon=f.curState as Moon;
                moon.Planet=null;
                f.SwitchTo(StateType.Planet);
                fsm.StartCoroutine(DieAndDestroy());
                break;
            case StateType.Planet:
                Planet planet=f.curState as Planet;
                if(planet.Star!=null) break;
                f.SwitchTo(StateType.Star);
                fsm.StartCoroutine(DieAndDestroy());
                break;
        }
    }
    IEnumerator DetectCollision(){
        WaitForSeconds wait=new WaitForSeconds(.1f);
        while(true){
            List<FSM> objs=GameManager.inst.ObjCloserThan(fsm, fsm.info.blackHole.gravityDistance);
            foreach(FSM f in objs){
                f.rgb.AddForce(fsm.info.blackHole.gravity*(fsm.transform.position-f.transform.position));
                switch(f.curStateType){
                }
            }
            yield return wait;
        }
    }
}