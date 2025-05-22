using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfinityWallController : MonoBehaviour
{
    Renderer renderTarget;

    public TMP_Dropdown wallPickerDropdown;

    readonly Dictionary<string, string> colors = new()
    {
        { "White",  "ffffff" },
        { "Black",  "000000" },
        { "Green",  "00ff00" },
        { "Grey",   "808080" }
    };

    void Start()
    {
        // First we fetch the renderer target that we want to change
        renderTarget = GetComponent<Renderer>();

        // Create array of options
        List<TMP_Dropdown.OptionData> options = new();

        // Loop through all the colors and add them
        foreach (string color in colors.Keys)
            options.Add(new TMP_Dropdown.OptionData()
            {
                text = color
            });
        wallPickerDropdown.AddOptions(options);
    }

    public void ChangeTexture(int index)
    {
        if (colors.TryGetValue(wallPickerDropdown.options[index].text, out string hex))
            renderTarget.material.color = new Color(
               Map(int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber), 0, 255, 0, 1),
               Map(int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber), 0, 255, 0, 1),
               Map(int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber), 0, 255, 0, 1)
            );
    }

    float Map(float x, float in_min, float in_max, float out_min, float out_max) =>
        (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
}
