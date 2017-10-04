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
    private LabelNode label; // 暫存 label

    public GameObject labelTogglePrefab; // labelToggle 模板
    public GameObject labelToggleParent; // labelToggle 放置的 Canvas
    private float labelToggleX; // labelToggle 初始位置 X
    private float labelToggleY; // labelToggle 初始位置 Y

    public Text labelLatitudeText;
    public Text labelLongitudeText;
    public Text labelListText;

    private void Start()
    {
        // 取得主要的 labelList
        labelList = LabelMain.Instance.labelList;

        // 設定 labelToggle 初始位置
        labelToggleX = labelTogglePrefab.transform.localPosition.x;
        labelToggleY = labelTogglePrefab.transform.localPosition.y;

        // 如果 labelList 本來就有東西
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            //Instantiate(labelTemp.Value.labelToggle);

            // 建立 labelToggle 物件, 指派 parent 為 LabelCanvas
            labelTemp.Value.labelToggle = Instantiate(labelTogglePrefab, labelToggleParent.transform).GetComponent<Toggle>();
            labelTemp.Value.labelToggle.name = labelTemp.Value.labelName;

            // 調整 labelToggle 的 Y 軸位置
            labelTemp.Value.labelToggle.transform.localPosition = new Vector2(labelToggleX, labelToggleY);
            labelToggleY -= 50;

            // 設定 labelToggle 中的文字
            labelTemp.Value.labelToggle.transform.Find("Label").GetComponent<Text>().text = labelTemp.Value.labelName;

            // 設應 isON
            labelTemp.Value.labelToggle.isOn = false;
        }
    }

    private void Update()
    {
        // 取得使用者的經緯度
        latitude = GPS.Instance.latitude;
        longitude = GPS.Instance.longitude;

        labelLatitudeText.text = "Latitude: " + latitude;
        labelLongitudeText.text = "Longitude: " + longitude;
        
        /*
        labelListText.text = string.Empty;

        char markerLabelCounter = 'A';

        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            labelListText.text += (markerLabelCounter + ": " + labelTemp.Value.labelName + " " + labelTemp.Value.labelLatitude + " " + labelTemp.Value.labelLongitude + " " + labelTemp.Value.labelDistance + "\r\n");

            markerLabelCounter++;
        }
        */
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
        if (!labelList.ContainsKey(LabelName.text))
        {
            // 建立 label 和 labelToggle
            label = new LabelNode(LabelName.text, latitude, longitude);

            // 建立 labelToggle 物件, 指派 parent 為 LabelCanvas
            label.labelToggle = Instantiate(labelTogglePrefab, labelToggleParent.transform).GetComponent<Toggle>();
            label.labelToggle.name = label.labelName;

            // 調整 labelToggle 的 Y 軸位置
            label.labelToggle.transform.localPosition = new Vector2(labelToggleX, labelToggleY);
            labelToggleY -= 50;

            // 設定 labelToggle 中的文字
            label.labelToggle.transform.Find("Label").GetComponent<Text>().text = label.labelName;

            // 設應 isON
            label.labelToggle.isOn = false;

            // 把 label 加入 List 中
            labelList.Add(LabelName.text, label);
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
        List<string> deleteKey = new List<string>();

        // 選出要被刪除的 label，利用 Toggle isON 判斷，並刪除所有的 labelToggle
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            Destroy(GameObject.Find(labelTemp.Value.labelName));

            if (labelTemp.Value.labelToggle.isOn)
            {
                deleteKey.Add(labelTemp.Key);
            }
        }

        // 從 labelList 中刪除 label
        foreach (string keyTemp in deleteKey)
        {
            labelList.Remove(keyTemp);
        }

        // 設定 labelToggle 初始位置
        labelToggleX = labelTogglePrefab.transform.localPosition.x;
        labelToggleY = labelTogglePrefab.transform.localPosition.y;

        // 重新建立 labelToggle
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            // 建立 labelToggle 物件, 指派 parent 為 LabelCanvas
            labelTemp.Value.labelToggle = Instantiate(labelTogglePrefab, labelToggleParent.transform).GetComponent<Toggle>();
            labelTemp.Value.labelToggle.name = labelTemp.Value.labelName;

            // 調整 labelToggle 的 Y 軸位置
            labelTemp.Value.labelToggle.transform.localPosition = new Vector2(labelToggleX, labelToggleY);
            labelToggleY -= 50;

            // 設定 labelToggle 中的文字
            labelTemp.Value.labelToggle.transform.Find("Label").GetComponent<Text>().text = labelTemp.Value.labelName;

            // 設應 isON
            labelTemp.Value.labelToggle.isOn = false;
        }
    }

    public void clearMap()
    {   
        // 刪除所有的 labelToggle 並清空 labelList
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            Destroy(labelTemp.Value.labelToggle);
            Destroy(GameObject.Find(labelTemp.Value.labelName));
        }

        labelList.Clear();
    }
}
