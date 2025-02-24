using UnityEngine;
using System.Collections.Generic;

public class FingerCollision : MonoBehaviour
{

    public Animator animator;

    private Dictionary <string, List<bool>> fingerCollisions = new Dictionary<string, List<bool>>(){
        {"ThumbCollision", new List<bool> {false,false,false}},
        {"IndexCollision", new List<bool> {false,false,false}},
        {"MiddleCollision", new List<bool> {false,false,false}},
        {"RingCollision", new List<bool> {false,false,false}},
        {"PinkyCollision", new List<bool> {false,false,false}}

    };
/*
    public Collider thumbOne;
    public Collider thumbTwo;
    public Collider thumbThree;

    public Collider indexOne;
    public Collider indexTwo;
    public Collider indexThree;

    public Collider middleOne;
    public Collider middleTwo;
    public Collider middleThree;

    public Collider ringOne;
    public Collider ringTwo;
    public Collider ringThree;

    public Collider pinkyOne;
    public Collider pinkyTwo;
    public Collider pinkyThree;*/


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();   
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
        if (collider.name == "ThumbCollision"){
            UpdateFinger("ThumbCollision", value);
        }
        else if (collider.name == "IndexCollider"){
            UpdateFinger("IndexCollision", value);
        }
        else if (collider.name == "MiddleCollider"){
            UpdateFinger("MiddleCollision", value);
        }
        else if (collider.name == "RingCollider"){
            UpdateFinger("RingCollision", value);
        }
        else if (collider.name == "PinkyCollider"){
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
