using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerController : MonoBehaviourPun
{
    public Pawn player;
    public enum KickState { Dribble, LightKick, HeavyKick};
    Plane GroundPlane;
    Camera MainCamera;

    Vector3 pointOnPlane = new Vector3(-1, 0, 0);

    Vector2 Movement = new Vector2 (0, 0);

    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        //player = GetComponent<Pawn>();
        MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }else
        {
         TurnOnCamera();
        }

        MovePlayer();
        RotateMouse();
        Kick();
    }

    public void TurnOnCamera()
    {
       transform.parent.GetChild(2).gameObject.SetActive(true);
    }

    public void MovePlayer()
    {
        Movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isSprinting = false;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        if (anim.GetBool("IsIdle") == true)
        {
            if (Movement.magnitude > 0.2f)
            {
                anim.SetBool("IsRunning", true);
                anim.SetBool("IsIdle", false);
            }
        }
        else if (anim.GetBool("IsRunning") == true)
        {
            if (Movement.magnitude < 0.9f)
            {
                anim.SetBool("IsRunning", false);
                anim.SetBool("IsIdle", true);

            }
        }else if(anim.GetBool("IsRunning") == false && anim.GetBool("IsIdle") == false)
        {
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsIdle", true);
        }

        /*
        if (Movement.magnitude > 0.9f && anim.GetBool("HeavyKick") == false)
        {
            anim.SetBool("IsRunning", true);
            anim.SetBool("IsIdle", false);
        }else
        {
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsIdle", true);
        }
        */
        
        /*
        else {
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsIdle", true);
        }*/

        Movement = Vector2.ClampMagnitude(Movement, 1);
        player.MovePawn(Movement, isSprinting);
    }

    public void RotateMouse()
    {
        /** GroundPlane = new Plane(Vector3.up, player.tf.position);
         Ray mouseRay = MainCamera.ScreenPointToRay(Input.mousePosition);
         float distanceToPlane;
         GroundPlane.Raycast(mouseRay, out distanceToPlane);
         Vector3 pointOnPlane = mouseRay.GetPoint(distanceToPlane); **/

        //print(Movement);

        if (Mathf.Sqrt(Movement.x * Movement.x + Movement.y * Movement.y) > 0.1f)
        { 
         pointOnPlane = transform.position + new Vector3(Movement.x, 0, Movement.y);
        }    

        if (Movement.magnitude >= 0.1f)
        {
            //Vector3 
            //pointOnPlane = transform.position + new Vector3(Movement.x, 0, Movement.y);
        }
        else {
           // pointOnPlane = transform.position + new Vector3(-1, 0, 0);
        }
        player.RotateTowards(pointOnPlane);
    }

    public void Kick()
    {
        KickState ks = KickState.Dribble;
        
        anim.SetBool("HeavyKick", false);
        if (Input.GetButtonDown("Fire1"))
        {
            ks = KickState.LightKick;
        }
        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetBool("IsRunning", false);
            anim.SetBool("HeavyKick", true);
            ks = KickState.HeavyKick;
            
        }
        player.Kick(ks);
    }
}
