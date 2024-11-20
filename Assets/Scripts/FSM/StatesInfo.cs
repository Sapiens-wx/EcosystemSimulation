using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "info", menuName = "State Info")]
public class StatesInfo : ScriptableObject{
    public Star star;
    public Planet planet;
    public BlackHole blackHole;
    public Moon moon;
    public Nebula nebula;
    [System.Serializable]
    public class ObjBase{
        public Sprite sprite;
        public float scale;
        public float roamSpeed;
    }
    [System.Serializable]
    public class Star : ObjBase{
        public float gravity;
        public float gravityDistance;
        public int childMaxCount;
        public float rotateSpeed;
    }
    [System.Serializable]
    public class Planet : ObjBase{
        public float gravity;
        public float gravityDistance;
        public int childMaxCount;
        public float rotateSpeed;
    }
    [System.Serializable]
    public class BlackHole : ObjBase{
        public float gravity;
        public float gravityDistance;
    }
    [System.Serializable]
    public class Nebula : ObjBase{
    }
    [System.Serializable]
    public class Moon : ObjBase{
    }
}