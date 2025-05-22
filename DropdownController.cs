using UnityEngine;
using TMPro;
using Valve.VR;

public class DropdownController : MonoBehaviour
{
    public GameObject targetDropdown;
    public GameObject infinityWall;

    TMP_Dropdown dropdown;
    InfinityWallController iWallController;

    bool isPickerActive = false;

    public SteamVR_Action_Boolean listChanger;
    public SteamVR_Input_Sources leftHand;
    public SteamVR_Input_Sources rightHand;

    void Start()
    {
        dropdown = targetDropdown.GetComponent<TMP_Dropdown>();
        iWallController = infinityWall.GetComponent<InfinityWallController>();
    }

    void LateUpdate()
    {
        if (isPickerActive)
        {
            if (listChanger.GetStateDown(leftHand))
            {
                int selectedValue = dropdown.value;
                int optionsLength = dropdown.options.Count;

                if (selectedValue + 1 == optionsLength)
                {
                    dropdown.SetValueWithoutNotify(0);
                    iWallController.ChangeTexture(0);
                }
                else
                {
                    dropdown.SetValueWithoutNotify(selectedValue + 1);
                    iWallController.ChangeTexture(selectedValue + 1);
                }
            }
            else if (listChanger.GetStateDown(rightHand))
            {
                int selectedValue = dropdown.value;
                int optionsLength = dropdown.options.Count;

                if (selectedValue == 0)
                {
                    dropdown.SetValueWithoutNotify(optionsLength - 1);
                    iWallController.ChangeTexture(optionsLength - 1);
                }
                else
                {
                    dropdown.SetValueWithoutNotify(selectedValue - 1);
                    iWallController.ChangeTexture(selectedValue - 1);
                }
            }
        }
    }

    public void SetPickerState(bool active)
    {
        isPickerActive = active;
        if (active)
            dropdown.Show();
        else
            dropdown.Hide();
    }
}
