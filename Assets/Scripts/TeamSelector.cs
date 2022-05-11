using System;
using System.Collections.Generic;
using System.Linq;
using Configuration;
using Core.Core.Manager;
using UnityEngine;
using Core.Games;
using Core.Tournament.Models;
using Core.Utils;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class TeamSelector : MonoBehaviour
    {
        [SerializeField] TeamMenuItem teamMenuItemPrefab;
        [SerializeField] Transform availableContainer;
        [SerializeField] Transform selectedContainer;
        [SerializeField] Text name;
        [SerializeField] Dropdown type;
        [SerializeField] Button CreateButton;
        
        [SerializeField] WebHelper WebHelper;

        List<TeamMenuItem> availableTeams = new List<TeamMenuItem>();
        List<TeamMenuItem> selectedTeams = new List<TeamMenuItem>();

        void Start()
        {
            CreateButton.onClick.AddListener(OnCreateTournament);
            var allTeams = ReflectionUtils.GetIstancesOf<Team>().Select(t => t.GetName());
            foreach (var teamName in allTeams)
            {
                var teamMenuItem = Instantiate(teamMenuItemPrefab, availableContainer);
                teamMenuItem.Initialize(teamName, OnTeamClick);
                availableTeams.Add(teamMenuItem);
            }
            
        }

        void OnTeamClick(TeamMenuItem team)
        {
            if (IsAvailable(team))
            {
                availableTeams.Remove(team);
                selectedTeams.Add(team);

                team.MoveTo(selectedContainer);
            }
            else
            {
                selectedTeams.Remove(team);
                availableTeams.Add(team);

                team.MoveTo(availableContainer);
            }
        }

        private void OnCreateTournament()
        {
            WebHelper.Post(
                $"https://{PlayerPrefs.GetString("username")}:{PlayerPrefs.GetString("api_key")}@api.challonge.com/v1/tournaments.json",
                CreateTournamentBody(), OnComplete);
        }

        private void OnComplete(string obj)
        {
            Debug.Log(obj);
            var tournament = JsonConvert.DeserializeObject<Root>(obj).Tournament;
            MainManager.Instance.SelectedTournament = tournament;
            WebHelper.Post(
                $"https://{PlayerPrefs.GetString("username")}:{PlayerPrefs.GetString("api_key")}@api.challonge.com/v1/tournaments/{tournament.Url}/participants/bulk_add.json",
                CreateBulkBody(), OnLoadTeams);;
        }

        private void OnLoadTeams(string obj)
        {
            WebHelper.PostWithoutPayload(
                $"https://{PlayerPrefs.GetString("username")}:{PlayerPrefs.GetString("api_key")}@api.challonge.com/v1/tournaments/{MainManager.Instance.SelectedTournament.Url}/start.json?include_participants=1&include_matches=1"
                , OnFinish);;
        }

        private void OnFinish(string obj)
        {
            var tournament = JsonConvert.DeserializeObject<Root>(obj).Tournament;
            MainManager.Instance.SelectedTournament = tournament;
            MainManager.Instance.isTournament = true;
            SceneManager.LoadScene(Scenes.Menu, LoadSceneMode.Single);
        }

        private object CreateTournamentBody()
        {
            return new
            {
                tournament = new
                {
                    name = name.text,
                    tournament_type = type.options[type.value].text.ToLower(),
                    url = $"{name.text.Replace(" ","_")}_{DateTime.Now.Millisecond}"
                }
            };
        }
        private object CreateBulkBody() =>
            new
            {
                participants = selectedTeams.Select(item => new
                {
                    name = item.TeamName
                })
            };


        bool IsAvailable(TeamMenuItem team) => 
            availableTeams.Contains(team);
    }
}