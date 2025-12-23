using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Movement")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;

    [Header("Detection")]
    public LayerMask whatIsWall;
    public float wallRunForce = 200f;
    public float wallJumpUpForce = 7f;
    public float wallJumpSideForce = 12f;
    public float maxWallRunTime = 1.5f;
    private float wallRunTimer;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    [Header("Camera Effects")]
    public float tiltAmount = 5f; // Kamera ne kadar eğilsin
    public float tiltSpeed = 10f;

    private bool isWallRight, isWallLeft;
    private bool isWallRunning;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
        DoCameraTilt(); // Kamera efekti
    }

    private void FixedUpdate()
    {
        if (isWallRunning)
            WallRunningMovement();
    }

    private void CheckForWall()
    {
        // Sağda veya solda duvar var mı?
        isWallRight = Physics.Raycast(transform.position, orientation.right, 1f, whatIsWall);
        isWallLeft = Physics.Raycast(transform.position, -orientation.right, 1f, whatIsWall);
        
        // Inputları al
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Duvardayken zıplama (Wall Jump)
        if (Input.GetKeyDown(KeyCode.Space) && isWallRunning)
        {
            Vector3 wallNormal = isWallRight ? -orientation.right : orientation.right;
            Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

            // Reset velocity Y for consistent jump height
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(forceToApply, ForceMode.Impulse);
        }
    }

    private void StateMachine()
    {
        // İleri gidiyorsak ve yanımızda duvar varsa
        if ((isWallLeft || isWallRight) && verticalInput > 0 && !IsGrounded())
        {
            if (!isWallRunning) StartWallRun();
        }
        else
        {
            if (isWallRunning) StopWallRun();
        }
    }

    private void StartWallRun()
    {
        isWallRunning = true;
        rb.useGravity = false; // Yer çekimini kapat
    }

    private void StopWallRun()
    {
        isWallRunning = false;
        rb.useGravity = true; // Yer çekimini aç
    }

    private void WallRunningMovement()
    {
        // Duvara yapışık kalması için hafif kuvvet
        Vector3 wallNormal = isWallRight ? orientation.right : -orientation.right;
        rb.AddForce(-wallNormal * 10f);

        // İleri doğru it
        rb.AddForce(orientation.forward * wallRunForce * Time.deltaTime);

        // Düşmeyi engelle ama hafif süzül
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    }
    
    // Yerde miyiz kontrolü (Basit Raycast)
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.5f);
    }

    // --- KAMERA EFEKTİ ---
    private void DoCameraTilt()
    {
        float targetTilt = 0;
        if (isWallRunning)
        {
            if (isWallRight) targetTilt = tiltAmount;
            if (isWallLeft) targetTilt = -tiltAmount;
        }

        // Mevcut rotasyonu al, sadece Z eksenini (eğimi) değiştir
        Vector3 currentRot = playerCam.localRotation.eulerAngles;
        float newZ = Mathf.LerpAngle(currentRot.z, targetTilt, Time.deltaTime * tiltSpeed);
        playerCam.localRotation = Quaternion.Euler(currentRot.x, currentRot.y, newZ);
    }
}