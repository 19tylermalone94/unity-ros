using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityRoboticsDemo;

public class MapClickDetector : MonoBehaviour {
    private Camera cam;
    private GameObject map;
    ROSConnection ros;
    public string topicName = "pos_rot";
    private float resolution = 5f; // Ensure this is the desired scaling factor

    void Start() {
        cam = Camera.main;
        map = GameObject.Find("GroundPlane");
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PosRotMsg>(topicName);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, LayerMask.NameToLayer("Map"))) { 
                hit.point /= resolution;
                Debug.Log("x: " + hit.point.x + ", y: " + hit.point.z + ", z: " + 0.0f); // y and z are swapped in gazebo
                PosRotMsg pose = new PosRotMsg(
                    hit.point.x,
                    hit.point.z,
                    0.0f,
                    0.0f,
                    0.0f,
                    0.0f,
                    0.0f
                );
                ros.Publish(topicName, pose);
            }
        }
    }
}
