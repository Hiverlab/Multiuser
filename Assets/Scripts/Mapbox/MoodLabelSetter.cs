using Mapbox.Unity.MeshGeneration.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class MoodLabelSetter : MonoBehaviour, IFeaturePropertySettable {
    [SerializeField]
    TMPro.TextMeshPro _textMesh;

    public void Set(Dictionary<string, object> props) {
        _textMesh.text = "";

        if (props.ContainsKey("name")) {
            _textMesh.text = "Mood\n" + props["name"].ToString();
        } else if (props.ContainsKey("house_num")) {
            _textMesh.text = "Mood\n" + props["house_num"].ToString();
        } else if (props.ContainsKey("type")) {
            _textMesh.text = "Mood\n" + props["type"].ToString();
        }
    }
}