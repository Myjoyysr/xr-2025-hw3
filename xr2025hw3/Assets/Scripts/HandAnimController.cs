using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimController : MonoBehaviour


//https://www.youtube.com/watch?v=DLXLT6YCD6c 

{
    public InputActionReference gripInput;
    public InputActionReference triggerInput;
    public InputActionReference indexInput;
    public InputActionReference thumbInput;


    public float indexTouch;
    public float thumbTouch;


    public float grip;
    public float trigger;

    private Animator animator;

    private void Awake(){
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator) return;
        grip = gripInput.action.ReadValue<float>();
        trigger = triggerInput.action.ReadValue<float>();

        indexTouch = indexInput.action.ReadValue<float>();
        thumbTouch = thumbInput.action.ReadValue<float>();

        animator.SetFloat("Grip", grip);
        animator.SetFloat("Trigger", trigger);
        animator.SetFloat("Index", indexTouch);
        animator.SetFloat("Thumb", thumbTouch);

    }
}
