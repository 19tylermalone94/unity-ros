using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapLoader : MonoBehaviour {
    public Dropdown mapDropdown;
    private List<string> mapNames = new List<string>();

    void Start() {
        LoadMapNames();
        mapDropdown.onValueChanged.AddListener(delegate { LoadSelectedMap(mapDropdown.value); });
    }

    void LoadMapNames() {
        var info = new DirectoryInfo(Path.Combine(Application.dataPath, "Maps"));
        var fileInfo = info.GetFiles("*.png");
        foreach (var file in fileInfo) {
            string name = Path.GetFileNameWithoutExtension(file.Name);
            mapNames.Add(name);
        }

        mapDropdown.ClearOptions();
        mapDropdown.AddOptions(mapNames);
    }

    void LoadSelectedMap(int index) {
        string selectedMapName = mapNames[index];
        Texture2D mapTexture = LoadTexture(selectedMapName + ".png");
        // Assuming YAML handling is necessary
        // var mapData = LoadYaml(selectedMapName + ".yaml");

        ApplyTextureToPlane(mapTexture);
    }

    Texture2D LoadTexture(string fileName) {
        string filePath = Path.Combine(Application.dataPath, "Maps", fileName);
        Texture2D texture = null;
        byte[] fileData;

        if (File.Exists(filePath)) {
            fileData = File.ReadAllBytes(filePath);
            texture = new Texture2D(2, 2); // Texture size will be replaced by LoadImage
            texture.LoadImage(fileData); // This will auto-resize the texture
        }

        return texture;
    }

    // Example method for YAML loading, assuming some YAML parsing library is used
    // MyMapData LoadYaml(string fileName) { /* YAML loading logic here */ }

    void ApplyTextureToPlane(Texture2D texture) {
        GameObject groundPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        groundPlane.GetComponent<Renderer>().material.mainTexture = texture;
        // Scale plane to texture's size / 100
        float scaleX = texture.width / 100.0f;
        float scaleY = texture.height / 100.0f;
        groundPlane.transform.localScale = new Vector3(scaleX, 1, scaleY);
    }
}
