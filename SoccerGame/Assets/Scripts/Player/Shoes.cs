using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoes : MonoBehaviour
{
    public bool canKick = true;
    float kickForce = 0.0f;
    Rigidbody ball;
   // GameObject ball;

    bool isKicking = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (isKicking == true) {
            Debug.Log("kick");
            ball.gameObject.transform.Translate(Vector3.forward * 10.15f * Time.deltaTime);
           }
    }

    void coolDown()
    {
        isKicking = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Ball")
        {
            ball = collision.gameObject.GetComponent<Rigidbody>();
            canKick = true;
          //  Kick(kickForce);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        canKick = false;
        kickForce = 0.0f;
        ball = null;
    }

    public void Kick(float kickValue)
    {
        kickForce = kickValue;
        if (ball != null)
        {
            isKicking = true;
            Invoke("coolDown", 2f);
           // ball.AddForce(transform.forward * kickForce);
        }
    }
}
