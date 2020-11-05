using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.AI;

public class AwayAI : MonoBehaviour
{
    GameObject ball;
    NavMeshAgent enemyAI;

    public Shoes shoe;
    bool hasBall;
    public GameObject goalToShoot;
    public float speed = 4;
    public float lightKick = 1000;
    public float heavyKick = 2500;

    private Vector3 targetPosition;

    
    void Update()
    {
        if (ball == null){
            ball = GameObject.FindGameObjectWithTag("Ball");
        }
        if (goalToShoot == null) {
            goalToShoot = GameObject.FindGameObjectWithTag("Home");
        }
        if (enemyAI == null)
        {
            enemyAI = GetComponent<NavMeshAgent>();
        }

        if (hasBall == false)
        {
            targetPosition = ball.transform.position;
        }
        if (hasBall == true) {
            targetPosition = goalToShoot.transform.position;
            float distance = transform.position.magnitude - goalToShoot.transform.position.magnitude;
            if (Mathf.Abs(distance) <= 13){
                ball.transform.parent = null;
                ball.GetComponent<Rigidbody>().AddForce(transform.forward * heavyKick);
                hasBall = false;
            }
        }

        enemyAI.SetDestination(targetPosition);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball") {
            hasBall = true;
            collision.gameObject.transform.parent = gameObject.transform;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            hasBall = false;
            collision.gameObject.transform.parent = null;
        }
    }
}
