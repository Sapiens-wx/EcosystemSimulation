using UnityEngine;

public class Moon : IState
{
    Planet planet;
    /// <summary>
    /// index in the planet.childMoons list
    /// </summary>
    public int childIndex;
    public Planet Planet{
        get=>planet;
        set{
            if(value!=null && roamCoro!=null){
                fsm.StopCoroutine(roamCoro);
                roamCoro=null;
            }else if(value==null && roamCoro==null){
                roamCoro = fsm.StartCoroutine(Roam(fsm.info.planet));
            }
            if(planet!=null) planet.RemoveMoonFromChild(childIndex);
            planet=value;
        }
    }
    public Moon(FSM fsm): base(fsm){

    }
    public override void OnEnter()
    {
        InitOnEnter(fsm.info.moon);
    }

    public override void OnExit()
    {
        base.OnExit();
        if(planet!=null) Planet=null;
    }

    public override void OnUpdate()
    {
        
    }
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        FSM f = collision.gameObject.GetComponent<FSM>();
        switch(f.curStateType){
            case StateType.Star:
                if(planet!=null)
                    Planet=null;
                fsm.SwitchTo(StateType.Nebula);
                break;
        }
    }
}