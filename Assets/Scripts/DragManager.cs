using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragManager : MonoBehaviour
{

    [SerializeField] private InputActionReference r_LeftButton;
    [SerializeField] private InputActionReference r_RightButton;
    [SerializeField] private InputActionReference r_MousePosition;
    [SerializeField] private InputActionReference r_MouseDelta;
    [SerializeField] private InputActionReference r_MouseScroll;

    [SerializeField] private Camera m_Camera;
    [SerializeField] private float dragSensitivity;
    [SerializeField] private float lerpConstant;
    
    private float camSoftFloor = 10;
    private float camSoftCeiling = 100;
    private Vector3 targetPos;
    private Vector3 targetItemPos;
    private Vector3 dragAnchor;
    private Vector3 itemDragAnchor;
    private Transform currentItem;
    private bool IsMoving;
    private bool IsDragging;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPos = m_Camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"LButton: {r_LeftButton.action.ReadValue<float>()}");
        //Debug.Log($"RButton: {r_RightButton.action.ReadValue<float>()}");
        //Debug.Log($"Mouse Pos: {r_MousePosition.action.ReadValue<Vector2>()}");
        //Debug.Log($"Scroll: {r_MouseScroll.action.ReadValue<Vector2>()}");

        ScrollCam();

        if (IsMoving)
            DragCam();
        if (IsDragging)
            DragItem();

        //Debug.DrawRay(worldPos, Vector2.zero);
    }

    private void ScrollCam()
    {
        // Scroll more when floor < z < ceiling, and less when closer
        float middle = camSoftCeiling - camSoftFloor;
        float scrollMultiplier = Mathf.Abs(middle - targetPos.z) / Mathf.Abs(middle - camSoftFloor);
        targetPos.z += (r_MouseScroll.action.ReadValue<Vector2>().y * 3 * scrollMultiplier);
        targetPos.z = Math.Clamp(targetPos.z, -camSoftCeiling, -camSoftFloor);
        float lerpConst = m_Camera.transform.position.z;
        m_Camera.transform.position = Vector3.Lerp(m_Camera.transform.position, targetPos, lerpConstant * Time.deltaTime);
    }

    private void DragCam()
    {
        Vector3 currentWorld = ScreenToWorld(GetMousePos);
        Vector3 moveVector = dragAnchor - currentWorld;

        m_Camera.transform.position += moveVector;
        targetPos += moveVector;
    }

    private void DragItem()
    {
        Vector3 currentWorld = ScreenToWorld(GetMousePos);
        Vector3 moveVector = itemDragAnchor - currentWorld;

        currentItem.position += moveVector;
    }

    private Vector3 ScreenToWorld(Vector2 screenPos)
    {
        // Z passed to ScreenToWorldPoint = distance from camera to the target plane
        float distanceToGroundPlane = Mathf.Abs(transform.position.z);
        return Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, distanceToGroundPlane));
    }

    public void OnLeftButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Claude assisted

            Vector3 worldPos = ScreenToWorld(GetMousePos);
            //Physics.Raycast(m_Camera.transform.position, worldPos - m_Camera.transform.position, out RaycastHit hit);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider == null)
                return;
            Debug.Log("Dragging: " + hit.collider.transform.gameObject.name);

            currentItem = hit.collider.transform;
            targetItemPos = worldPos;
            itemDragAnchor = ScreenToWorld(GetMousePos);
        }
        else if (context.canceled)
        {
            IsDragging = false;
            Debug.Log("Stopped dragging");
        }
    }

    public void OnRightButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsMoving = true;
            dragAnchor = ScreenToWorld(GetMousePos);
        }
        //else if (context.performed)
        //{
        //    //Debug.Log("Right performed");
        //}
        else if (context.canceled)
        {
            IsMoving = false;
        }
    }

    private Vector3 GetMousePos => r_MousePosition.action.ReadValue<Vector2>();
    //private Vector3 GetMouseWorldPos => m_Camera.ScreenToWorldPoint((Vector3)r_MousePosition.action.ReadValue<Vector2>());
}
