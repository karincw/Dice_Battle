using System;
using UnityEngine;

namespace karin.worldmap.dice
{
    public class DiceFaceDetecter : MonoBehaviour
    {
        [Tooltip("�ٴڿ� �ش���� ������� ��µ� �ѹ�\n���Ը��� �ݴ����� ����")]
        public int CurrntNumber;
        [HideInInspector] public Dice dice;

        public void Init(Dice owner)
        {
            dice = owner;
        }
    }
}