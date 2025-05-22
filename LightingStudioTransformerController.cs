using System.Collections.Generic;
using UnityEngine;

public class LightingStudioTransformerController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textDisplay;
    public List<GameObject> thingsToHideOnTransform;
    public GameObject beanBagToCloneAndUseAsCenter;
    public GameObject lightingRigsFolder;
    public int beanBagRows = 5;
    public int beanBagCols = 5;
    public float duplicateMultiplier = 1.1f;

    LSStates currentState = LSStates.Studio;

    readonly List<GameObject> clonedBeanBags = new();

    public void Transform()
    {
        // Toggle active state of certain things
        foreach (GameObject go in thingsToHideOnTransform)
            go.SetActive(currentState == LSStates.Theater);

        // Set bean bag to active so that the cloned bean bags are also active
        beanBagToCloneAndUseAsCenter.SetActive(true);

        // Toggle lights because they show up on the bean bags
        foreach (Light light in lightingRigsFolder.GetComponentsInChildren<Light>(true))
            light.gameObject.SetActive(currentState == LSStates.Theater);

        if (currentState == LSStates.Studio)
        {
            currentState = LSStates.Theater;

            for (int i = 0; i < beanBagRows; i++)
                for (int j = 0; j < beanBagCols; j++)
                    clonedBeanBags.Add(Instantiate(beanBagToCloneAndUseAsCenter, new Vector3(
                            beanBagToCloneAndUseAsCenter.transform.position.x + (j - 2) * duplicateMultiplier,
                            beanBagToCloneAndUseAsCenter.transform.position.y,
                            beanBagToCloneAndUseAsCenter.transform.position.z + (i - 2) * duplicateMultiplier
                        ), beanBagToCloneAndUseAsCenter.transform.rotation, beanBagToCloneAndUseAsCenter.transform.parent));

            textDisplay.text = "Transform to Studio";
        }
        else
        {
            currentState = LSStates.Studio;

            // Destroy all physical bean bags
            foreach (GameObject go2 in clonedBeanBags)
                Destroy(go2);

            // Clear the list of destroyed bean bags
            clonedBeanBags.Clear();

            textDisplay.text = "Transform to Theater";
        }

        // Set active state to false so there aren't two beans bags on top of each other (not that it really matters)
        beanBagToCloneAndUseAsCenter.SetActive(false);
    }

    // Pathetic, I know, but it looks nice
    enum LSStates
    {
        Studio, Theater
    }
}
