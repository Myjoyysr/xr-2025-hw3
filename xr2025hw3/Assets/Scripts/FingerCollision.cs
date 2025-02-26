using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FingerCollision : MonoBehaviour
{

    public Animator animator;

    public InputActionReference gripInput;
    public InputActionReference triggerInput;
    public InputActionReference thumbInput;


    public float thumbTouch;
    public float grip;
    public float trigger;

    private Dictionary <string, List<bool>> fingerCollisions = new Dictionary<string, List<bool>>(){
        {"ThumbCollision", new List<bool> {false,false,false}},
        {"IndexCollision", new List<bool> {false,false,false}},
        {"MiddleCollision", new List<bool> {false,false,false}},
        {"RingCollision", new List<bool> {false,false,false}},
        {"PinkyCollision", new List<bool> {false,false,false}}

    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void LateUpdate(){
        grip = gripInput.action.ReadValue<float>();
        trigger = triggerInput.action.ReadValue<float>();
        thumbTouch = thumbInput.action.ReadValue<float>();

        if (thumbTouch < 0.1f){
            setFingerFalse("ThumbCollision");
            UpdateFinger("ThumbCollision", false);
        }
        if (trigger < 0.1f){
            setFingerFalse("IndexCollision");
            UpdateFinger("IndexCollision", false);
        }
        if (grip < 0.1f){
            setFingerFalse("MiddleCollision");
            UpdateFinger("MiddleCollision", false);
            setFingerFalse("RingCollision");
            UpdateFinger("RingCollision", false);
            setFingerFalse("PinkyCollision");
            UpdateFinger("PinkyCollision", false);
        }
    }

    void setFingerFalse(string finger){
        var fingerTriggers = fingerCollisions[finger];

        for(int i = 0; i< fingerTriggers.Count; i++){
            fingerTriggers[i] = false;
        }

    }



    private void OnCollisionEnter(Collision other){
        UpdateCollision(other.collider, true);
        Debug.Log("collision");
    }

    private void OnCollisionExit(Collision other){
        UpdateCollision(other.collider, false);
        Debug.Log("Exit Collision");
    }

    private void UpdateCollision(Collider collider, bool value){

        grip = gripInput.action.ReadValue<float>();
        trigger = triggerInput.action.ReadValue<float>();
        thumbTouch = thumbInput.action.ReadValue<float>();


        if (collider.name == "ThumbCollision"){
            if (thumbTouch < 0.1f){
                value = false;
            }
                UpdateFinger("ThumbCollision", value);

        }
        else if (collider.name == "IndexCollider"){
            if (trigger < 0.1f){
                value = false;
            }
                UpdateFinger("IndexCollision", value);

        }
        else if (collider.name == "MiddleCollider"){
            if (grip < 0.1f){
                value = false;
            }
                UpdateFinger("MiddleCollision", value);
            
        }
        else if (collider.name == "RingCollider"){
            if (grip < 0.1f){
                value = false;
                }
                UpdateFinger("RingCollision", value);
        }
        else if (collider.name == "PinkyCollider"){
            if (grip < 0.1f){
                value = false;
            }
                UpdateFinger("PinkyCollision", value);
            
        }else{
            Debug.Log($"NOTHING CORRECT {collider.name}");
        }
        
    }

    private void UpdateFinger(string finger, bool value){
        var collisions = fingerCollisions[finger];


        for (int i = 0; i < collisions.Count; i++){
            if (collisions[i] != value){
                collisions[i] = value;
                break;
            }
        }

        bool isCollisions = collisions.Contains(true);

        Debug.Log($"{finger} {isCollisions}");
        animator.SetBool($"{finger}", isCollisions);
    }
}
