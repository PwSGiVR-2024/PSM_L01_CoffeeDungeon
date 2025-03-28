using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;
    private Transform mainCamera;
    private Vector3 offset;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        offset = player.transform.position - mainCamera.transform.position;
    }

    void Update()
    {
        mainCamera.transform.position = player.transform.position - offset;
    }
}
