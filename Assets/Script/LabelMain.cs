using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LabelMain : MonoBehaviour {

    public static LabelMain Instance { set; get; }

    public Dictionary<string, LabelNode> labelList;

    public Dropdown fileListDropDown;
    public List<string> fileNameList;

    public string selectFileName;

    public void Dropdown_IndexChanged(int index)
    {
        selectFileName = fileNameList[index];

        Debug.Log(selectFileName);
    }


    private void Start()
    {
        Instance = this;

        labelList = new Dictionary<string, LabelNode>();

        // 預設檔名，防止使用者沒有出入檔名
        selectFileName = "temp.txt";

        // 建立暫存檔
        string path = Application.persistentDataPath + "/" + selectFileName;

        StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.Close();

        fileNameList = new List<string>();
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo fileinfo in info)
        {
            fileNameList.Add(fileinfo.Name);
        }

        fileListDropDown.AddOptions(fileNameList);

        //DontDestroyOnLoad(gameObject);
        //StartCoroutine(StartLocationService());
    }
}
