using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityRoboticsDemo;
using TMPro;

public class MapClickDetector : MonoBehaviour {
    private Camera cam;
    ROSConnection ros;
    public string topicName = "pos_rot";
    private float resolution = 5f;

    void Start() {
        cam = Camera.main;
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PosRotMsg>(topicName);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 worldPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
            worldPosition.z = 0;
            worldPosition /= resolution;
            PosRotMsg pose = new PosRotMsg(
                worldPosition.x,
                worldPosition.y,
                worldPosition.z,
                0.0f,
                0.0f,
                0.0f,
                0.0f
            );
            ros.Publish(topicName, pose);
            Debug.Log("World position: " + worldPosition);
        }
    }
}
