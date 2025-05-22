using UnityEngine;

public class LightColorChanger : MonoBehaviour
{
    public SliderController RedController;
    public SliderController GreenController;
    public SliderController BlueController;
    public SliderController AlphaController;

    Light lightComp;

    void Start() =>
        lightComp = GetComponent<Light>();

    private void LateUpdate() =>
        ChangeLightColor(RedController.colorValue, GreenController.colorValue, BlueController.colorValue);

    void ChangeLightColor(float r, float g, float b, float a = 1) =>
		lightComp.color = new Color(r, g, b, a);
}
