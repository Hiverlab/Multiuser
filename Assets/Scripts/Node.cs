using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// The node class is purely for the visual representation of a data point
public class Node : MonoBehaviour {

    // These are the different dimensions that the node is affected by
    #region Dimensions

    // Private properties
    private Vector3 position;
    private float scale;
    private Color color;
    private GameObject shape;

    // Position (X, Y, Z) 3-Dimensions
    public Vector3 Position {
        get {
            return position;
        }
        set {
            position = value;

            UpdatePosition();
        }
    }

    // Scale 1-Dimension
    public float Scale {
        get {
            return scale;
        }
        set {
            scale = value;

            UpdateScale();
        }
    }

    // Color 1-Dimension
    public Color Color {
        get {
            return color;
        }
        set {
            color = value;

            UpdateColor();
        }
    }

    // Shape 1-Dimension
    public GameObject Shape {
        get {
            return shape;
        }
        set {
            shape = value;

            UpdateShape();
        }
    }

    #endregion
    
    #region Dimension References

    [Header("Dimension References")]
    [SerializeField]
    private GameObject shapeModel;

    #endregion

    // Other node properties
    #region Properties

    private string labelName;

    // Name
    public string LabelName {
        get {
            return labelName;
        }
        set {
            labelName = value;

            UpdateLabelName();
        }
    }

    #endregion

    #region Properties References

    [Header("Properties References")]
    [SerializeField]
    private TextMeshPro labelTextMesh;

    #endregion


    #region Dimension Updating
    private void UpdatePosition() {
        transform.position = position;
    }

    private void UpdateScale() {
        transform.localScale = Vector3.one * scale;
    }

    private void UpdateColor() {
        // TODO: Add mesh renderer or sprite
    }

    private void UpdateShape() {
        // TODO: Add shape, preferably enum
    }

    #endregion

    #region Properties Updating

    private void UpdateLabelName() {
        labelTextMesh.text = labelName;
    }

    #endregion
}
