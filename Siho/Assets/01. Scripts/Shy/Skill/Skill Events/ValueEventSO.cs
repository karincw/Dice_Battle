using System.Collections.Generic;
using UnityEngine;

namespace Shy.Unit
{
    [CreateAssetMenu(menuName = "SO/Shy/SkillEvent/Value")]
    public class ValueEventSO : SkillEventSO
    {
        [Header("Data")]
        public EventType eventType;
        public string formula;

        public List<GetStat> getStats;
        public List<GetStack> getStacks;

        public int GetValue(Character _user, Character _target)
        {
            string data = formula;

            //stat�� ���
            foreach (GetStat stat in getStats)
            {
                Character c = (stat.target == ActionWay.Self) ? _user : _target;
                data = data.Replace(stat.key, c.GetStat(stat.stat).ToString());
            }

            foreach (GetStack stack in getStacks)
            {
                int v = _user.GetStackCnt(stack.buff);
                data = data.Replace(stack.key, v.ToString());
            }

            //�����س��� Formula���� �� ����
            int value = int.Parse(Formula.GetFormula(data)), bAtk = _user.GetNowStr();

            if (bAtk != 0) value += Mathf.RoundToInt(value * bAtk * 0.01f);

            return value;
        }
    }
}
