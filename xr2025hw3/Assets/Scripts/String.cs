using UnityEngine;

using UnityEngine.InputSystem;

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
    
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;

    private GameObject currentArrow;
    public float arrowSpeed = 30f;

    private bool shootingRight = false;
    private bool shootingLeft = false;
    

    private bool canShoot = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInParent<Animator>();

        leftTriggerAction.action.Enable();
        rightTriggerAction.action.Enable();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3;

        defaultPosition = middlePoint.position;

        StringUpdate();

    }

    // Update is called once per frame
    void Update()
    {

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
            middlePoint.position = grabPoint.position;/*
            if (currentArrow == null){
                SpawnArrow();
            }*/

        }else{
            middlePoint.position = Vector3.Lerp(middlePoint.position, defaultPosition, Time.deltaTime * returnSpeed);
        }
/*
        if (canShoot && currentArrow){
            if (shootingRight && !rightTrigger){
                ShootArrow();
                canShoot = false;
                shootingRight = false;
                shootingLeft = false;
            }else if(shootingLeft && !leftTrigger){
                ShootArrow();
                canShoot = false;
                shootingRight = false;
                shootingLeft = false;
            }
        }*/

        StringUpdate();

        transform.position = middlePoint.position;
        transform.rotation = middlePoint.rotation;







    }

    private void StringUpdate(){
        lineRenderer.SetPosition(0, topPoint.position);
        lineRenderer.SetPosition(1, middlePoint.position);
        lineRenderer.SetPosition(2, bottomPoint.position);
    }


    private void OnTriggerEnter(Collider other){
        //Debug.Log("COLLISION");
        if (other.CompareTag("Right") && rightTrigger && !isGrabbed){
            //Debug.Log("COLLISION RIGHT");
            isGrabbed = true;
            grabPoint = rightHold;
            shootingRight = true;
            shootingLeft = false;

        }else if(other.CompareTag("Left")&&rightTrigger && !isGrabbed){
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
            currentArrow.transform.LookAt(arrowPointRight);
        }else if(isGrabbed && grabPoint == leftHold){
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
}


