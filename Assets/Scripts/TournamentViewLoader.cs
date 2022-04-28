using System.Collections;
using System.IO;
using Core.Utils;
using DataObjects;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TournamentViewLoader : MonoBehaviour
{
    [SerializeField] Image viewport;
    [SerializeField] string tournament;
    [SerializeField] WebHelper web;
    void Start()
    {
        ConvertSvg($"https://challonge.com/{tournament}.svg");
    }

    void ConvertSvg(string url)
    {
        var rawBody = new SvgConverterBody
        {
            Parameters = new object[]
            {
                new SvgConverterFileParameter
                {
                    Name = "File",
                    FileValue = new SvgConverterFileValue { Url = url}
                }, 
                new SvgConverterOptionParameter
                {
                    Name = "StoreFile",
                    Value = true
                }
            }
        };
        
        Debug.Log(JsonConvert.SerializeObject(rawBody));

        web.Post("https://v2.convertapi.com/convert/svg/to/png?Secret=LxFjHfkOFFKkyXcT", rawBody, ParseResponse);
    }

    void ParseResponse(string obj)
    {
        Debug.Log(obj);
    }

    IEnumerator GetPlayerImage(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        var rawSprite = www.downloadHandler.data;

        Texture2D myTexture = new Texture2D(1920, 1080);
        myTexture.LoadImage(rawSprite);

        Rect rec = new Rect(0, 0, myTexture.width, myTexture.height);
        Sprite spriteToUse = Sprite.Create(myTexture, rec, new Vector2(0.5f, 0.5f), 100);

        viewport.sprite = spriteToUse;
    }
}
