using Cinemachine;
using UnityEngine;

public class script_FreelookCamera : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)) transform.position += transform.forward * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow)) transform.position -= transform.forward * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow)) transform.position -= transform.right * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow)) transform.position += transform.right * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space)) transform.position += transform.up * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftControl)) transform.position -= transform.up * speed * Time.deltaTime;
    }
}
