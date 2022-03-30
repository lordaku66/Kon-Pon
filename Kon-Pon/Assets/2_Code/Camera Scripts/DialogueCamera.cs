using UnityEngine;

/// <summary>
/// KonPon 2021
/// 
/// This script controlls the FOV of the camera when the player
/// is interacting with objects. This script makes use of events
/// defined in the input manager to determine when to change the camera'S FOV.
/// 
/// Author: Jacques Visser
/// </summary>

public class DialogueCamera : MonoBehaviour
{
    public GameObject activeCamera;
    public GameObject walkingCamera;
    public GameObject talkingCamera;
    private InputManager inputManager;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    private void OnEnable()
    {
        inputManager.OnInteractableClicked += IncreaseFOV;
        inputManager.OnDialogueExit += DecreaseFOV;
    }

    private void DecreaseFOV()
    {
        walkingCamera.SetActive(true);
        talkingCamera.SetActive(false);
    }

    private void IncreaseFOV()
    {
        walkingCamera.SetActive(false);
        talkingCamera.SetActive(true);
    }

    private void OnDisable()
    {
        inputManager.OnInteractableClicked -= IncreaseFOV;
        inputManager.OnDialogueExit -= DecreaseFOV;
    }
}