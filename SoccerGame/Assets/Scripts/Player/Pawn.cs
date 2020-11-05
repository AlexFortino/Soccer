using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Pawn : MonoBehaviourPun
{
    [Header("Values")]
    public float walkSpeed = 5.0f;
    public float sprintSpeed = 10.0f;
    public float sprintDuration = 2.0f;
    public float rotateSpeed = 90.0f;
    public float lightKick = 10.0f;
    public float heavyKick = 25.0f;

    public Transform tr;

    [Header("Components")]
    public Shoes shoes;
    [HideInInspector]
    public Transform tf;
    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
        
    }

    public void MovePawn(Vector2 movement, bool isSprinting)
    {
        if (isSprinting)
        {
            if (timer < sprintDuration)
            {
                Vector3 moveValues = new Vector3(movement.x, 0, movement.y);
                tf.Translate(moveValues * sprintSpeed * Time.deltaTime, Space.Self);
                timer += Time.deltaTime;
            }
            else
            {
                Vector3 moveValues = new Vector3(movement.x, 0, movement.y);
                tf.Translate(moveValues * walkSpeed * Time.deltaTime, Space.Self);
            }

        }
        else
        {
            Vector3 moveValues = new Vector3(movement.x, 0, movement.y);
            tf.Translate(moveValues * walkSpeed * Time.deltaTime, Space.Self);
            timer -= Time.deltaTime;
            if (timer < 0) { timer = 0; }
        }
    }

    public void RotateTowards(Vector3 target)
    {
        Vector3 vectorToTarget = target - tr.position;
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget);

        tr.rotation = Quaternion.RotateTowards(tr.rotation, targetRotation, rotateSpeed);
    }

    public void Kick(PlayerController.KickState kickState)
    {
        if (shoes.canKick)
        {
            switch (kickState)
            {
                case PlayerController.KickState.HeavyKick:
                    shoes.Kick(heavyKick);
                    break;

                case PlayerController.KickState.LightKick:
                    shoes.Kick(lightKick);
                    break;

                case PlayerController.KickState.Dribble:
                    shoes.Kick(0.0f);
                    break;

                default:
                    shoes.Kick(0.0f);
                    break;
            }
        }
        else
        {
            shoes.Kick(0.0f);
        }
    }
}
