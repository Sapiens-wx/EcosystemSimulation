using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGImageBehav : MonoBehaviour
{
    public float percent;
    Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cam=Camera.main.transform;
    }
    Vector2 tmpPos;
    void FixedUpdate()
    {
        tmpPos=cam.position;
        tmpPos*=percent;
        transform.position=tmpPos;
    }
}
