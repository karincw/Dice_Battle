﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shy
{
    public class HealthCompo : MonoBehaviour
    {
        [SerializeField] private int maxHp;
        [SerializeField] private int hp;
        [SerializeField] private int shield;

        [SerializeField] private Image healthGuage;
        [SerializeField] private TextMeshProUGUI healthValue;

        public Action dieEvent;

        public void Init(int _hp)
        {
            maxHp = _hp;
            hp = maxHp;

            UpdateHealth();
        }

        public void OnDamageEvent(int _value)
        {
            if (_value <= 0) return;

            _value -= shield;

            if(_value > 0)
            {
                hp -= _value;
            }

            UpdateHealth();

            if (hp <= 0) dieEvent?.Invoke();
        }

        public void OnHealEvent(int _value)
        {
            //회복 불가 디버프가 있다면 체크

            _value = Mathf.Min(_value + hp, maxHp);
            hp = _value;
        }

        public void OnShieldEvent(int _value)
        {
            shield += _value;
        }

        public int GetHealth() => hp;
        public int GetMaxHealth() => maxHp;

        public void UpdateHealth()
        {
            healthGuage.fillAmount = hp / (float)maxHp;
            healthValue.text = hp + " / " + maxHp;
        }
    }
}
