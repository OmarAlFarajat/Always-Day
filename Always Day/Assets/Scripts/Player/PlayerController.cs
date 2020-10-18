﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 forward, right;
    public Animator animator;

    [SerializeField]
    private float moveSpeed = 4f;
    [SerializeField]
    private float jumpForce = 6f;
    [SerializeField]
    public bool isGrounded = true;
    [SerializeField]
    public bool isStunned = false;

    //For lock-on system
    private GhostController lockOnTarget;
    private StunBar _currentStunBar;
    public StunBar[] ghostsStunBars;
    public float stunBarIncrement;
    public GameObject electricity;

    public Transform currentMovingPlatform;

    void Awake()
    {
        electricity.SetActive(false);

        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        // Make movement directions relative to isometric camera
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        lockOnTarget = null;
    }

    public void Move()
    {
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
        transform.forward = heading;

        if (!isStunned)
        {
            transform.position += rightMovement;
            transform.position += upMovement;
        }
    }
    public void Jump()
    {
        if (isGrounded && !isStunned)
        {
            transform.parent = null;
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isGrounded = false;
            animator.SetBool("isGrounded", false);

        }
    }

    public void GetStunned()
    {
        if (!isStunned)
        {
            isStunned = true;
            Invoke(nameof(ResetStun), 1.5f);
        }
    }

    private void ResetStun()
    {
        isStunned = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            animator.SetBool("isGrounded", true);
            isGrounded = true;
        }

        if (collision.transform.CompareTag("Platform"))
        {
            animator.SetBool("isGrounded", true);
            isGrounded = true;
            currentMovingPlatform = collision.gameObject.transform;
            transform.SetParent(currentMovingPlatform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
            currentMovingPlatform = null;
    }

    public void LockOnToTarget()
    {
        RaycastHit rayHit;
        // Test between raycast and spherecast. 
        //Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit);
        Physics.SphereCast(Camera.main.ScreenPointToRay(Input.mousePosition), 1f, out rayHit);
        if (lockOnTarget = rayHit.collider.GetComponent<GhostController>())
        {
            electricity.SetActive(true);
            electricity.transform.rotation = Quaternion.RotateTowards(electricity.transform.rotation, this.transform.rotation, 10);

            _currentStunBar = lockOnTarget.GetComponentInChildren<StunBar>();
            animator.SetBool("isAttacking", true);
            transform.LookAt(new Vector3(lockOnTarget.transform.position.x, transform.position.y, lockOnTarget.transform.position.z));
            _currentStunBar.StunBarImg.enabled = true;
            _currentStunBar.StunBarProgress(stunBarIncrement * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isAttacking", false);
            electricity.SetActive(false);
            foreach (StunBar stunBar in ghostsStunBars)
            {
                if (!stunBar.StunBarImg)
                {
                    continue;
                }

                stunBar.StunBarImg.fillAmount = 0.0f;
                stunBar.StunBarImg.enabled = false;
            }
        }
    }
}
