using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Atom1 : AtomBase
{
    private bool[] boundMap;
    List<Atom1> connectedAtoms;
    // Start is called before the first frame update
    internal override void Start()
    {
        base.Start();
        id=Atom1Manager.inst.RegisterAtom(this);

        boundMap=new bool[Atom1Manager.inst.atomCount];
        connectedAtoms=new List<Atom1>(Atom1Manager.inst.atomCount);
    }
    void FixedUpdate(){
        HandleBounds();
    }
    void HandleBounds(){
        List<Atom1> closeAtoms=Atom1Manager.inst.AtomsCloseTo(this);
        //break bound between far atoms
        for(int i=0;i<connectedAtoms.Count;){
            if(AtomUtil.DistGreatorThan(transform.position, connectedAtoms[i].transform.position, Atom1Manager.inst.boundDistMax)){
                boundMap[connectedAtoms[i].id]=false;
                connectedAtoms.RemoveAt(i);
            } else ++i;
        }
        if(Atom1Manager.inst.maxBoundNum>connectedAtoms.Count){
            //form bound with close atoms
            foreach(var atom in closeAtoms){
                if(boundMap[atom.id]==false && atom.connectedAtoms.Count<Atom1Manager.inst.maxBoundNum){
                    boundMap[atom.id]=true;
                    connectedAtoms.Add(atom);
                }
            }
        }
        //update bound relative positions
        foreach(var atom in connectedAtoms){
            Vector2 dir=((Vector2)transform.position-(Vector2)atom.transform.position);
            if(dir==Vector2.zero) dir=Vector2.up;
            float dist=dir.magnitude;
            dir/=dist;
            dist=(Atom1Manager.inst.boundDetectDist-dist)/Atom1Manager.inst.boundDetectDist;
            //update position
            rgb.AddForce(dist*dir*Atom1Manager.inst.boundLerpAmount*EaseFunc.EaseInQuint(1-AtomUtil.inst.normedTemperature));
        }
    }
}
