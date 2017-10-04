using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LabelCreate : MonoBehaviour {


    private readonly float maxDistance = 1000f;

    private float latitude; // 人所在的緯度
    private float longitude; // 人所在的經度
    private float compass; // 跟北方的角度

    private float vectorDistance; // label 向量計算用的距離
    private float labelLatitude; // label 的緯度
    private float labelLongitude; // label 的經度
    private double labelDistance; // label 跟人的距離

    // label 的 X Y Z 位置
    private float labelPositionX, labelPositionY, labelPositionZ;

    private GameObject label; // RawImage + Text
    private Text labelDistanceText;
    private GameObject labelParent; // LabelCanvas
    private Dictionary<string, LabelNode> labelList;

    public GameObject labelPrefab; // label 模板

    public Text LabelText; // Debug Label
    //public Text DistanceText; // Debug Distance
    //public Text labelListText; // Debug labelList

    private void Start()
    {
        //labelList = new Dictionary<string, LabelNode>();

        // 取得主要的 labelList
        labelList = LabelMain.Instance.labelList;

        labelParent = GameObject.Find("LabelCanvas");

        createLabel();
    }

    private void Update()
    {
        // 取得使用者的經緯度
        latitude = GPS.Instance.latitude;
        longitude = GPS.Instance.longitude;
        compass = GPS.Instance.compass;

        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            label = GameObject.Find(labelTemp.Value.labelName);
            labelLatitude = labelTemp.Value.labelLatitude;
            labelLongitude = labelTemp.Value.labelLongitude;

            // 計算向量長度
            vectorDistance = Mathf.Sqrt(Mathf.Pow(labelLatitude - latitude, 2) + Mathf.Pow(labelLongitude - longitude, 2));

            // 經度-X, 緯度-Z
            labelPositionX = -1 * (maxDistance * (labelLongitude - longitude)) / vectorDistance;
            labelPositionY = 0f;
            labelPositionZ = -1 * (maxDistance * (labelLatitude - latitude)) / vectorDistance;

            label.transform.position = new Vector3(labelPositionX, labelPositionY, labelPositionZ);

            // 更新 label 中的 Distance 文字
            Calc(latitude, longitude, labelLatitude, labelLongitude); // 計算

            labelDistanceText = label.transform.Find("Distance").GetComponent<Text>();
            labelDistanceText.text = labelTemp.Value.labelName;
            labelDistanceText.text += ("\r\n" + labelTemp.Value.labelDistance.ToString("00.00"));

            // 調整 label 顯示時面對使用者的角度
            Vector3 relativePos = label.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            label.transform.rotation = rotation;

            // 更新 labelNode 的 labelDistance
            labelTemp.Value.labelDistance = labelDistance;

            // Debug Label Text
            LabelText.text = "labelDistance : " + vectorDistance.ToString() + " ; labelPositionX : " + labelPositionX.ToString() + " ; labelPositionZ : " + labelPositionZ.ToString();
        }
    }

    private void createLabel()
    {
        // 清空 labelList，重新加入檔案中的 label
        labelList.Clear();

        //string path = Application.dataPath + "/Resources/test.txt";
        string path = Application.persistentDataPath + "/" + LabelMain.Instance.selectFileName;

        // Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string line;
        string[] lineSplite;

        while ((line = reader.ReadLine()) != null)
        {
            if (line == "") break;

            lineSplite = line.Split(' ');

            labelLatitude = float.Parse(lineSplite[1], CultureInfo.InvariantCulture.NumberFormat);
            labelLongitude = float.Parse(lineSplite[2], CultureInfo.InvariantCulture.NumberFormat);

            // 建立 label 物件, 指派 parent 為 LabelCanvas
            label = Instantiate(labelPrefab, labelParent.transform);
            label.name = lineSplite[0];
            // 左右相反 (?)
            label.transform.localScale = new Vector3(-1 * label.transform.localScale.x, label.transform.localScale.y, label.transform.localScale.z);
            // label layer
            label.layer = 8;

            // 取得使用者的經緯度
            latitude = GPS.Instance.latitude;
            longitude = GPS.Instance.longitude;
            compass = GPS.Instance.compass;

            // 計算向量長度
            vectorDistance = Mathf.Sqrt(Mathf.Pow(labelLatitude - latitude, 2) + Mathf.Pow(labelLongitude - longitude, 2));

            // 經度-X, 緯度-Z
            labelPositionX = -1 * (maxDistance * (labelLongitude - longitude)) / vectorDistance;
            labelPositionY = 0f;
            labelPositionZ = -1 * (maxDistance * (labelLatitude - latitude)) / vectorDistance;
            // 設定 label 位置
            label.transform.position = new Vector3(labelPositionX, labelPositionY, labelPositionZ);

            // 設定 label 中的 distance 文字
            Calc(latitude, longitude, labelLatitude, labelLongitude); // 計算

            labelDistanceText = label.transform.Find("Distance").GetComponent<Text>();
            labelDistanceText.transform.localScale = new Vector3(-1 * labelDistanceText.transform.localScale.x, labelDistanceText.transform.localScale.y, labelDistanceText.transform.localScale.z);
            labelDistanceText.text = label.name;
            labelDistanceText.text += ("\r\n" + labelDistance.ToString());

            // 調整 label 顯示時面對使用者的角度
            Vector3 relativePos = label.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            label.transform.rotation = rotation;

            // 把 label 加入 List 中
            labelList.Add(label.name, new LabelNode(label.name, labelLatitude, labelLongitude, labelDistance));

            // Debug Label
            LabelText.text = "labelDistance : " + vectorDistance.ToString() + " ; labelPositionX : " + labelPositionX.ToString() + " ; labelPositionZ : " + labelPositionZ.ToString();
            // Debug file data
            Debug.Log(line);
            Debug.Log(labelLatitude);
            Debug.Log(labelLongitude);
            // Debug Label List
            Debug.Log(labelList.Count);
        }

        reader.Close();
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
