using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Atom1Manager : MonoBehaviour
{
    public static Atom1Manager inst;
    public int atomCount;
    public float boundDetectDist;
    /// <summary>
    /// when the distance btween two atoms is greator than boundDistMax, the bound breaks
    /// </summary>
    public float boundDistMax;
    public float boundLerpAmount;
    public int maxBoundNum;
    List<Atom1> atoms;

    void OnDrawGizmosSelected(){
        Gizmos.color=Color.white;
        Gizmos.DrawWireSphere(transform.position, boundDetectDist);
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position, boundDistMax);
    }
    void Awake(){
        inst=this;
        atoms=new List<Atom1>();
    }
    /// <summary>
    /// returns atom id
    /// </summary>
    /// <param name="atom"></param>
    /// <returns></returns>
    public int RegisterAtom(Atom1 atom){
        atoms.Add(atom);
        return atoms.Count-1;
    }
    public List<Atom1> AtomsCloseTo(Vector2 pos){
        List<Atom1> res=new List<Atom1>();
        float distsqr=boundDetectDist*boundDetectDist;
        for(int i=atoms.Count-1;i>=0;--i){
            if(AtomUtil.DistanceSqr(atoms[i].transform.position, pos)<=distsqr){
                res.Add(atoms[i]);
            }
        }
        return res;
    }
    public List<Atom1> AtomsCloseTo(Atom1 self){
        List<Atom1> res=new List<Atom1>();
        float distsqr=boundDetectDist*boundDetectDist;
        for(int i=atoms.Count-1;i>=0;--i){
            if(atoms[i]!=self && AtomUtil.DistanceSqr(atoms[i].transform.position, self.transform.position)<=distsqr){
                res.Add(atoms[i]);
            }
        }
        return res;
    }
}
