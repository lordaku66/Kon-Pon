using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// KonPon 2021
/// 
/// This script controls the behavior of draggable objects in click and drag events.
/// The SetDragged and SetDropped methods are subscribed to the OnUIDrag and OnUIDrop delegates in the input manager.
/// 
/// Author: Jacques Visser
/// </summary>

public class KarutaCard : MonoBehaviour
{
    public Vector3 startPos;
    public bool dragged;
    public bool droppped;
    public bool submitted;
    private RectTransform rectTransform;
    private UIManager ui_manager;
    private Vector3 lastMousePos;
    public Transform dropPosition;
    [SerializeField] private float pickUpAnimationSpeed = 0.1f;
    [SerializeField] private LeanTweenType pickUpEase = LeanTweenType.easeInOutSine;

    List<RaycastResult> hitObjects = new List<RaycastResult>();

    private void Awake()
    {
        ui_manager = FindObjectOfType<UIManager>();
    }

    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;

        ui_manager.OnUIDrag += SetDragged;
        ui_manager.OnUIDrop += SetDropped;

        transform.SetAsFirstSibling();
        LeanTween.scale(rectTransform, new Vector3(0.9f, 0.9f, 1f), 0.2f).setLoopPingPong(1);
    }

    public void SetDragged()
    {
        if (gameObject == ui_manager.selectedKarutaCard)
        {
            // Debug.Log(gameObject.name + " Dragged");
            dragged = true;
            Drag();
        }
    }

    public void SetDropped()
    {
        if (gameObject == ui_manager.selectedKarutaCard)
        {
            // Debug.Log(gameObject.name + " Dropped");
            Drop();
            dragged = false;
        }
    }

    private void Drag()
    {
        droppped = false;
        dropPosition = null;

        Vector3 grow = new Vector3(2f, 2f, 1);
        rectTransform.LeanRotateZ(0, pickUpAnimationSpeed);
        rectTransform.LeanScale(grow, pickUpAnimationSpeed).setEase(pickUpEase);

        transform.SetSiblingIndex(transform.parent.childCount - 2);
        // Debug.Log(name + "'s sibling index is " + transform.GetSiblingIndex().ToString());
    }

    private void Drop()
    {
        dropPosition = GetDropTransformUnderMouse();

        if (dropPosition != null)
        {
            droppped = true;
            submitted = true;
        }
        else
        {
            droppped = false;
        }

        Vector3 normalSize = Vector3.one;
        rectTransform.LeanScale(normalSize, 0.2f);
        rectTransform.LeanRotateZ(-45, pickUpAnimationSpeed);
    }

    private void Update()
    {
        var view = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        bool offScreen = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;

        if (dragged && !offScreen)
        {
            if (ui_manager.selectedKarutaCard != null)
            {
                Vector2 dragVector = Input.mousePosition - lastMousePos;

                rectTransform.anchoredPosition += dragVector;
            }
        }

        if (droppped && !offScreen)
        {
            rectTransform.anchoredPosition = dropPosition.localPosition + new Vector3(0, 100, 0);
        }

        lastMousePos = Input.mousePosition;
    }

    private void OnDisable()
    {
        ui_manager.OnUIDrag -= SetDragged;
        ui_manager.OnUIDrop -= SetDropped;
    }

    private GameObject GetObjectUnderMouse()
    {
        var pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        EventSystem.current.RaycastAll(pointer, hitObjects);

        if (hitObjects.Count <= 0) return null;

        return hitObjects[0].gameObject;
    }

    private Transform GetDropTransformUnderMouse()
    {
        GameObject dropBox = GetObjectUnderMouse();

        if (dropBox != null && dropBox.tag == "Drop Zone")
        {
            return dropBox.transform;
        }
        return null;
    }
}
