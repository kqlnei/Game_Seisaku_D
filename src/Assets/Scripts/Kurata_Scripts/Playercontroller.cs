using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    public float turnSpeed = 20f;
    public float moveSpeed = 5f;
    public float stoppingTime = 0.1f; // Adjust this value for the desired stopping time

    private Animator m_Animator;
    private Rigidbody m_Rigidbody;
    private Vector3 m_Movement;
    private Quaternion m_Rotation = Quaternion.identity;
    private Vector3 m_Velocity;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = CorrectInputDirection(new Vector3(horizontal, 0f, vertical));
        inputDirection.Normalize();

        m_Movement.Set(inputDirection.x, 0f, inputDirection.z);

        bool hasHorizontalInput = !Mathf.Approximately(inputDirection.x, 0f);
        bool hasVerticalInput = !Mathf.Approximately(inputDirection.z, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("Walking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        Move();
    }

    void Move()
    {
        Vector3 movement = m_Movement * moveSpeed * Time.fixedDeltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);

        // Apply inertia to slow down when there is no input
        if (!m_Movement.Equals(Vector3.zero))
        {
            m_Velocity = m_Movement * moveSpeed;
        }
        else
        {
            m_Velocity = Vector3.Lerp(m_Velocity, Vector3.zero, stoppingTime * Time.fixedDeltaTime);
            m_Rigidbody.MovePosition(m_Rigidbody.position + m_Velocity * Time.fixedDeltaTime);
        }
    }

    void OnAnimatorMove()
    {
        m_Rigidbody.MoveRotation(m_Rotation);
    }

    Vector3 CorrectInputDirection(Vector3 inputDirection)
    {
        float cameraRotationY = Camera.main.transform.rotation.eulerAngles.y;
        Quaternion correction = Quaternion.Euler(0f, cameraRotationY, 0f);
        Vector3 correctedDirection = correction * inputDirection;

        return correctedDirection.normalized;
    }
}
