using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform m_target;

    public Vector3 m_offset;

    public bool m_useOffsetValues;

    public float m_rotationSpeed;

    public Transform m_pivot;

    public float m_maxViewAngle = 65f;
    public float m_minViewAngle = -60f;

    public bool m_invertY;

    private float m_translateY;
    private float m_waveSlice;
    private float m_timer = 0f;
    public float m_bobbingSpeed = 0.18f;
    public float m_bobbingY = 0.2f;
    public float m_midPoint = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if(!m_useOffsetValues)
        {
            m_offset = m_target.position - transform.position;
        }

        m_pivot.transform.position = m_target.transform.position;
        m_pivot.transform.parent = m_target.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
       
        //Get the X position of the mouse & rotate the camera
        float horizontal = Input.GetAxis("Mouse X") * m_rotationSpeed;
        m_target.Rotate(0, horizontal, 0);
        //m_pivot.Rotate(0, horizontal, 0);

        //Get the Y position of the mouse & rotate the pivot
        float vertical = Input.GetAxis("Mouse Y") * m_rotationSpeed;
        if (m_invertY)
        {
            m_pivot.Rotate(vertical, 0, 0);
        }
        else
        {
            m_pivot.Rotate(-vertical, 0, 0);
        }

        //Limit up/down camera rotation
        if (m_pivot.rotation.eulerAngles.x > m_maxViewAngle && m_pivot.rotation.eulerAngles.x < 180f )
        {
            m_pivot.rotation = Quaternion.Euler(m_maxViewAngle, 0, 0);
        }

        if(m_pivot.rotation.eulerAngles.x > 180f && m_pivot.rotation.eulerAngles.x < 360f + m_minViewAngle)
        {
            m_pivot.rotation = Quaternion.Euler(360f + m_minViewAngle, 0, 0);
        }

        //Move the Camera based on the current position on the target & the original offset
        float yAngle = m_target.eulerAngles.y;
        float xAngle = m_pivot.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(xAngle, yAngle, 0);

        transform.position = m_target.position - (rotation * m_offset);
        //transform.position = m_target.position - m_offset;

        m_waveSlice = 0f;
        if (Mathf.Abs(Input.GetAxis("Horizontal")) == 0 && Mathf.Abs(Input.GetAxis("Vertical")) == 0)
        {
            m_timer = 0f;
        }
        else
        {
            m_waveSlice = Mathf.Sin(m_timer);
            m_timer = m_timer + m_bobbingSpeed;

            if (m_timer > Mathf.PI * 2)
            {
                m_timer = m_timer - (Mathf.PI * 2);
            }
        }
        if (m_waveSlice != 0f)
        {
            m_translateY = (m_waveSlice * m_bobbingY) * Mathf.Clamp((Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"))), 0f, 1f);
            transform.position = new Vector3(transform.position.x, m_midPoint + m_translateY, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, m_midPoint, transform.position.z);
        }

        //Stop the angle of camera
        if (transform.position.y < m_target.position.y)
        {
            transform.position = new Vector3(transform.position.x, m_target.position.y - 0.5f, transform.position.z);
        }
        transform.rotation = rotation;
        //transform.rotation = Quaternion.Euler(m_pivot.rotation.x, m_target.transform.rotation.y, m_target.transform.rotation.z);
        //transform.LookAt(m_target);
    }
}
