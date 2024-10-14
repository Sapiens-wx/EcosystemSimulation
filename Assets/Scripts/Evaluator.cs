using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaseFunc
{
    public static float Linear(float x){
        return x;
    }
    public static float EaseInQuint(float x){
        return x * x * x * x * x;
    }
    public static float EaseOutQuad(float x){
        return 1 - (1 - x) * (1 - x);
    }
}
