using System;
using UnityEngine;

public class labelNode
{
    public string labelName; // label 的名稱
    public float labelLatitude; // label 的緯度
    public float labelLongitude; // label 的經度
    public double labelDistance; // label 跟人的距離

    public labelNode(string labelName, float labelLatitude, float labelLongitude)
    {
        this.labelName = labelName;
        this.labelLatitude = labelLatitude;
        this.labelLongitude = labelLongitude;
        Calc(labelLatitude, labelLongitude, labelLatitude, labelLongitude);
    }

    public labelNode(string labelName, float labelLatitude, float labelLongitude, double labelDistance)
    {
        this.labelName = labelName;
        this.labelLatitude = labelLatitude;
        this.labelLongitude = labelLongitude;
        this.labelDistance = labelDistance;
    }

    //calculates distance between two sets of coordinates, taking into account the curvature of the earth.
    public void Calc(float lat1, float lon1, float lat2, float lon2)
    {

        var R = 6378.137; // Radius of earth in KM
        var dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
        var dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;
        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
            Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
            Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        labelDistance = R * c;
        labelDistance = labelDistance * 1000f; // meters
                                               //set the distance text on the canvas
        //convert distance from double to float
        //float distanceFloat = (float)distance;
        //set the target position of the ufo, this is where we lerp to in the update function
        //targetPosition = originalPosition - new Vector3(0, 0, distanceFloat * 12);
        //distance was multiplied by 12 so I didn't have to walk that far to get the UFO to show up closer
    }
}
