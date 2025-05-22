using System.Collections.Generic;
using UnityEngine;

public class SliderController : MonoBehaviour
{
    public GameObject targetedDisplay;
    public string sliderMovementAxis = "X";
    public string selectedColor = "Red";
    public float colorValue = 0f;
    public TMPro.TextMeshPro textDisplayTarget;
    public string startingText;


    readonly List<string> possibleBoundaries = new()
    {
        "X", "Y", "Z"
    };

    Transform currentTransform = null;
    Transform displayTransform = null;

    Material targetDisplayMaterial = null;
    Material sliderMaterial = null;

    Vector3 startingLocation = Vector3.zero;

    float boundaryMin = 0f;
    float boundaryMax = 0f;

    TargetedBoundary tBound = TargetedBoundary.None;


    void Start()
    {
        if (!possibleBoundaries.Contains(sliderMovementAxis))
            throw new System.Exception("BoundaryRestriction must be either: X,Y or Z");

        currentTransform = GetComponent<Transform>();
        displayTransform = targetedDisplay.transform;

        startingLocation = currentTransform.position;

        targetDisplayMaterial = targetedDisplay.GetComponent<MeshRenderer>().material;
        sliderMaterial = GetComponent<MeshRenderer>().material;
        sliderMaterial.color = Color.yellow;

        // Figure out which boundary we are working with
        if (sliderMovementAxis == "X")
            tBound = TargetedBoundary.X;
        else if (sliderMovementAxis == "Y")
            tBound = TargetedBoundary.Y;
        else if (sliderMovementAxis == "Z")
            tBound = TargetedBoundary.Z;

        // Calculate the boundaries automatically (because world space must be used)
        if (tBound == TargetedBoundary.X)
        {
            boundaryMin = displayTransform.position.x - 0.5f;
            boundaryMax = displayTransform.position.x + 0.5f;
        }
        else if (tBound == TargetedBoundary.Y)
        {
            boundaryMin = displayTransform.position.y - 0.5f;
            boundaryMax = displayTransform.position.y + 0.5f;
        }
        else if (tBound == TargetedBoundary.Z)
        {
            boundaryMin = displayTransform.position.z - 0.5f;
            boundaryMax = displayTransform.position.z + 0.5f;
        }
    }

    void Update()
    {
        if (tBound == TargetedBoundary.X)
        {
            currentTransform.position = new Vector3(
                currentTransform.position.x > boundaryMax ? boundaryMax :
                currentTransform.position.x < boundaryMin ? boundaryMin : currentTransform.position.x,
                startingLocation.y, startingLocation.z);

            colorValue = Map(currentTransform.position.x, boundaryMin, boundaryMax, 0, 1);
        }
        else if (tBound == TargetedBoundary.Y)
        {
            currentTransform.position = new Vector3(startingLocation.x,
                currentTransform.position.y > boundaryMax ? boundaryMax :
                currentTransform.position.y < boundaryMin ? boundaryMin : currentTransform.position.y,
                startingLocation.z);

            colorValue = Map(currentTransform.position.y, boundaryMin, boundaryMax, 0, 1);
        }
        else if (tBound == TargetedBoundary.Z)
        {
            currentTransform.position = new Vector3(startingLocation.x, startingLocation.y,
                currentTransform.position.z > boundaryMax ? boundaryMax :
                currentTransform.position.z < boundaryMin ? boundaryMin : currentTransform.position.z);

            // The mapping values are reversed due to direction. It's the quickest fix.
            colorValue = Map(currentTransform.position.z, boundaryMin, boundaryMax, 1, 0);
        }


        // Set the display's color value
        if (selectedColor == "Red")
            targetDisplayMaterial.color = new Color(colorValue, 0, 0, 1);
        if (selectedColor == "Green")
            targetDisplayMaterial.color = new Color(0, colorValue, 0, 1);
        if (selectedColor == "Blue")
            targetDisplayMaterial.color = new Color(0, 0, colorValue, 1);
        if (selectedColor == "Brightness")
            targetDisplayMaterial.color = new Color(1, 1, 1, colorValue);

        textDisplayTarget.text = $"{startingText}{Mathf.RoundToInt(Map(colorValue, 0, 1, 0, 255))}";

        ResetRotation();
    }

    // From here: https://reference.arduino.cc/reference/en/language/functions/math/map/
    float Map(float x, float in_min, float in_max, float out_min, float out_max) =>
        (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;

    enum TargetedBoundary
    {
        None, X, Y, Z
    }

    public void ResetRotation() =>
        transform.rotation = Quaternion.identity;

    public void ChangeHoldingStatus(bool beingHeld)
    {
        if (beingHeld)
            sliderMaterial.color = Color.magenta;
        else
            sliderMaterial.color = Color.yellow;
    }
}
