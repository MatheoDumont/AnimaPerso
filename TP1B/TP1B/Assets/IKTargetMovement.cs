using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTargetMovement : MonoBehaviour
{
    public float rotation_speed = 15.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))    
            transform.position -= transform.right;
        else if (Input.GetKey(KeyCode.RightArrow))
            transform.position += transform.right;
        else if (Input.GetKey(KeyCode.UpArrow))
            transform.position += transform.forward;
        else if (Input.GetKey(KeyCode.DownArrow))
            transform.position -= transform.forward;

        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // transform.Rotate(Vector3.up * mouseInput.x * rotation_speed);
        // transform.Rotate(Vector3.up * mouseInput.y * rotation_speed);
        // transform.Rotate(Vector3.up * )

    }
}
