using UnityEngine;

namespace Shy.Event
{
    [CreateAssetMenu(menuName = "SO/Shy/EventResult/Stat")]
    public class StatResultSO : EventResultSO
    {
        public MainStatEnum mainStat;
        public int value;
    }
}