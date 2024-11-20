using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    public GameObject objPrefab;
    [Header("Parameters")]
    public int numOfObjects;
    public float universeRadius;
    public float collapseInterval;
    public float collapseForce;
    public float collapseAddForceDuration;
    [Header("PostProcess")]
    public Material postProcMat;
    public float duration;
    public float distortAMP, bangPow, collapsePow;

    [HideInInspector] public List<FSM> objs;
    void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(Vector3.zero, universeRadius);
    }
    void Awake(){
        inst=this;
    }
    void Start(){
        BigBang();
        StartCoroutine(BangAnim(distortAMP,0));
        StartCoroutine(Collapse());
    }
    void BigBang(){
        /*
        objs=new List<FSM>(numOfObjects);
        for(int i=0;i<numOfObjects;++i){
            Vector2 pos=Random.insideUnitCircle*universeRadius;
            GameObject go=Instantiate(objPrefab, pos,Quaternion.identity);
            FSM fsm=go.GetComponent<FSM>();
            fsm.Init();
            objs.Add(fsm);
        }*/
        objs=new List<FSM>(numOfObjects);
        for(int i=0;i<numOfObjects;++i){
            Vector2 pos=Random.insideUnitCircle*universeRadius;
            FSM fsm=PoolManager.inst.pool.Get();
            GameObject go=fsm.gameObject;
            go.transform.position=pos;
            go.name=$"obj [{i}]";
            fsm.id=i;
            fsm.Init();
            fsm.SwitchTo((StateType)Random.Range(1,(int)StateType.MAX));
            objs.Add(fsm);
        }
    }
    public void OnObjDestroyed(){
        Vector2 pos=Random.insideUnitCircle*universeRadius;
        FSM fsm=PoolManager.inst.pool.Get();
        AddObj(fsm);
        GameObject go=fsm.gameObject;
        go.name=$"obj [{fsm.id}]";
        go.transform.position=pos;
        fsm.Init();
        StateType newObjType=(StateType)Random.Range(1, (int)StateType.MAX);
        fsm.SwitchTo(newObjType);
    }
    public void AddObj(FSM obj){
        obj.id=objs.Count;
        objs.Add(obj);
    }
    public void RemoveObj(int id){
        if(id==objs.Count-1){
            objs.RemoveAt(objs.Count-1);
            return;
        }
        objs[id]=objs[^1];
        objs[id].id=id;
        objs.RemoveAt(objs.Count-1);
    }
    public List<FSM> ObjCloserThan(FSM self, float dist, System.Func<FSM,bool> predicate=null){
        dist*=dist;
        List<FSM> ret=new List<FSM>();
        for(int i=0;i<objs.Count;++i){
            if(objs[i]==null){
                Debug.LogError($"obj[{i}] is null");
            }
            if(self!=objs[i]){
                if(predicate==null || predicate(objs[i])){
                    Vector2 offset=self.transform.position-objs[i].transform.position;
                    if(offset.x*offset.x+offset.y*offset.y<dist){
                        ret.Add(objs[i]);
                    }
                }
            }
        }
        return ret;
    }
    void OnRenderImage(RenderTexture source, RenderTexture destine){
        Graphics.Blit(source, destine, postProcMat);
    }
    void DestroyAllObjs(){
        for(int i=objs.Count-1;i>=0;--i){
            PoolManager.inst.pool.Release(objs[i]);
        }
        objs.Clear();
    }
    IEnumerator BangAnim(float startAmp, float endAmp){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        float t=0;
        while(t<duration){
            float nt=t/duration;
            nt=Mathf.Lerp(startAmp, endAmp, Mathf.Pow(nt, bangPow));
            postProcMat.SetFloat("_amp", nt);
            t+=Time.fixedDeltaTime;
            yield return wait;
        }
        postProcMat.SetFloat("_amp", endAmp);
    }
    IEnumerator CollapseAnim(float startAmp, float endAmp){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        float t=0;
        while(t<duration){
            float nt=t/duration;
            nt=Mathf.Lerp(startAmp, endAmp, Mathf.Pow(nt, collapsePow));
            postProcMat.SetFloat("_amp", nt);
            t+=Time.fixedDeltaTime;
            yield return wait;
        }
        postProcMat.SetFloat("_amp", endAmp);
    }
    IEnumerator Collapse(){
        yield return new WaitForSeconds(collapseInterval);
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        int numFrames=(int)(collapseAddForceDuration/Time.fixedDeltaTime);
        //set objs' rigidbodies' bodytype to dynamic
        foreach(FSM f in objs){
            f.rgb.bodyType=RigidbodyType2D.Dynamic;
        }
        //add force to all objs
        for(;numFrames>0;--numFrames){
            foreach(FSM f in objs){
                f.rgb.AddForce((-f.transform.position).normalized*collapseForce);
            }
            yield return wait;
        }
        //begins to collapse (post processing)
        IEnumerator collapseAnim=CollapseAnim(0,distortAMP);
        while(collapseAnim.MoveNext())
            yield return collapseAnim.Current;
        DestroyAllObjs();
        yield return new WaitForSeconds(1);
        BigBang();
        IEnumerator bangAnim=BangAnim(distortAMP,0);
        while(bangAnim.MoveNext())
            yield return bangAnim.Current;
        StartCoroutine(Collapse());
    }
}
