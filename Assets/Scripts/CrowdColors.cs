using Core.TeamSelector;
using UnityEngine;

public class CrowdColors : MonoBehaviour
{
    [SerializeField] Transform homeCrowd;
    [SerializeField] Transform awayCrowd;

    void Start()
    {
        var homeColor = TeamPersistence.Home.PrimaryColor;
        var awayColor = TeamPersistence.Away.PrimaryColor;

        var homeGradient = new Gradient();
        homeGradient.SetKeys(new[] {new GradientColorKey(homeColor, 0), new GradientColorKey(Color.white, 1)},
            new[] {new GradientAlphaKey(1, 0)});
        
        
        var awayGradient = new Gradient();
        awayGradient.SetKeys(new[] {new GradientColorKey(awayColor, 0), new GradientColorKey(Color.white, 1)},
            new[] {new GradientAlphaKey(1, 0)});
        
        SetColor(homeCrowd.GetComponentsInChildren<ParticleSystem>(), homeColor);
        SetColor(awayCrowd.GetComponentsInChildren<ParticleSystem>(), awayColor);
    }

    void SetColor(ParticleSystem[] crowds, ParticleSystem.MinMaxGradient color)
    {
        foreach (var c in crowds)
        {
            var particle = c.main;
            particle.startColor = color;
        }
    }
}
