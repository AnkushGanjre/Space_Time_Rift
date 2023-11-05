using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [SerializeField] Vector3 distance = new Vector3(0, -1, 5);
    private float rotationSpeed = 2f;

    [SerializeField] float minVerticalAngle = -10;
    [SerializeField] float maxverticalAngle = 45;

    [SerializeField] Vector2 framingOffset;

    [SerializeField] bool invertX;
    [SerializeField] bool invertY;

    float rotationX;
    float rotationY;

    float invertXVal;
    float invertYVal;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        invertXVal = (invertX) ? -1 : 1;
        invertYVal = (invertY) ? -1 : 1;

        rotationX += Input.GetAxis("Mouse Y") * invertYVal * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxverticalAngle);

        rotationY += Input.GetAxis("Mouse X") * invertXVal * rotationSpeed;
        
        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
        var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);

        transform.position = focusPosition - targetRotation * distance;
        transform.rotation = targetRotation;
    }

    public Quaternion PlanerRotation => Quaternion.Euler(0, rotationY, 0);
}
