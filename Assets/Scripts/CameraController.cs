using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float cameraSpeed = 50f;
    public float vertSpeed = 50f;
    
    private float _xRotation = 40f;
    private float _yRotation = -65f;
	
    // Use this for initialization
    void Start ()
    {
        // _xRotation = transform.rotation.x;
        // _yRotation = transform.rotation.y;
        Cursor.lockState = CursorLockMode.Locked;
    }
	
    // Update is called once per frame
    void Update()
    {
        //Deal with mouse movement
        var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
        
        //Deal with keyboard input
        // var x = Input.GetAxis("Horizontal");
        // var z = Input.GetAxis("Vertical");

        var move = GetMove();
        var vMove = Vector3.zero;

        if (Input.GetKey(KeyCode.Space)) vMove += Vector3.up;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) vMove += Vector3.down;
		
        transform.Translate(Time.deltaTime * (cameraSpeed * move + vertSpeed * vMove));
    }
    
    private Vector3 GetMove() {
        Vector3 move = new Vector3();
        if (Input.GetKey (KeyCode.W)){
            move += Vector3.forward;
        }
        if (Input.GetKey (KeyCode.S)){
            move += Vector3.back;
        }
        if (Input.GetKey (KeyCode.A)){
            move += Vector3.left;
        }
        if (Input.GetKey (KeyCode.D)){
            move += Vector3.right;
        }
        return move.normalized;
    }
}
