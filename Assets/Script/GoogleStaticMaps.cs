using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleStaticMaps : MonoBehaviour {

    List<GoogleMapMarker> markers;

    GoogleMapLocation center;
    string zoom = "18";
    string size = "640x640";
    string scale = "2";
    string maptype = "roadmap";

    private void Start()
    {
        markers = new List<GoogleMapMarker>();

        GoogleMapMarker test1 = new GoogleMapMarker("mid", "blue", "V", new GoogleMapLocation(22.6270f, 120.2672f));
        markers.Add(test1);
        GoogleMapMarker test2 = new GoogleMapMarker("mid", "red", "T", new GoogleMapLocation(22.62749f, 120.2672f));
        markers.Add(test2);

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
        GetComponent<CanvasRenderer>().SetTexture(url.texture);

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