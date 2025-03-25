using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class LightColorChanger : MonoBehaviour
{
    Light lightComp;

    void Start()
	{
        lightComp = GetComponent<Light>();
    }

    bool toggleLight = false;
    bool toggle = false;

    private void LateUpdate()
    {
        toggleLight |= Input.GetKeyDown(KeyCode.L);
        if (toggleLight)
        {
            if (toggle)
                ChangeLightColor(255, 0, 0);
            else
                ChangeLightColor(0, 255, 0);
            toggle = !toggle;
            toggleLight = false;
        }
    }

    void ChangeLightColor(float r, float g, float b)
	{
		lightComp.color = new Color(r, g, b);
	}
}
