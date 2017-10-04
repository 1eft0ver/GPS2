using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelChoose : MonoBehaviour {

    //public bool labelChoose; True 顯示，False 隱藏;

    private Dictionary<string, LabelNode> labelList;

    public GameObject labelTogglePrefab; // labelToggle 模板
    public GameObject labelToggleParent; // labelToggle 放置的 Canvas
    private float labelToggleX; // labelToggle 初始位置 X
    private float labelToggleY; // labelToggle 初始位置 Y

    private void Start()
    {
        // 取得主要的 labelList
        labelList = LabelMain.Instance.labelList;

        // 設定 labelToggle 初始位置
        labelToggleX = labelTogglePrefab.transform.localPosition.x;
        labelToggleY = labelTogglePrefab.transform.localPosition.y;

        // 建立 Toggle 選單
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
            labelTemp.Value.labelToggle.isOn = true;
        }
    }

    private void Update()
    {

    }

    public void setlabelChoose()
    {
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            labelTemp.labelChoose = labelTemp.Value.labelToggle.isOn;
        }
    }
}
