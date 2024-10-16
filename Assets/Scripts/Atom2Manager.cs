using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom2Manager : MonoBehaviour
{
    public static Atom2Manager inst;
    public int atomCount;
    public float boundDetectDist;
    /// <summary>
    /// when the distance btween two atoms is greator than boundDistMax, the bound breaks
    /// </summary>
    public float boundDistMax;
    public float boundLerpAmount;
    public int maxBoundNum;
    List<Atom2> atoms;
    void OnDrawGizmosSelected(){
        Gizmos.color=Color.white;
        Gizmos.DrawWireSphere(transform.position, boundDetectDist);
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position, boundDistMax);
    }
    void Awake(){
        inst=this;
        atoms=new List<Atom2>();
    }

    public int RegisterAtom(Atom2 atom){
        atoms.Add(atom);
        return atoms.Count-1;
    }
}
