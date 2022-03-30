using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// KonPon 2021
/// 
/// This class is a reference to an object in KonPon that can be picked up by the player.
/// 
/// 
/// Author: Jacques Visser and Krishna Thiruvengadam
/// </summary>

[System.Serializable]
public class Liftable
{
    public GameObject pickupPrefab;
    public InteractableObject turnInNPC;
    Rigidbody r;

    // Remove this flag when the condition to check quest is ready
    bool gravity = true;

    public void TurnOnGravity()
    {
        gravity = true;
    }
}
