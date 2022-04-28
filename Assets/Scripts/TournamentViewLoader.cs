using System.Collections;
using System.Linq;
using Core.Core.Manager;
using Core.Utils;
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
        var url = MainManager.Instance != null ? MainManager.Instance.SelectedTournament.Url : tournament;
        ConvertSvg($"https://challonge.com/{url}.svg");
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
