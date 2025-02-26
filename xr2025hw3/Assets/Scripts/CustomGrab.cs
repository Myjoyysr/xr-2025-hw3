using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGrab : MonoBehaviour
{
    // This script should be attached to both controller objects in the scene
    // Make sure to define the input in the editor (LeftHand/Grip and RightHand/Grip recommended respectively)
    CustomGrab otherHand = null;
    public List<Transform> nearObjects = new List<Transform>();
    public Transform grabbedObject = null;
    public InputActionReference action;
    bool grabbing = false;

    private Vector3 lastPosition;
    private Quaternion lastRotation;
    //private Vector3 lastLocalPosition;

    public InputActionReference triggerAction;
    private bool trigger;

    public Transform holdPosition;
    public float speed = 100f;

    public InputActionReference moveButton;
    private bool isMoving = false;

    private bool wasMoving = false;

    private void Start()
    {
        action.action.Enable();
        triggerAction.action.Enable();


        // Find the other hand
        foreach(CustomGrab c in transform.parent.GetComponentsInChildren<CustomGrab>())
        {
            if (c != this)
                otherHand = c;
        }
    }
    
    void LateUpdate()
    {
        grabbing = action.action.IsPressed();
        trigger = triggerAction.action.IsPressed();
        isMoving = moveButton.action.ReadValue<Vector2>().magnitude > 0.1f;

        if (grabbing)
        {
            // Grab nearby object
            if (!grabbedObject && nearObjects.Count > 0){
                Transform nearObject = nearObjects[0];
                if (otherHand.grabbedObject != nearObject){
                    grabbedObject = nearObjects[0];
                }
            }
        
            
            if (grabbedObject){
                //"normal" grabbing interaction
                if(!trigger){
                    // 1. calculate the deltas:
                    Vector3 deltaPosition = transform.position - lastPosition;
                    Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(lastRotation);
                    // apply position and rotation
                    grabbedObject.position = grabbedObject.position+ deltaPosition;
                    grabbedObject.rotation = deltaRotation * grabbedObject.rotation;
                    // Set it relative to controller
                    Vector3 controllerVector = grabbedObject.position - transform.position;
                    controllerVector = deltaRotation * controllerVector;
                    grabbedObject.position = transform.position + (controllerVector);
                //we move object to the hold position
                }else{
                    grabbedObject.position = Vector3.MoveTowards(grabbedObject.position, holdPosition.position, speed * 10f* Time.deltaTime);
                    grabbedObject.rotation = Quaternion.RotateTowards(grabbedObject.rotation, holdPosition.rotation, speed * 100f * Time.deltaTime);



                    
                }

                if(isMoving){
                        Debug.Log("isMoving");
                        //Vector3 temp = transform.position;
                        //temp.y = holdPosition.position.y;
                        //grabbedObject.position =  temp;
                        wasMoving = true;
                        grabbedObject.position = holdPosition.position;
                }
                if(wasMoving){
                    wasMoving = false;
                    grabbedObject.position = holdPosition.position;
                }
            }


        }else if (grabbedObject){
            grabbedObject = null;
        }

            // Should save the current position and rotation here
            lastPosition = transform.position;
            lastRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Make sure to tag grabbable objects with the "grabbable" tag
        // You also need to make sure to have colliders for the grabbable objects and the controllers
        // Make sure to set the controller colliders as triggers or they will get misplaced
        // You also need to add Rigidbody to the controllers for these functions to be triggered
        // Make sure gravity is disabled though, or your controllers will (virtually) fall to the ground

        //Debug.Log("Collision");

        Transform t = other.transform;
        if(t && t.tag.ToLower()=="grabbable")
            nearObjects.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Collision EXIT");
        Transform t = other.transform;
        if( t && t.tag.ToLower()=="grabbable")
            nearObjects.Remove(t);
    }
}