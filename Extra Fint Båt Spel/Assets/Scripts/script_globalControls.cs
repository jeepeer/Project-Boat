using UnityEngine;
using UnityEngine.SceneManagement;

public class script_globalControls : MonoBehaviour
{
    [SerializeField] private GameObject circleCamera;
    [SerializeField] private GameObject followCamera;
    [SerializeField] private GameObject sideviewCamera;
    [SerializeField] private GameObject[] cameras;

    [SerializeField] private GameObject controlLegend;
    [SerializeField] private GameObject boat;
    private Quaternion boatOriginalQuaternion;

    private void Start()
    {
        cameras[0].SetActive(true);
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].SetActive(false);
        }

        controlLegend.SetActive(false);

        boatOriginalQuaternion = boat.transform.rotation;
    }

    private void Update()
    {
        // Pause/Unpause Game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1) { Time.timeScale = 0.1f; controlLegend.SetActive(true); }
            else if (Time.timeScale < 1) { Time.timeScale = 1; controlLegend.SetActive(false); }
        }

        if (Input.GetKeyDown(KeyCode.Q) && controlLegend.activeSelf) Application.Quit();

        // Reset Scene
        if (Input.GetKeyDown(KeyCode.L)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Reset Boat Position and Rotation
        if (Input.GetKeyDown(KeyCode.R))
        {
            boat.transform.position = Vector3.zero;
            boat.transform.rotation = boatOriginalQuaternion;
        }

        // Switch Cameras
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCamera(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCamera(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCamera(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchCamera(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SwitchCamera(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SwitchCamera(5);
        if (Input.GetKeyDown(KeyCode.Alpha0)) SwitchCamera(6);
    }

    private void SwitchCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (i == index) cameras[i].SetActive(true);
            else cameras[i].SetActive(false);
        }
    }
}
