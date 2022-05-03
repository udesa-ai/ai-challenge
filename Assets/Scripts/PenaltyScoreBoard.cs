using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class PenaltyScoreBoard : MonoBehaviour
    {
        [SerializeField] Color pipNeutral;
        [SerializeField] Color pipScore;
        [SerializeField] Color pipMiss;

        [SerializeField] Image[] pips;
        [SerializeField] Text teamName;
        
        int nextPipIndex = 0;

        public void Reset()
        {
            foreach (var pip in pips)
            {
                pip.color = pipNeutral;
                nextPipIndex = 0;
            }
        }

        public void Score()
        {
            pips[nextPipIndex].color = pipScore;
            nextPipIndex++;
        }

        public void Miss()
        {
            pips[nextPipIndex].color = pipMiss;
            nextPipIndex++;
        }
    }
}