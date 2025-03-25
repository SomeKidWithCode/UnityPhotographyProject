using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TableGenerator : MonoBehaviour
{
    public GameObject parent;
    public GameObject startingTable;
    public int tableCount = 25;
    public float distFromCenter = 10;
    public bool starter = false;

    private void Start()
    {
        if (!starter)
            return;

        // Calculate how many steps it takes to reach 360 degrees based on the number of tables needed
        float degStep = 360 / tableCount;

        // Get the inital tables position
        Vector2 initalTablePos = new(startingTable.transform.position.x, startingTable.transform.position.z);

        for (int i = 0; i < tableCount + 1; i++)
        {
            // Create a vector which represents a position, rotated from a certain angle at a certain distance from a center point
            Vector2 v2 = RotateAroundPoint(initalTablePos, degStep * i, distFromCenter);

            // Create object copy with required parameters (object to copy, position of new object, rotation of new object, parent object to place this in)
            GameObject go = Instantiate(startingTable, new Vector3(v2.x, startingTable.transform.position.y, v2.y), Quaternion.identity, parent.transform);

            // Make the new table look at the original
            go.transform.LookAt(startingTable.transform);

            // Must destroy the duplicated objects TableGenerator so it doesn't try to also generate tables
            Destroy(go.GetComponent<TableGenerator>());
        }

        // Delete the original table, as this is the one that just determines the circle center
        Destroy(startingTable);
    }

    // Commits degree maths
    static Vector2 RotateAroundPoint(Vector2 startPoint, float deg, float dist)
            => new(
                (float)(startPoint.x + Math.Cos(deg * Math.PI / 180) * dist),
                (float)(startPoint.y + Math.Sin(deg * Math.PI / 180) * dist)
            );
}
