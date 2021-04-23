using UnityEngine;
using System.Collections;
public class FreeCamera : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 initialRotation;    // Use this for initialization
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Time.deltaTime * 20, Space.Self);
        }
        else
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Time.deltaTime * 20, Space.Self);
        }
        else
         if (Input.GetKey(KeyCode.W))
        {
            transform.Translate((Vector3.forward + Vector3.up) * Time.deltaTime * 20, Space.Self);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate((Vector3.forward + Vector3.up) * Time.deltaTime * -20, Space.Self);
        }
        else
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(Vector3.up * Time.deltaTime * -55, Space.World);
        else
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(Vector3.down * Time.deltaTime * -55, Space.World);
        Vector3 tmp = transform.position;
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Vector3.back.y * 1.2f + transform.position.y >= 1.5)
        {
            transform.Translate(Vector3.forward * 1.2f);
        }
        else
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && Vector3.back.y * 1.2f + transform.position.y <= 25.0)
        {
            transform.Translate(Vector3.back * 1.2f);
        }
    }
}