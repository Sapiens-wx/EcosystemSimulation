using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum StateType
{
    None,
    Nebula,
    Moon,
    Planet,
    Star,
    BlackHole,
    MAX,
    Asteroid,
    WhiteHole,
}

public abstract class IState{
    public FSM fsm;
    internal Coroutine roamCoro;
    internal Coroutine bornCoro;
    public IState(FSM fsm){
        this.fsm=fsm;
    }
    public abstract void OnEnter();
    internal void InitOnEnter(StatesInfo.ObjBase obj){
        roamCoro=fsm.StartCoroutine(Roam(obj));
        bornCoro=fsm.StartCoroutine(Born(obj));
    }
    public abstract void OnUpdate();
    public virtual void OnExit(){
        if(roamCoro!=null) fsm.StopCoroutine(roamCoro);
        if(bornCoro!=null) fsm.StopCoroutine(bornCoro);
    }
    public virtual void OnCollisionEnter2D(Collision2D collision){}
    public virtual void OnTriggerEnter2D(Collider2D collider){}
    internal IEnumerator Roam(StatesInfo.ObjBase obj){
        WaitForSeconds wait=new WaitForSeconds(.4f);
        while(true){
            Vector2 dir=fsm.rgb.velocity;
            if(fsm.transform.position.magnitude>GameManager.inst.universeRadius){ // if out side of the bound
                dir=obj.roamSpeed*(-(Vector2)fsm.transform.position).normalized;
            } //otherwise
            else if(dir==Vector2.zero){
                dir=Utilities.RandomDirection()*obj.roamSpeed;
            } else{
                dir=Utilities.RotateVec2(dir, Random.Range(-.4f,.4f));
            }
            fsm.rgb.velocity=dir;
            yield return wait;
        }
    }
    internal IEnumerator Born(StatesInfo.ObjBase obj){
        //die animation
        if(fsm.prevStateType!=StateType.None){
            IEnumerator die=Die();
            while(die.MoveNext())
                yield return die.Current;
        }
        //change sprite
        fsm.spr.sprite=obj.sprite;

        //born animation
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        float duration=1;
        int frameCount=(int)(duration/Time.fixedDeltaTime);
        Vector3 scaleVec=new Vector3(0,0,1);
        float dScale=obj.scale/frameCount;
        for(int i=0;i<frameCount;++i){
            fsm.transform.localScale=scaleVec;
            scaleVec.x+=dScale;
            scaleVec.y+=dScale;
            yield return wait;
        }
        fsm.transform.localScale=new Vector3(obj.scale,obj.scale,1);
        bornCoro=null;
    }
    internal IEnumerator Die(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        float duration=1;
        int frameCount=(int)(duration/Time.fixedDeltaTime);
        Vector3 scaleVec=fsm.transform.localScale;
        float dScale=scaleVec.x/frameCount;
        for(int i=0;i<frameCount;++i){
            fsm.transform.localScale=scaleVec;
            scaleVec.x-=dScale;
            scaleVec.y-=dScale;
            yield return wait;
        }
        fsm.transform.localScale=new Vector3(0,0,1);
    }
    internal IEnumerator DieAndDestroy(){
        IEnumerator dieAnim=Die();
        while(dieAnim.MoveNext())
            yield return dieAnim.Current;
        OnExit();
        GameManager.inst.RemoveObj(fsm.id);
        GameManager.inst.OnObjDestroyed();
        PoolManager.inst.pool.Release(fsm);
    }
}