using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom3 : AtomBase
{
    internal override void Start()
    {
        base.Start();
        Atom3Manager.inst.RegisterAtom(this);
    }
}
