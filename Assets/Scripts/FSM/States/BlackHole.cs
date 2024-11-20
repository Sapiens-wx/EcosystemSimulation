using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : IState
{
    Coroutine detectCoro;
    public BlackHole(FSM fsm): base(fsm){

    }
    public override void OnEnter()
    {
        InitOnEnter(fsm.info.blackHole);
        fsm.rgb.bodyType=RigidbodyType2D.Kinematic;
        detectCoro=fsm.StartCoroutine(DetectCollision());
        fsm.rgb.velocity=Vector2.zero;
    }

    public override void OnExit()
    {
        base.OnExit();
        if(detectCoro!=null) fsm.StopCoroutine(detectCoro);
    }

    public override void OnUpdate()
    {
    }
    public override void OnTriggerEnter2D(Collider2D collider)
    {
        FSM f=collider.gameObject.GetComponent<FSM>();
        IState obj=f.curState;
        f.StartCoroutine(obj.DieAndDestroy());
    }
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        FSM f=collision.gameObject.GetComponent<FSM>();
        IState obj=f.curState;
        f.StartCoroutine(obj.DieAndDestroy());
    }
    IEnumerator DetectCollision(){
        WaitForSeconds wait=new WaitForSeconds(.1f);
        while(true){
            List<FSM> objs=GameManager.inst.ObjCloserThan(fsm, fsm.info.blackHole.gravityDistance);
            foreach(FSM f in objs){
                f.rgb.AddForce(fsm.info.blackHole.gravity*(fsm.transform.position-f.transform.position));
                switch(f.curStateType){
                    case StateType.Moon:
                        ((Moon)f.curState).Planet=null;
                        break;
                }
            }
            yield return wait;
        }
    }
}