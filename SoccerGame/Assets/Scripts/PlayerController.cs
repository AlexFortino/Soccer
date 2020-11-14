using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviourPun
{
    public Pawn player;
    public enum KickState { Dribble, LightKick, HeavyKick};
    Plane GroundPlane;
    Camera MainCamera;

    Vector3 pointOnPlane = new Vector3(-1, 0, 0);

    Vector2 Movement = new Vector2 (0, 0);

    public Animator anim;

    public GameObject ball;

    public bool isKicking = false;
    // Start is called before the first frame update
    void Start()
    {
        //player = GetComponent<Pawn>();
        MainCamera = Camera.main;
        ball = GameObject.FindGameObjectWithTag("Ball");
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
         MovePlayer();
         RotateMouse();
         Kick();
        }


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


            if (Movement.magnitude > 0.2f)
            {
                anim.SetBool("IsRunning", true);
                anim.SetBool("IsIdle", false);

            }else { 
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
        if (isKicking)
        {
           
            ball.transform.Translate(Vector3.forward * Time.deltaTime * 10 * 0.5f);
            
            //ball.transform.parent = transform;

        }
        KickState ks = KickState.Dribble;
        
        anim.SetBool("HeavyKick", false);
        if (Input.GetMouseButtonDown(1))
        {
            ks = KickState.LightKick;
            if (ball.transform.parent != null && isKicking == false && ball.transform.parent == transform)
            {

                isKicking = true;
                ball.transform.eulerAngles = transform.eulerAngles;
                Invoke("CoolDown", 2f);
                anim.SetBool("IsRunning", false);
                anim.SetBool("IsIdle", false);
                anim.SetBool("HeavyKick", true);
                // ball.GetComponent<Rigidbody>().AddForce(Vector3.up * 1000);
            }
            ball.transform.parent = null;

        }
        if (Input.GetMouseButtonDown(0))
        {

            //ks = KickState.HeavyKick;
            

        }
      //  player.Kick(ks);
    }

    public void StopKick()
    {
        isKicking = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ball")
        {
            Debug.Log("Ball Entered");
            ball.transform.parent = transform;
            Debug.Log("Ball Parented");

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Ball")
        {
            Debug.Log("Ball Exited");
            ball.transform.parent = null;

        }
    }

    public void CoolDown()
    {
        isKicking = false;
        
    }
}
