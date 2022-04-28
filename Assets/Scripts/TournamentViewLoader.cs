using System.Collections;
using System.Linq;
using Core.Utils;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TournamentViewLoader : MonoBehaviour
{
    [SerializeField] Image viewport;
    [SerializeField] LayoutElement scalableContainer;
    [SerializeField] string tournament;
    [SerializeField] WebHelper web;
    void Start()
    {
        ConvertSvg($"https://challonge.com/{tournament}.svg");
    }

    void ConvertSvg(string url)
    {
        var rawBody = new
        {
            Parameters = new object[]
            {
                new {Name = "File", FileValue = new {Url = url}}, 
                new {Name = "StoreFile", Value = true},
                new {Name = "TransparentColor", Value = "255,255,255"},
                new {Name = "ImageQuality", Value = "100"}
            }
        };
        web.Post("https://v2.convertapi.com/convert/svg/to/png?Secret=LxFjHfkOFFKkyXcT", rawBody, ParseResponse);
    }

    void ParseResponse(string response)
    {
        var extractedPngUrl = response.Split('"').First(s => s.Contains("https"));
        StartCoroutine(GetPlayerImage(extractedPngUrl));
    }

    IEnumerator GetPlayerImage(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        var myTexture = DownloadHandlerTexture.GetContent(www);

        Rect rec = new Rect(0, 0, myTexture.width, myTexture.height);
        Sprite spriteToUse = Sprite.Create(myTexture, rec, new Vector2(0.5f, 0.5f), 100);

        viewport.sprite = spriteToUse;
        scalableContainer.minWidth = myTexture.width;
        scalableContainer.minHeight = myTexture.height;
    }
}
