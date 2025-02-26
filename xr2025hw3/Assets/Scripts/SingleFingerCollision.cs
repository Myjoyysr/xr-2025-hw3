using UnityEngine;
using UnityEngine.InputSystem;

public class SingleFingerCollision : MonoBehaviour
{

    public Animator animator;
    public string colliderName;

    public Transform bone;
    public Quaternion frozenBone;
    public bool frozen = false;

    public InputActionReference controlInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInParent<Animator>();

    }

    void LateUpdate(){

        float inputFloat = controlInput.action.ReadValue<float>();

        if (frozen){
            bone.localRotation = frozenBone;
        }

        if (0.1f > inputFloat){
            frozen = false;
            animator.SetBool(colliderName, false);
        }
    }

    private void OnTriggerEnter(Collider other){
        //Debug.Log($"Collision, {colliderName}");
        float inputFloat = controlInput.action.ReadValue<float>();
        if (inputFloat > 0.1f){
            animator.SetBool(colliderName, true);
            frozenBone = bone.localRotation;
            frozen = true;
        }else{
            animator.SetBool(colliderName, false);
            frozenBone = bone.localRotation;
            frozen = false;
        }
    }
    private void OnTriggerExit(Collider other){
        animator.SetBool(colliderName, false);
        //Debug.Log($"Collision ended, {colliderName}");
        frozen = false;
    }

    private void OnTriggerStay(Collider other){
        //   Debug.Log($"Collision, {colliderName}");
        float inputFloat = controlInput.action.ReadValue<float>();
        if (inputFloat > 0.1f){
            animator.SetBool(colliderName, true);
        }else{
            animator.SetBool(colliderName, false);
        }
    }
}
