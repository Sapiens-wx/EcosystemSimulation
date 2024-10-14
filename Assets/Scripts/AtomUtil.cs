using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AtomUtil : MonoBehaviour
{
    public Slider slider;
    public float initialTemperature, maxTemperature;

    public static AtomUtil inst;
    [HideInInspector] public float normedTemperature;
    public static float DistanceSqr(Vector2 a, Vector2 b){
        a-=b;
        return a.x*a.x+a.y*a.y;
    }
    public static bool DistGreatorThan(Vector2 pos1, Vector2 pos2, float dist){
        dist*=dist;
        return DistanceSqr(pos1, pos2)>dist;
    }
    void FixedUpdate(){
        normedTemperature=GetTemperature0to1();
    }
    public float GetTemperature0to1(){
        return slider.value/maxTemperature;
    }
    void Awake(){
        inst=this;
        slider.maxValue=maxTemperature;
        slider.minValue=0;
        slider.value=initialTemperature;
    }
}
