using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core.Games;
using Core.Utils;

namespace DefaultNamespace
{
    public class TeamSelector : MonoBehaviour
    {
        [SerializeField] TeamMenuItem teamMenuItemPrefab;
        [SerializeField] Transform availableContainer;
        [SerializeField] Transform selectedContainer;

        List<TeamMenuItem> availableTeams = new List<TeamMenuItem>();
        List<TeamMenuItem> selectedTeams = new List<TeamMenuItem>();

        void Start()
        {
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
            if (IsAvaiable(team))
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

        bool IsAvaiable(TeamMenuItem team) => 
            availableTeams.Contains(team);
    }
}