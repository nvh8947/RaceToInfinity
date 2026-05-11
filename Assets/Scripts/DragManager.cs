using UnityEngine;
using UnityEngine.InputSystem;

public class DragManager : MonoBehaviour
{

    [SerializeField] private InputActionReference r_LeftButton;
    [SerializeField] private InputActionReference r_RightButton;
    [SerializeField] private InputActionReference r_MousePosition;
    [SerializeField] private InputActionReference r_MouseScroll;

    [SerializeField] private Camera m_Camera;
    [SerializeField] private float time;
    [SerializeField] private Vector3 targetPos;

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
        Debug.Log($"Scroll: {r_MouseScroll.action.ReadValue<Vector2>()}");

        targetPos.z += (r_MouseScroll.action.ReadValue<Vector2>().y * 3);
        m_Camera.transform.position = targetPos;
        //m_Camera.transform.position = Vector3.MoveTowards(
        //    m_Camera.transform.position, 
        //    targetPos, 
        //    Mathf.Abs(targetPos.z - m_Camera.transform.position.z * Time.deltaTime));

    }

    //public void OnMouseScroll(InputAction.CallbackContext context)
    //{
    //    Debug.Log(context.ReadValue<Vector2>());
    //}
}
