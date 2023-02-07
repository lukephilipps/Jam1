using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    ParticleSystem ps;
    Rigidbody rb;
    Transform transform;
    SphereCollider sc;
    CinemachineVirtualCamera camera;
    [SerializeField] Transform cameraTarget;
    [SerializeField] float topClamp = 70f;
    [SerializeField] float bottomClamp = -30f;
    [SerializeField] float baseFOV = 40f;
    [SerializeField] float sprintFOV = 60f;
    float camThreshold = .01f;
    float camYaw;
    float camPitch;
    public float sensitivity = 1f;
    float sprintLerpRatio;
    int sprintLerpFrames = 0;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] bool sprinting;
    [SerializeField] float groundedRadius = .28f;
    [SerializeField] bool grounded;
    [SerializeField] LayerMask groundLayers;
    [SerializeField] float jumpPower = 10f;
    float targetRotation;
    [SerializeField] float growScale = 1f;
    [SerializeField] float growScaleCamera = 1f;
    public float curSize = .5f;
    [SerializeField] float minSize = 1f;
    [SerializeField] float maxSize = 100f;
    public bool freezeMovement;

    void Awake()
    {
        camera = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        ps = GetComponentInChildren<ParticleSystem>();
        transform = GetComponent<Transform>();
        sc = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        curSize = transform.localScale.x;
    }

    private void Update()
    {
        if (!freezeMovement)
        {
            HandleSprint();
            HandleJump();
            // HandleSpecials();
        }
    }

    private void FixedUpdate()
    {
        if (!freezeMovement)
        {
            GroundCheck();
            HandleMovement();
            // HandleSizeSpecials();
        }
    }

    void LateUpdate()
    {
        if (!freezeMovement)
        {
            CameraRotation();
        }
    }

    void CameraRotation()
    {
        Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (mouseMovement.magnitude > camThreshold)
        {
            camYaw += mouseMovement.x * sensitivity;
            camPitch += mouseMovement.y * sensitivity;
        }
        camYaw = ClampAngle(camYaw, float.MinValue, float.MaxValue);
        camPitch = ClampAngle(camPitch, bottomClamp, topClamp);

        cameraTarget.rotation = Quaternion.Euler(camPitch, camYaw, 0.0f);
    }

    private void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - sc.radius * transform.localScale.x, transform.position.z);
        grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(new Vector3(0f, jumpPower, 0f), ForceMode.VelocityChange);
        }
    }

    void HandleSizeSpecials()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            ShrinkPurposefully();
        }
    }

    void HandleSprint()
    {
        sprinting = Input.GetKey(KeyCode.LeftShift);

        //for camera fov changing and possibly anything else sprint related
        if (sprinting)
        {
            if (sprintLerpFrames < 60)
            {
                sprintLerpFrames++;
                sprintLerpRatio = (float) sprintLerpFrames * .016f;
                camera.m_Lens.FieldOfView = Mathf.Lerp(baseFOV, sprintFOV, sprintLerpRatio);
            }
        }
        else
        {
            if (sprintLerpFrames > 0) 
            {
                sprintLerpFrames--;
                sprintLerpRatio = (float) sprintLerpFrames * .016f;
                camera.m_Lens.FieldOfView = Mathf.Lerp(baseFOV, sprintFOV, sprintLerpRatio);
            }
        }
    }

    void HandleMovement()
    {
        float targetSpeed = sprinting ? sprintSpeed : moveSpeed;

        //possibly normalize this
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        //if theres player input
        if (inputDirection != Vector3.zero)
        {
            // if (grounded) Grow();
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTarget.eulerAngles.y;
            if (!grounded) targetSpeed *= .33f;
        }
        else 
        {
            if (grounded) rb.velocity = new Vector3(rb.velocity.x * 0.95f, rb.velocity.y, rb.velocity.z * 0.95f);
            targetSpeed = 0f;
        }
                
        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        rb.AddForce(targetDirection.normalized * (targetSpeed));
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    void Grow()
    {
        float currentScale = transform.localScale.x;
        if (currentScale >= maxSize)
        {
            transform.localScale = new Vector3(maxSize, maxSize, maxSize);
            curSize = currentScale;
            return;
        }

        //magic numbers :)
        float growAmount = growScale * rb.velocity.magnitude * .0057f;
        transform.localScale += new Vector3(growAmount, growAmount, growAmount);
        var body = camera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        body.CameraDistance = currentScale * growScaleCamera + 5.5f;
        curSize = transform.localScale.x;
    }

    void Shrink()
    {
        float currentScale = transform.localScale.x;
        if (currentScale <= minSize)
        {
            transform.localScale = new Vector3(minSize, minSize, minSize);
            curSize = currentScale;
            return;
        }

        //magic numbers :)
        float shrinkAmount = growScale * rb.velocity.magnitude * .0057f;
        transform.localScale -= new Vector3(shrinkAmount, shrinkAmount, shrinkAmount);
        var body = camera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        body.CameraDistance = currentScale * growScaleCamera + 5.5f;
        curSize = transform.localScale.x;
    }

    void ShrinkPurposefully()
    {
        float currentScale = transform.localScale.x;
        if (currentScale <= minSize)
        {
            transform.localScale = new Vector3(minSize, minSize, minSize);
            curSize = currentScale;
            return;
        }

        var main = ps.main;
        var shape = ps.shape;
        main.startSize = transform.localScale.x * .35f;
        shape.radius = currentScale * .5f;
        ps.Emit(1);

        //magic numbers :)
        float shrinkAmount = growScale * .05f;
        transform.localScale -= new Vector3(shrinkAmount, shrinkAmount, shrinkAmount);
        var body = camera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        body.CameraDistance = currentScale * growScaleCamera + 5.5f;
        curSize = transform.localScale.x;
    }

    public void GrowAmount(float amount)
    {
        float currentScale = transform.localScale.x;
        if (currentScale >= maxSize)
        {
            transform.localScale = new Vector3(maxSize, maxSize, maxSize);
            curSize = currentScale;
            return;
        }

        var main = ps.main;
        var shape = ps.shape;
        main.startSize = transform.localScale.x * .35f;
        shape.radius = currentScale * .5f;
        ps.Emit(10);

        transform.localScale += new Vector3(amount, amount, amount);
        foreach(Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            if (child != transform) 
            {
                child.localScale -= new Vector3(amount, amount, amount);
                if (child.localScale.x < 0)  child.localScale = Vector3.zero;
                child.position = Vector3.MoveTowards(child.position, transform.position, amount);
            }
        }
        var body = camera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        body.CameraDistance = currentScale * growScaleCamera + 5.5f;
        curSize = transform.localScale.x;
        groundedRadius *= 1f + (amount * .5f);
    }

    public void WinSequence()
    {
        ps.Emit(15);
        freezeMovement = true;
        rb.isKinematic = true;
    }
}
