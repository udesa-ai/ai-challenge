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
    [SerializeField] LayoutElement scalableContainer;
    [SerializeField] string tournament;
    [SerializeField] WebHelper web;

    void Start() => LoadBracketView($"https://challonge.com/{GetTournamentUrl()}.svg");

    private string GetTournamentUrl() => MainManager.Instance? MainManager.Instance.SelectedTournament.Url : tournament;

    void LoadBracketView(string targetFile) => 
        web.Post("https://v2.convertapi.com/convert/svg/to/png?Secret=LxFjHfkOFFKkyXcT", SvgConverterBody(targetFile), ParseConverterResponse);

    object SvgConverterBody(string targetFile) =>
        new
        {
            Parameters = new object[]
            {
                new {Name = "File", FileValue = new {Url = targetFile}}, 
                new {Name = "StoreFile", Value = true},
                new {Name = "TransparentColor", Value = "255,255,255"},
                new {Name = "ImageQuality", Value = "100"}
            }
        };

    void ParseConverterResponse(string response)
    {
        var extractedPngUrl = response.Split('"').First(s => s.Contains("https"));
        StartCoroutine(DownloadPngIntoRenderer(extractedPngUrl));
    }

    //TODO: Build a GetSprite in the WebHelper class and use that here
    IEnumerator DownloadPngIntoRenderer(string url)
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
