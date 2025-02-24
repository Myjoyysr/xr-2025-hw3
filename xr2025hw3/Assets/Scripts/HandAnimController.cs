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

    //public InputActionReference moveButton;
    //public bool isMoving = false;



    public float indexTouch;
    public float thumbTouch;
    public float grip;
    public float trigger;
    private Animator animator;

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

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //_body.detectCollisions = false;
/*    
        isMoving = moveButton.action.ReadValue<Vector2>().magnitude > 0.1f;

        if(isMoving){
            _body.isKinematic = true;

            transform.position = _followTarget.position;
            transform.rotation = _followTarget.rotation;


        }else{
            _body.isKinematic = false;
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
        targetPositionUpdate = positionWithOffset;
        var distance = Vector3.Distance(positionWithOffset, transform.position);
        _body.linearVelocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);

        //rotation
        var rotationWithOffset = _followTarget.rotation * Quaternion.Euler(rotationOffset);
        var q = rotationWithOffset * Quaternion.Inverse(_body.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);

        _body.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed);
    }

    private void FingerShoot(){
        
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();

        bulletRigid.AddForce(bulletSpawnPoint.forward * bulletVelocity, ForceMode.Impulse);
    }
}
