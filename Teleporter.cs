using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Tooltip("In world coordinates")]
    public Vector3 destination;
    [Tooltip("In degrees")]
    public Vector3 facing;

    public void Teleport()
    {
        // This only works when using the VR rig
        GameObject player = GameObject.Find("Player");
        if (player != null)
            player.transform.SetPositionAndRotation(destination, Quaternion.Euler(facing));
    }
}
