using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom2Manager : MonoBehaviour
{
    public static Atom2Manager inst;

    List<Atom2> atoms;
    void Awake(){
        inst=this;
    }

    public int RegisterAtom(Atom2 atom){
        atoms.Add(atom);
        return atoms.Count-1;
    }
}
