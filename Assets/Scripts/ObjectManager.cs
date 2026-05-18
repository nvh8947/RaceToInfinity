using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private GameObject basicShape;
    [SerializeField] private GameObject holderShape;
    [SerializeField] private InputActionReference r_MousePosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDEBUGAddObject(InputValue value)
    {
        Debug.Log(value.Get<float>());
        Vector3 mouseWorldPos = ScreenToWorld(r_MousePosition.action.ReadValue<Vector2>());
        GameObject.Instantiate(basicShape, mouseWorldPos, Quaternion.identity);
    }

    private Vector3 ScreenToWorld(Vector2 screenPos)
    {
        // Z passed to ScreenToWorldPoint = distance from camera to the target plane
        float distanceToGroundPlane = Mathf.Abs(Camera.main.transform.position.z);
        return Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, distanceToGroundPlane));
    }
}
