using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputAction MoveAction;

    public float walkSpeed = 1.0f;
    public float turnSpeed = 20f;

    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    Animator m_Animator;
    AudioSource m_AudioSource;

    public GameObject shieldPrefab;
    public GameObject Player;
    public bool hasUsedShield = true;
    void Start ()
    {
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_Animator = GetComponent<Animator> ();
        m_AudioSource = GetComponent<AudioSource> ();
        MoveAction.Enable();
        hasUsedShield = true;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shield")
        {
            hasUsedShield = false;
            shieldPrefab.SetActive(true);
            Destroy(other.gameObject);
            InvokeRepeating("CreateShieldParticle", 0f, 1.5f);
        }
        if (hasUsedShield == false && other.tag == "Ghost")
        {
            Destroy(other.transform.parent.gameObject);
            Destroy(other.gameObject);
            shieldPrefab.SetActive(false);
            hasUsedShield = true;
            CancelInvoke("CreateShieldParticle");
        }
    }
    void CreateShieldParticle()
    {
        Instantiate(shieldPrefab, Player.transform.position + new Vector3(0, 0.7f, 0), Quaternion.identity);
    }
    void FixedUpdate ()
    {
        var pos = MoveAction.ReadValue<Vector3>();
        
        float horizontal = pos.x;
        float vertical = pos.y;
        
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize ();

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward);
        
        m_Rigidbody.MoveRotation (m_Rotation);
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * walkSpeed * Time.deltaTime);

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool ("IsWalking", isWalking);

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }
    }
}