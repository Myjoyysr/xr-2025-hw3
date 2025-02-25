using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RotateBridge : MonoBehaviour
{
    public List<Transform> targets = new List<Transform>();

    public float rotationSpeed = 3f;
    private bool isRotating = false;
    private bool finishedRotation = false;

    private Quaternion targetRotation;

    public GameObject wallBlock;

    private bool isTargets = false;




    // Update is called once per frame
    void Update()
    {

        foreach(Transform target in targets){
            if (target != null){
                isTargets = true;
                break;
            }
            isTargets = false;
        }

        if (!isTargets){

            if (!isRotating && !finishedRotation){
                isRotating = true;
                targetRotation = Quaternion.Euler(transform.eulerAngles.x -90f, transform.eulerAngles.y,transform.eulerAngles.z);
            }

            if (isRotating){
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f){
                    transform.rotation = targetRotation;
                    isRotating = false;
                    finishedRotation = true;

                    if (wallBlock != null){
                        Destroy(wallBlock);
                    }
                }
            }
        }   
    }
}
