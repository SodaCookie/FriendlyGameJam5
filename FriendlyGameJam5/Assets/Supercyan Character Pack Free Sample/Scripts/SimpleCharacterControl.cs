using UnityEngine;
using System.Collections.Generic;
using System;
using UnityStandardAssets.CrossPlatformInput;

public class SimpleCharacterControl : MonoBehaviour
{
    private enum ControlMode
    {
        Tank,
        Direct
    }

    public bool Sprinting { get { return m_moveSpeed == m_sprintSpeed; } }
    public bool CanSprint { get { return !m_recharging; } }

    [Header("Running")]
    [SerializeField] private float m_sprintSpeed = 4;
    [SerializeField] public float m_sprintMeterMax = 2; // In seconds
    [SerializeField] public float m_sprintRechargeMin = 0;
    [HideInInspector] public float m_sprintMeter; // In seconds
    [HideInInspector] public bool m_recharging = false;
    private float m_walkSpeed;

    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_jumpForce = 4;
    [SerializeField] private Animator m_animator;
    [SerializeField] private Rigidbody m_rigidBody;

    [SerializeField] private ControlMode m_controlMode = ControlMode.Direct;

    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;
    private readonly float m_walkScale = 0.33f;
    private readonly float m_backwardsWalkScale = 0.16f;
    private readonly float m_backwardRunScale = 0.66f;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;

    private bool m_isGrounded;
    private List<Collider> m_collisions = new List<Collider>();

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }

    private void Start()
    {
        m_sprintMeter = m_sprintMeterMax;
        m_walkSpeed = m_moveSpeed;
    }

    void Update()
    {
        m_animator.SetBool("Grounded", m_isGrounded);

        if (CrossPlatformInputManager.GetButton("Sprint") && m_sprintMeter > 0 && !m_recharging)
        {
            m_animator.speed = 1.3f;
            m_sprintMeter -= Time.deltaTime;
            if (m_sprintMeter <= 0) m_recharging = true;
            m_sprintMeter = Mathf.Max(m_sprintMeter, 0);
            m_moveSpeed = m_sprintSpeed;
        }
        else
        {
            m_animator.speed = 1;
            if (!CrossPlatformInputManager.GetButton("Sprint"))
            {
                m_sprintMeter += Time.deltaTime * 0.8f;
            }
            if (m_recharging && m_sprintMeter > m_sprintRechargeMin)
            {
                m_recharging = false;
            }
            m_sprintMeter = Mathf.Min(m_sprintMeter, m_sprintMeterMax);
            m_moveSpeed = m_walkSpeed;
        }

        switch (m_controlMode)
        {
            case ControlMode.Direct:
                DirectUpdate();
                break;

            case ControlMode.Tank:
                TankUpdate();
                break;

            default:
                Debug.LogError("Unsupported state");
                break;
        }

        m_wasGrounded = m_isGrounded;
    }

    private void TankUpdate()
    {
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        float h = CrossPlatformInputManager.GetAxis("Horizontal");

        if (v < 0)
        {
            v *= m_backwardRunScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
        transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

        m_animator.SetFloat("MoveSpeed", m_currentV);

        JumpingAndLanding();
    }

    private void DirectUpdate()
    {
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        float h = CrossPlatformInputManager.GetAxis("Horizontal");

        Transform camera = Camera.main.transform;

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero)
        {
            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

            m_animator.SetFloat("MoveSpeed", direction.magnitude);
        }

        JumpingAndLanding();
    }

    private void JumpingAndLanding()
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && CrossPlatformInputManager.GetButton("Jump"))
        {
            m_jumpTimeStamp = Time.time;
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        if (!m_wasGrounded && m_isGrounded)
        {
            m_animator.SetTrigger("Land");
        }

        if (!m_isGrounded && m_wasGrounded)
        {
            m_animator.SetTrigger("Jump");
        }
    }
}
