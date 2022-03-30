using UnityEngine;

/// <summary>
/// KonPon 2021
/// 
/// Player class - This script contains the rules for how the player moves around
///                and interacts with the environment.
/// 
///  Author: Jacques Visser and Krishna Thiruvengadam
/// </summary>

// TODO: Instatiate Particle FX on Box Turn-In 

public class Player : MonoBehaviour
{
    public float moveSpeed = 1f;
    float moveSpeedX, moveSpeedZ;

    public LeanTweenType easeType;

    // Player's Components
    Rigidbody r;
    SpriteRenderer s;
    Animator a;

    // Manager Dependencies and other classes
    InputManager inputManager;
    UIManager uiManager;
    FetchQuestManager questManager;
    ItemStack itemStack;
    FastTravel fastTravel;
    [SerializeField] GameObject teleportPromptPrefab;
    private GameObject prefabInstance;

    // Flags and checks
    bool moveOnInteract = false;
    float moveTowards;
    public bool isInteracting = false;
    public bool isHoldingItem = false;
    public float interactDist = 0.35f;
    GameObject interactObj;

    Quaternion turnAngle;
    bool turn = false;

    int collFactor = 1;

    GameObject teleporter;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        uiManager = FindObjectOfType<UIManager>();
        questManager = FindObjectOfType<FetchQuestManager>();
        itemStack = FindObjectOfType<ItemStack>();
    }

    private void OnEnable()
    {
        inputManager.OnInteractableClicked += OnInteract;
        inputManager.OnDialogueExit += EndInteraction;
        inputManager.OnPickup += SetPickup;
        inputManager.OnDrop += SetDrop;

        uiManager.OnJournalOpen += LockMovement;

        FastTravel.OnCourtyardCorner += TurnAround;
        FastTravel.OnCourtyardCornerExit += TurnBack;

        FastTravel.OnTeleport += HidePrompt;
    }

    public void SetPickup()
    {
        isHoldingItem = true;
    }

    // Drop object when the player is not near a teleporter
    private void SetDrop()
    {
        if (!fastTravel.nearTeleport || fastTravel == null)
        {
            isHoldingItem = false;
            questManager.DropItem();
        }
    }

    private void OnInteract()
    {
        if (!isInteracting)
        {
            interactObj = inputManager.GetSelectedObject().gameObject;
            Debug.Log("Interacted");
            isInteracting = true;
        }
    }

    public void EndInteraction()
    {
        isInteracting = false;
    }

    public void StartInteraction()
    {
        isInteracting = true;
    }

    public void DisablePlayerMovement()
    {
        r.isKinematic = true;
    }

    public void EnablePlayerMovement()
    {
        r.isKinematic = false;
    }
    void Start()
    {
        r = GetComponent<Rigidbody>();
        s = GetComponent<SpriteRenderer>();
        a = GetComponent<Animator>();

        moveSpeedX = moveSpeed;
        moveSpeedZ = 0f;
        fastTravel = FindObjectOfType<FastTravel>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float Horizontal = Input.GetAxis("Horizontal");

        a.enabled = true;

        if (turn)
        {
            moveSpeedX = 0f;
            moveSpeedZ = moveSpeed;
        }
        else
        {
            moveSpeedX = moveSpeed;
            moveSpeedZ = 0f;
        }

        if (!isInteracting && !fastTravel.Freeze())
        {
            if (Horizontal < 0f)
            {
                //gameObject.LeanRotateY(-180, 0.5f).setEase(easeType);
                r.velocity = new Vector3(-moveSpeedX, 0f, -moveSpeedZ);
                a.SetBool("facingLeft", true);
            }
            else if (Horizontal > 0f)
            {
                //gameObject.LeanRotateY(0, 0.5f).setEase(easeType);
                r.velocity = new Vector3(moveSpeedX, 0f, moveSpeedZ);
                a.SetBool("facingLeft", false);
            }
        }

        if (isInteracting)
        {
            if (!turn)
            {
                if (transform.position.x < interactObj.transform.position.x)
                {
                    moveTowards = interactObj.transform.position.x - interactDist;
                    a.SetBool("facingLeft", false);
                    if (transform.position.x < moveTowards)
                    {
                        r.velocity = new Vector3(moveSpeedX, 0f, moveSpeedZ);
                        a.SetFloat("speed", 1.0f);
                    }
                    else
                    {
                        // Debug.Log("Toro is close enough");
                        a.SetFloat("speed", 0f);
                    }
                }
                else if (transform.position.x > interactObj.transform.position.x)
                {
                    moveTowards = interactObj.transform.position.x + interactDist;
                    a.SetBool("facingLeft", true);
                    if (transform.position.x > moveTowards)
                    {
                        r.velocity = new Vector3(-moveSpeedX, 0f, -moveSpeedZ);
                        a.SetFloat("speed", -1.0f);
                    }
                    else
                    {
                        // Debug.Log("Toro is close enough");
                        a.SetFloat("speed", 0f);
                    }
                }
                else
                    moveTowards = transform.position.x;

            }
            else
            {
                if (transform.position.z < interactObj.transform.position.z)
                {
                    moveTowards = interactObj.transform.position.z - interactDist;
                    a.SetBool("facingLeft", false);
                    if (transform.position.x < moveTowards)
                    {
                        r.velocity = new Vector3(moveSpeedX, 0f, moveSpeedZ);
                        a.SetFloat("speed", 1.0f);
                    }
                    else
                    {
                        // Debug.Log("Toro is close enough");
                        a.SetFloat("speed", 0f);
                    }
                }
                else if (transform.position.z > interactObj.transform.position.z)
                {
                    moveTowards = interactObj.transform.position.z + interactDist;
                    a.SetBool("facingLeft", true);
                    if (transform.position.z > moveTowards)
                    {
                        r.velocity = new Vector3(-moveSpeedX, 0f, -moveSpeedZ);
                        a.SetFloat("speed", -1.0f);
                    }
                    else
                    {
                        // Debug.Log("Toro is close enough");
                        a.SetFloat("speed", 0f);
                    }
                }
                else
                    moveTowards = transform.position.z;
            }
        }

        //s.flipX = a.GetBool("facingLeft");
        if (!isInteracting)
        {
            a.SetFloat("speed", Input.GetAxis("Horizontal"));
        }
    }

    void TurnAround()
    {
        turnAngle.eulerAngles = new Vector3(0f, -90f, 0f);
        r.rotation = turnAngle;
        turn = true;
    }

    void TurnBack()
    {
        turnAngle.eulerAngles = new Vector3(0f, 0f, 0f);
        r.rotation = turnAngle;
        turn = false;
    }

    public void LockMovement()
    {
        this.enabled = false;
    }

    // Temporary Teleport Prompt
    public void ShowPrompt()
    {
        prefabInstance = Instantiate(teleportPromptPrefab, transform);
    }

    public void HidePrompt()
    {
        foreach (GameObject prompt in GameObject.FindGameObjectsWithTag("TelePrompt"))
        {
            Destroy(prompt);
        }
    }

    // To check if player is going to teleport, and then get the teleporter's component
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fast Travel")
        {
            teleporter = other.gameObject;
            fastTravel = teleporter.GetComponent<FastTravel>();
        }
    }
}
