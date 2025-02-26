using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimController : MonoBehaviour


//https://www.youtube.com/watch?v=DLXLT6YCD6c anim/rig
//https://www.youtube.com/watch?v=RwGIyRy-Lss physics movement

{
    //animation
    public InputActionReference gripInput;
    public InputActionReference triggerInput;
    public InputActionReference indexInput;
    public InputActionReference thumbInput;

    public InputActionReference mainButton;

    public InputActionReference moveButton;
    public bool isMoving = false;

    public bool transitionTeleport = false;


    public LayerMask ignoreLayer;

    public float positionSmoothing = 1f;
    public float rotationSmoothing = 1f;
    public float verticalSmoothing = 0.9f;


    public float indexTouch;
    public float thumbTouch;
    public float grip;
    public float trigger;
    private Animator animator;




    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 velocity;
    private Vector3 angularVelocity;




    //physics movement
    [SerializeField] private GameObject followObject;
    [SerializeField] private float followSpeed = 30f;
    [SerializeField] private float rotateSpeed = 100f;

    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;

    private Transform _followTarget;
    public Rigidbody _body;


    public Vector3 targetPositionUpdate;

    public bool gunLoaded = false;


    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletVelocity;

    //bool isCollision = false;


    private void Awake(){
        animator = GetComponent<Animator>();
    }



    private void Start(){
        _followTarget = followObject.transform;
        _body = GetComponent<Rigidbody>();
        _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _body.interpolation = RigidbodyInterpolation.Interpolate;
        _body.mass = 20f;

        //teleport hands at start
        _body.position = _followTarget.position;
        _body.rotation = _followTarget.rotation;

        lastPosition = transform.position;
        lastRotation = transform.rotation;


    }

    // Update is called once per frame
    void LateUpdate()
    {
        //_body.detectCollisions = false;
   
        isMoving = moveButton.action.ReadValue<Vector2>().magnitude > 0.1f;
        /*
        if(isMoving){
            _body.isKinematic = true;
            transform.position = _followTarget.position;
            transform.rotation = _followTarget.rotation;

        }else{
            _body.isKinematic = false;
            transitionTeleport = true;
            PhysicsMove();
        }*/
            PhysicsMove();
        

            if (!animator) return;
            grip = gripInput.action.ReadValue<float>();
            trigger = triggerInput.action.ReadValue<float>();

            indexTouch = indexInput.action.ReadValue<float>();
            thumbTouch = thumbInput.action.ReadValue<float>();

            animator.SetFloat("Grip", grip);
            animator.SetFloat("Trigger", trigger);
            animator.SetFloat("Index", indexTouch);
            animator.SetFloat("Thumb", thumbTouch);
        
        

        if (grip > 0.9f && trigger < 0.1f && indexTouch < 0.1f && thumbTouch < 0.1f){
            //Debug.Log("Gun loaded");
            gunLoaded = true;
        }

        if (grip < 0.9f || trigger >= 0.1f || indexTouch >= 0.1f){
            //Debug.Log("gun not loaded");
            gunLoaded = false;
        }
        /*
        if (mainButton.action.IsPressed()){
            //Debug.Log("Pressed MainButton");
            //Debug.Log(gunLoaded);
            
        }*/

        if (gunLoaded && mainButton.action.IsPressed()){
            //Debug.Log("Shooting");
            FingerShoot();
            gunLoaded = false;
        }
            
    }

    private void PhysicsMove(){
        //position

        var positionWithOffset = _followTarget.position + positionOffset;
        var rotationWithOffset = _followTarget.rotation * Quaternion.Euler(rotationOffset);
        var distance = Vector3.Distance(positionWithOffset, transform.position);

/*
        if (isMoving){
            targetPositionUpdate = positionWithOffset;
            var smoothedVelocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);
            smoothedVelocity.y = smoothedVelocity.y*verticalSmoothing;
            _body.linearVelocity = smoothedVelocity;

            //rotation
            var q = rotationWithOffset * Quaternion.Inverse(_body.rotation);
            q.ToAngleAxis(out float angle, out Vector3 axis);
            _body.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed);

        }else{
            targetPositionUpdate = positionWithOffset;
            _body.linearVelocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);

            //rotation
            var q = rotationWithOffset * Quaternion.Inverse(_body.rotation);
            q.ToAngleAxis(out float angle, out Vector3 axis);
            _body.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed);
        }*/
        targetPositionUpdate = positionWithOffset;
        _body.linearVelocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);

        //rotation
        var q = rotationWithOffset * Quaternion.Inverse(_body.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        _body.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed);
        lastPosition = positionWithOffset;
        lastRotation = rotationWithOffset;
/*
        if (transitionTeleport){
            Debug.Log("kek");
            transform.position = _followTarget.position;
            transform.rotation = _followTarget.rotation;

            transitionTeleport = false;
        }*/

        
        if (isMoving){
        //if (isMoving && !isCollision){
            Vector3 temp = transform.position;
            temp.y = _followTarget.position.y;
            transform.position = temp;
            //transform.rotation = _followTarget.rotation;

            //_body.linearVelocity = Vector3.zero;
            //_body.linearVelocity = Vector3.zero;
        }

    }

    private void FingerShoot(){
        
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();

        bulletRigid.AddForce(bulletSpawnPoint.forward * bulletVelocity, ForceMode.Impulse);
    }
    /*
    private void OnCollisionEnter(Collision collision){
        if ((ignoreLayer & (1 << collision.gameObject.layer)) == 0){
            isCollision = true;
        }else{
            Debug.Log("toinen kasi");
        }
        
    }
    private void OnCollisionExit(Collision collision){
        if ((ignoreLayer & (1 << collision.gameObject.layer)) == 0){
            isCollision = false;
        }
    }*/
}
