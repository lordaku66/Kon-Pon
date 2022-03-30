using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// KonPon 2021
/// 
/// This class has information about the item stack, which consists of several liftable objects.
/// 
/// Authors: Jacques Visser and Krishna Thiruvengadam
/// </summary>

public class ItemStack : MonoBehaviour
{
    [SerializeField] Liftable item;

    public GameObject prefab;
    private InputManager inputManager;
    private FetchQuestManager questManager;
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] bool inRange = false;
    [SerializeField] private LayerMask whatIsPlayer;
    BoxCollider itemStackCollider;
    Transform playerPos;
    GameObject pickupObject;
    int faceLeft = 1;
    int itemCount = 0;

    private void Awake()
    {
        itemStackCollider = GetComponent<BoxCollider>();
        itemStackCollider.size = new Vector3(interactionRadius, itemStackCollider.size.y, 1f);
        inputManager = FindObjectOfType<InputManager>();
        questManager = FindObjectOfType<FetchQuestManager>();
    }

    private void OnEnable()
    {
        inputManager.OnPickup += SpawnItem;
        inputManager.OnDrop += ReduceItemCount;
        inputManager.OnPickupAttempt += RangeCheck;
    }

    private void SpawnItem()
    {
        if (itemCount == 0 && inputManager.currentItemStack == this.gameObject)
        {
            playerPos = inputManager.GetPlayer().transform;
            pickupObject = Instantiate(prefab, playerPos);
            pickupObject.transform.parent = playerPos;
            questManager.heldItem = pickupObject;
            questManager.heldItemProperites = GetLiftable();
        }

        itemCount++;
    }

    public void ReduceItemCount()
    {
        if (itemCount > 0)
        {
            itemCount--;
            //Destroy(pickupObject);
        }
    }

    void Update()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        if (Horizontal < 0f)
        {
            faceLeft = -1;
        }
        else if (Horizontal > 0f)
        {
            faceLeft = 1;
        }
        // Check if pickup object exists, then set its localPosition
        // To avoid nullreferenceexception
        if (pickupObject)
        {
            pickupObject.transform.localPosition = new Vector3(0.15f * faceLeft, -0.05f, -0.05f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //inRange = true;
            inputManager.currentItemStack = this.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //inRange = false;
            inputManager.currentItemStack = null;
        }
    }

    public bool RangeCheck()
    {
        return inRange;
    }

    public Liftable GetLiftable()
    {
        return item;
    }
}
