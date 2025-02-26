using UnityEngine;
using UnityEngine.InputSystem;

using System.Collections;
using System.Collections.Generic;


public class String : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public Transform topPoint;
    public Transform middlePoint;
    public Transform bottomPoint;

    private bool isGrabbed = false;
    private Transform grabPoint;

    public Transform leftHold;
    public Transform rightHold;

    public float returnSpeed = 10f;

    private Vector3 defaultPosition;


    public InputActionReference rightTriggerAction;
    private bool rightTrigger;

    public InputActionReference leftTriggerAction;
    private bool leftTrigger;

    public Animator anim;

    public float maxPullDistance = 1.0f;

    public Transform arrowPointRight;
    public Transform arrowPointLeft;

    public Transform arrowPointLeftSecondary;
    public Transform arrowPointRightSecondary;
    
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;

    private GameObject currentArrow;
    public float arrowSpeed = 30f;

    private bool shootingRight = false;
    private bool shootingLeft = false;

    public float tiltThreshold = 30f;

    private bool canShoot = false;

    public Transform bow;


    private bool swapped = false;
    private bool tiltedWhere = false;

    private List<Transform> resetPositions =new List<Transform>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInParent<Animator>();

        leftTriggerAction.action.Enable();
        rightTriggerAction.action.Enable();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3;

        defaultPosition = middlePoint.position;

        resetPositions.Add(arrowPointRight);
        resetPositions.Add(arrowPointRightSecondary);
        resetPositions.Add(arrowPointLeft);
        resetPositions.Add(arrowPointLeftSecondary);

        StringUpdate();

    }

    // Update is called once per frame
    void Update()
    {
        SwapSides();
        defaultPosition = (topPoint.position + bottomPoint.position) / 2f;


        float pullDistance = Vector3.Distance(middlePoint.position, defaultPosition);
        float pullValue = Mathf.Clamp01(pullDistance/maxPullDistance);

        anim.SetFloat("Tension",pullValue);


        rightTrigger = rightTriggerAction.action.IsPressed();
        leftTrigger = leftTriggerAction.action.IsPressed();

        if(!rightTrigger && isGrabbed && shootingRight){
            isGrabbed = false;
            if (currentArrow != null){
                canShoot = true;
            }
        }

        if(!leftTrigger && isGrabbed && shootingLeft){
            isGrabbed = false;
            if (currentArrow != null){
                canShoot = true;
            }
        }
    
        if (isGrabbed && grabPoint){
            middlePoint.position = grabPoint.position;
            if (currentArrow == null){
                SpawnArrow();
            }

        }else{
            middlePoint.position = Vector3.Lerp(middlePoint.position, defaultPosition, Time.deltaTime * returnSpeed);
        }

        if (canShoot && currentArrow){
            if (shootingRight && !rightTrigger){
                ShootArrow();
                canShoot = false;
                shootingRight = false;
                shootingLeft = false;
                resetArrowSwapping();
            }else if(shootingLeft && !leftTrigger){
                ShootArrow();
                canShoot = false;
                shootingRight = false;
                shootingLeft = false;
                resetArrowSwapping();
            }
        }

        StringUpdate();

        transform.position = middlePoint.position;
        transform.rotation = middlePoint.rotation;

        if (currentArrow != null){
            currentArrow.transform.position = middlePoint.position;
            if (grabPoint == rightHold){
            currentArrow.transform.LookAt(arrowPointRight);
            }else if(grabPoint == leftHold){
            currentArrow.transform.LookAt(arrowPointLeft);
            }
        }




    }

    private void StringUpdate(){
        lineRenderer.SetPosition(0, topPoint.position);
        lineRenderer.SetPosition(1, middlePoint.position);
        lineRenderer.SetPosition(2, bottomPoint.position);
    }


    private void OnTriggerEnter(Collider other){
        //Debug.Log("COLLISION");

        
        float distanceRight = Vector3.Distance(bow.transform.position, rightHold.position);
        float distanceLeft = Vector3.Distance(bow.transform.position, leftHold.position);

        bool isItRight = distanceRight > distanceLeft;


        if (other.CompareTag("Right") && rightTrigger && !isGrabbed && isItRight){
            //Debug.Log("COLLISION RIGHT");
            isGrabbed = true;
            grabPoint = rightHold;
            shootingRight = true;
            shootingLeft = false;

        }else if(other.CompareTag("Left")&&rightTrigger && !isGrabbed && !isItRight){
                //Debug.Log("COLLISION LEFT");
                isGrabbed = true;
                grabPoint = leftHold;
                shootingLeft = true;
                shootingRight = false;
        }
    }

    private void SpawnArrow(){
        if (currentArrow == null){
            currentArrow = Instantiate(arrowPrefab, middlePoint.position, middlePoint.rotation);
            currentArrow.transform.SetParent(middlePoint);

        }

        if (isGrabbed && grabPoint == rightHold){
            resetArrowSwapping();
            swapped = false;
            tiltedWhere = true;
            SwapSides();
            currentArrow.transform.LookAt(arrowPointRight);


        }else if(isGrabbed && grabPoint == leftHold){
            resetArrowSwapping();
            swapped = false;
            tiltedWhere = false;
            SwapSides();
            currentArrow.transform.LookAt(arrowPointLeft);
        }
    }

    private void ShootArrow(){
        float pullDistance = Vector3.Distance(middlePoint.position, defaultPosition);
        float pullValue = Mathf.Clamp01(pullDistance / maxPullDistance);

        currentArrow.transform.SetParent(null);

        Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
        rb.isKinematic = false;

        Vector3 shootDirection = (arrowSpawnPoint.position - middlePoint.position).normalized;

        if (shootingRight){
            shootDirection = (arrowPointRight.position - middlePoint.position).normalized;
        }

        else if (shootingLeft){
            shootDirection = (arrowPointLeft.position - middlePoint.position).normalized;
        }

        rb.AddForce(shootDirection * pullValue * arrowSpeed, ForceMode.Impulse);
        currentArrow = null;
    }

    private void SwapSides(){
        float bowTilted = bow.localEulerAngles.z;

        if (bowTilted > 180f){
            bowTilted -= 360f;
        }

        //Debug.Log(bowTilted);
        if (bowTilted > tiltThreshold || bowTilted < -tiltThreshold){

            if (!swapped){
                Debug.Log("not swapped");
                if (bowTilted > tiltThreshold && !tiltedWhere){
                    Debug.Log("rilt right, swapping");
                    doSwap();

                    tiltedWhere = true;

                }else if(bowTilted < -tiltThreshold && tiltedWhere){
                    Debug.Log("tilt left, swapping");
                    doSwap();

                    tiltedWhere = false;
                }
                swapped = true;
            }

        }else{
            swapped = false;
        }
    }

    private void doSwap(){
                    Transform temp = arrowPointRight;
                    arrowPointRight = arrowPointRightSecondary;
                    arrowPointRightSecondary = temp;  
                    temp = arrowPointLeft;
                    arrowPointLeft = arrowPointLeftSecondary;
                    arrowPointLeftSecondary = temp;
    }

    private void resetArrowSwapping() {
        arrowPointRight = resetPositions[0];
        arrowPointRightSecondary = resetPositions[1];
        arrowPointLeft= resetPositions[2];
        arrowPointLeftSecondary= resetPositions[3];
    }
}


