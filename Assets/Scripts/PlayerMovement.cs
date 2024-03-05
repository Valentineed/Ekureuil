using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody m_body;

    public CharacterController m_controller;
    float m_movSpeed = 25f;
    public float m_jumpForce = 35f;
    public float m_gravityScale = 10f;
    bool m_wallJump = false;

    public float m_distRay;
    float m_distFront = 0.7f;
    public float m_airTime = 0f;
    Vector3 m_dirRayDow;
    Vector3 m_vecFrontDow;
    Vector3 m_vecFrontUp;
    Vector3 m_direction;
    public float magnitude;
    public float m_jumpWallForce = 30;
    int m_Jump = 2;

    //float m_airSpeed = 20f;


    void Start()
    {
        m_controller = GetComponent<CharacterController>();
        m_body = GetComponent<Rigidbody>();
        m_distRay = 1.115f;
        m_dirRayDow = new Vector3(0, -1, 0);

        m_vecFrontDow = new Vector3(0f, -1f, 0f);
        m_vecFrontUp = new Vector3(0f, 1f, 0f);
        m_direction = Vector3.zero;
    }

    void Update()
    {

        //DirLook();
        PlayerMove();



        if (m_controller.isGrounded)
        {
            m_Jump = 2;
            m_wallJump = false;
            m_direction.y = 0f;
          
        }
        if (Input.GetButtonDown("Jump") && m_Jump > 0)
        {
            m_Jump--;
            m_direction.y = m_jumpForce;
        }





        //DEBUG
        Debug.DrawRay(transform.position, m_dirRayDow * m_distRay, Color.red);
        Debug.DrawRay(transform.position + m_vecFrontDow, transform.forward * m_distFront, Color.green);
        Debug.DrawRay(transform.position + m_vecFrontUp, transform.forward * m_distFront, Color.green);

        m_direction.y = m_direction.y + (Physics.gravity.y * m_gravityScale * Time.deltaTime);
        m_controller.Move(m_direction * Time.deltaTime);




    }


    void PlayerMove()
    {
        //RaycastHit hit;
        if (!Physics.Raycast(transform.position + m_vecFrontUp, transform.forward, m_distFront) ||
           !Physics.Raycast(transform.position + m_vecFrontDow, transform.forward, m_distFront))
        {
            //if(!m_wallJump)
            //   m_direction = new Vector3(Input.GetAxis("Horizontal") * m_movSpeed, m_direction.y, 0f);
            float y = m_direction.y;
            m_direction = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
            m_direction = m_direction.normalized * m_movSpeed;
            m_direction.y = y;
        }
    }

    void DirLook()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            m_body.transform.eulerAngles = new Vector3(0f, -90f);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            m_body.transform.eulerAngles = new Vector3(0f, 90f);
        }
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    WallJump(hit);
    //}

    //void WallJump(ControllerColliderHit hit)
    //{
    //        if (!m_controller.isGrounded && hit.normal.y < 0.1f && hit.transform.gameObject.CompareTag("JumpWall"))
    //        {
    //            if (Input.GetButtonDown("Jump"))
    //            {
    //                m_wallJump = true;
    //                m_direction = hit.normal * m_jumpWallForce;
    //                m_direction.y += m_jumpForce;
    //                if (hit.normal.x > 0f)
    //                {
    //                    m_body.transform.eulerAngles = new Vector3(0f, -90f);
    //                }
    //                else
    //                {
    //                    m_body.transform.eulerAngles = new Vector3(0f, 90f);
    //                }
    //            }
    //        }
    //        else
    //        {
    //             return;
    //        }
    //}

}
