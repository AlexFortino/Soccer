using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoom : MonoBehaviour
{
    public Transform target;
    
    public Vector3 boomLength;
    Transform tf;
    // Start is called before the first frame update
    void Start()
    {
        tf = transform.GetChild(2);
    }

    // Update is called once per frame
    void Update()
    {
      //  tf.position = target.position + boomLength;
    }

    void LateUpdate()
    {
        transform.position = transform.parent.GetChild(1).position + new Vector3(0, 8, -8);    
    }
}
