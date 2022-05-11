using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class TeamMenuItem : MonoBehaviour
    {
        [SerializeField] Text buttonText;
        Action<TeamMenuItem> onClick;

        public string TeamName { get; private set; }

        public void Initialize(string teamName, Action<TeamMenuItem> onClick)
        {
            this.onClick = onClick;
            this.TeamName = teamName;
            buttonText.text = teamName.ToUpper();
        }

        public void OnClicked() => onClick(this);

        public void MoveTo(Transform container) => 
            transform.SetParent(container);
    }
}