using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LabelSetting : MonoBehaviour {

    private float latitude; // 人所在的緯度
    private float longitude; // 人所在的經度

    private Button button;
    private Text buttonText;

    //private InputField inputField;
    //private Text inputFieldText;

    public void OnBtn(Text inputFieldText)
    {
        Debug.Log("Click Button LabelSetting");
        Debug.Log(inputFieldText.text.ToString());

        latitude = GPS.Instance.latitude;
        longitude = GPS.Instance.longitude;

        button = transform.Find("SettingButton").GetComponent<Button>();
        buttonText = button.transform.Find("Text").GetComponent<Text>();

        //inputField = transform.Find("SettingInputField").GetComponent<InputField>();
        //inputFieldText = inputField.transform.Find("Text").GetComponent<Text>();

        buttonText.text = inputFieldText.text.ToString() + " " + latitude + " " + longitude;
        setLabel(buttonText.text);
    }

    public void setLabel(string labelString)
    {
        string path = Application.persistentDataPath + "/temp.txt";

        StreamWriter writer = new StreamWriter(path, true, Encoding.UTF8);

        writer.WriteLine(labelString);
        writer.Close();

    }
}
