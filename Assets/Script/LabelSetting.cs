using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LabelSetting : MonoBehaviour {

    private float latitude; // 人所在的緯度
    private float longitude; // 人所在的經度

    private Dictionary<string, LabelNode> labelList;

    public Text labelLatitudeText;
    public Text labelLongitudeText;
    public Text labelListText;

    private void Start()
    {
        //labelList = new Dictionary<string, LabelNode>();

        // 取得主要的 labelList
        labelList = LabelMain.Instance.labelList;
    }

    private void Update()
    {
        // 取得使用者的經緯度
        latitude = GPS.Instance.latitude;
        longitude = GPS.Instance.longitude;

        labelLatitudeText.text = "Latitude: " + latitude;
        labelLongitudeText.text = "Longitude: " + longitude;

        labelListText.text = string.Empty;

        char markerLabelCounter = 'A';

        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            labelListText.text += (markerLabelCounter + ": " + labelTemp.Value.labelName + " " + labelTemp.Value.labelLatitude + " " + labelTemp.Value.labelLongitude + " " + labelTemp.Value.labelDistance + "\r\n");

            markerLabelCounter++;
        }
    }

    public void setLabel(Text LabelName)
    {
        Debug.Log("Click Button LabelSetting");
        Debug.Log(LabelName.text.ToString());

        if(LabelName.text == string.Empty)
        {
            Debug.Log("Please Input Text");
            return;
        }

        // 把 label 加入 List 中
        if(!labelList.ContainsKey(LabelName.text))
        {
            labelList.Add(LabelName.text, new LabelNode(LabelName.text, latitude, longitude));
        }
    }

    public void saveMap(Text FileName)
    {
        string path = Application.persistentDataPath + "/" + FileName.text + ".txt";

        StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);

        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            writer.WriteLine(labelTemp.Value.labelName + " " + labelTemp.Value.labelLatitude + " " + labelTemp.Value.labelLongitude);
        }

        writer.Close();
    }

    public void deleteLabel(string buttonName)
    {
        GameObject tempButton;
        tempButton = GameObject.Find(buttonName);

        Destroy(tempButton);
    }

    public void clearMap()
    {
        labelList.Clear();
    }
}
