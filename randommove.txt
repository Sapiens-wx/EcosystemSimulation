using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class AtomBase : MonoBehaviour
{
    public float speed;
    public float turnForce;
    [HideInInspector] public Rigidbody2D rgb;
    float rand_amp,rand_frq,rand_offset;

    [HideInInspector] public int id;
    // Start is called before the first frame update
    internal virtual void Start()
    {
        rand_amp=UnityEngine.Random.Range(8000f,14000f);
        rand_frq=UnityEngine.Random.Range(8695f,12349f);
        rand_offset=UnityEngine.Random.Range(-1f,1f);
        rgb=GetComponent<Rigidbody2D>();

        StartCoroutine(Roam());
    }
    float Rand(float t){
        t=Mathf.Sin(rand_frq*t+rand_offset)*rand_amp;  
        return (t-Mathf.Floor(t));
    }
    float RandomForce(){
        float t=Time.time;
        float ti=Mathf.Floor(t);
        float tf=t-ti;
        return Mathf.Lerp(Rand(ti),Rand(ti+1),tf)*2-1;
    }
    Vector2 RandomDir(){
        float t=Time.time;
        float ti=Mathf.Floor(t);
        float tf=t-ti;
        float theta=Mathf.Lerp(Rand(ti),Rand(ti+1),tf)*6.28f-3.14f;
        Vector2 dir=Vector2.right;
        float cos=Mathf.Cos(theta), sin=Mathf.Sin(theta);
        return new Vector2(cos*dir.x-sin*dir.y,sin*dir.x+cos*dir.y);
    }
    IEnumerator Roam(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        rgb.velocity=Vector2.right*speed;
        while(true){
            rgb.AddForce(RandomDir()*turnForce*EaseFunc.EaseOutQuad(AtomUtil.inst.normedTemperature));
            yield return wait;
        }
    }
}
