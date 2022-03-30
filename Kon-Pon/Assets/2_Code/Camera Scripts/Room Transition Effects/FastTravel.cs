using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// KonPon 2021
/// 
/// This script ensures the player teleports from the object to which this script is attached to the desired end point.
/// It works with the BlackOutEffect script to ensure the screen fades in and out.
/// 
/// Author: Krishna Thiruvengadam 
/// </summary>
public class FastTravel : MonoBehaviour
{
    // Events to check teleportation
    public static event Action OnTeleport;
    public static event Action OnTeleported;

    // Events to check if player needs to turn at the corner
    public static event Action OnCourtyardCorner;
    public static event Action OnCourtyardCornerExit;

    // Get reference to the destination point and the player
    public Transform endPoint;
    Player thePlayer;

    int leftOfPortal;
    public bool nearTeleport = false;

    // Check teleporting status and time
    bool isTeleporting = false;
    bool fade = false;
    const float maxTime = 1f;
    float timer = 0f;
    bool freeze = false;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is left or right of the portal
        if (thePlayer.transform.position.x < endPoint.transform.position.x)
        {
            leftOfPortal = 1;
        }
        else if (thePlayer.transform.position.x > endPoint.transform.position.x)
        {
            leftOfPortal = -1;
        }

        // If ready to teleport, press E to teleport and start the camera fade
        if (isTeleporting)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                timer = 0f;
                OnTeleport?.Invoke();
                fade = true;
                freeze = true;
            }
        }
        if (!isTeleporting && freeze)
        {
            timer += Time.deltaTime;
            if (timer >= maxTime)
            {
                freeze = false;
            }
        }

        // Start timer when the fade begins
        if (fade)
        {
            timer += Time.deltaTime;
        }

        // When timer is up, you can Fast Travel
        // You cannot Fast Travel when there are enemies nearby
        if (timer >= maxTime && fade)
        {
            if (gameObject.tag == "Turning Corner 1")
            {
                thePlayer.transform.position = endPoint.transform.position + new Vector3(0f, 0.24f, 1f);
                OnCourtyardCorner?.Invoke();
            }
            else if (gameObject.tag == "Turning Corner 2")
            {
                thePlayer.transform.position = endPoint.transform.position + new Vector3(-1f, 0.24f, 0f);
                OnCourtyardCornerExit?.Invoke();
            }
            else
            {
                TeleportPlayer();
            }

            isTeleporting = false;
            fade = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            thePlayer.ShowPrompt();
            isTeleporting = true;
            nearTeleport = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            thePlayer.HidePrompt();
            isTeleporting = false;
            OnTeleported?.Invoke();
            timer = 0f;
            nearTeleport = false;
        }
    }

    public void TeleportPlayer()
    {
        thePlayer.transform.position = endPoint.transform.position + new Vector3(1f * leftOfPortal, 0f, 0f);
    }

    public bool Freeze()
    {
        return freeze;
    }
}
