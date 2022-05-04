using UnityEngine;

namespace DefaultNamespace
{
    public class TeamPlayer : MonoBehaviour
    {
        [SerializeField] MeshRenderer renderer;

        public void ChangeTeam(Color teamColor) => 
            renderer.material.SetColor("_Color", teamColor);
    }
}