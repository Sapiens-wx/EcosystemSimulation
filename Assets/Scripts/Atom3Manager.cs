using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom3Manager : MonoBehaviour
{
    public static Atom3Manager inst;
    public int atomCount;
    public float boundDetectDist;
    /// <summary>
    /// when the distance btween two atoms is greator than boundDistMax, the bound breaks
    /// </summary>
    public float boundDistMax;
    public float boundLerpAmount;
    public int maxBoundNum;
    List<Atom3> atoms;
    void OnDrawGizmosSelected(){
        Gizmos.color=Color.white;
        Gizmos.DrawWireSphere(transform.position, boundDetectDist);
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position, boundDistMax);
    }
    void Awake(){
        inst=this;
        atoms=new List<Atom3>();
    }

    public int RegisterAtom(Atom3 atom){
        atoms.Add(atom);
        return atoms.Count-1;
    }
}
