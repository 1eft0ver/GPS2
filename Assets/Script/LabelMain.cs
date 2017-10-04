using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        //fileList.option = string.Empty;

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
