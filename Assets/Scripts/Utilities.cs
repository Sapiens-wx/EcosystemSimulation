using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities{
    public static Vector2 RandomDirection(){
        float theta=Random.Range(0,6.28f);
        float cos=Mathf.Cos(theta), sin=Mathf.Sin(theta);
        return new Vector2(-sin, cos);
    }
    public static Vector2 RotateVec2(Vector2 dir, float theta){
        float cos=Mathf.Cos(theta), sin=Mathf.Sin(theta);
        return new Vector2(dir.x*cos-dir.y*sin, dir.x*sin+dir.y*cos);
    }
}