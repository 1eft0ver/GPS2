using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleStaticMaps : MonoBehaviour {

    List<GoogleMapMarker> markers;

    GoogleMapLocation center;
    string zoom = "18";
    string size = "640x640";
    string scale = "2";
    string maptype = "roadmap";

    private Dictionary<string, LabelNode> labelList;

    private void Start()
    {
        labelList = LabelMain.Instance.labelList;

        char markerLabelCounter = 'A';

        markers = new List<GoogleMapMarker>();

        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            GoogleMapMarker tempMarker = 
                new GoogleMapMarker("mid", "blue", markerLabelCounter.ToString(), new GoogleMapLocation(labelTemp.Value.labelLatitude, labelTemp.Value.labelLongitude));
            markers.Add(tempMarker);

            markerLabelCounter++;
        }

        StartCoroutine(GetGoogleMap());
    }

    IEnumerator GetGoogleMap()
    {
        string googleMapUrl = "https://maps.googleapis.com/maps/api/staticmap?";
        string parameters = string.Empty;

        parameters += ("zoom="+zoom);
        parameters += ("&size=" + size);
        parameters += ("&scale=" + scale);
        parameters += ("&maptype=" + maptype);

        foreach(GoogleMapMarker temp in markers)
        {
            parameters += "&markers=" + string.Format("size:{0}|color:{1}|label:{2}", temp.size, temp.color, temp.label);
            parameters += "|" + WWW.UnEscapeURL(string.Format("{0},{1}", temp.location.latitude, temp.location.longitude));
        }

        WWW url = new WWW(googleMapUrl + parameters);

        yield return url;
        //GetComponent<CanvasRenderer>().SetTexture(url.texture);

        GetComponent<RawImage>().texture = url.texture;

        Debug.Log(googleMapUrl + parameters);
    }
}

public class GoogleMapLocation
{
    public float latitude;
    public float longitude;

    public GoogleMapLocation(float latitude, float longitude)
    {
        this.latitude = latitude;
        this.longitude = longitude;
    }
}

public class GoogleMapMarker
{
    // 標記大小 (選擇性) {tiny, mid, small}
    public string size;
    // 標記顏色 {black, brown, green, purple, yellow, blue, gray, orange, red, white}
    public string color;
    // 標記標籤 {A-Z, 0-9} 單一字元
    public string label;
    // 標記位置
    public GoogleMapLocation location;

    public GoogleMapMarker(string size, string color, string label, GoogleMapLocation location)
    {
        this.size = size;
        this.color = color;
        this.label = label;
        this.location = location;
    }
}

public class GoogleMapPath
{
}