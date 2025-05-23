﻿using karin;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Shy
{
    public class BattleManager : MonoBehaviour
    {
        #region 변수
        public static BattleManager Instance;

        [Header("Character")]
        public List<Character> minions, enemies;
        private Dictionary<Character, List<DiceUi>> enemyDiceDic = new Dictionary<Character, List<DiceUi>>();

        [Header("Dice")]
        [SerializeField] private DiceUi dicePrefab;
        private int diceLoop;
        private List<DiceUi> dices = new List<DiceUi>(), enDices = new List<DiceUi>();
        [SerializeField] private RectTransform hand, handVisual;

        [Header("Other")]
        [SerializeField] private GameObject endBtn;
        private UnityAction buffEvent;
        #endregion

        #region Init
        private void Awake()
        {
            if (Instance != null) Destroy(this);
            else Instance = this;
        }

        private void Start()
        {
            Init();
        }

        private void CharacterInit(Team _team, CharacterSO[] _so)
        {
            List<Character> characterList = (_team == Team.Player ? minions : enemies);
            for (int i = 0; i < characterList.Count; i++)
            {
                if (i >= _so.Length || _so[i] == null)
                {
                    characterList[i].Init(_team, null);
                    characterList.RemoveAt(i--);
                    continue;
                }

                characterList[i].Init(_team, _so[i]);
                buffEvent += characterList[i].BuffCheck;
            }
        }

        public void Init()
        {
            EnemySO[] enemyDatas = karin.Core.DataLinkManager.Instance.GetEnemyData.list.ToArray();
            List<DiceSO> _diceList = DataManager.Instance.dices;
            Transform spawn = hand.Find("Groups");

            buffEvent = null;
            handVisual.sizeDelta = Vector2.zero;

            CharacterInit(Team.Player, DataManager.Instance.minions);
            CharacterInit(Team.Enemy, enemyDatas.Select(enemy => enemy.data).ToArray());

            for (int i = 0; i < _diceList.Count; i++)
            {
                DiceUi dUi = Instantiate(dicePrefab, spawn);
                dices.Add(dUi);
                dUi.Init(_diceList[i], Team.Player);
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                List<DiceUi> dUiList  = new List<DiceUi>();
                foreach (DiceSO dice in enemyDatas[i].GetDices())
                {
                    DiceUi dUi = Instantiate(dicePrefab, spawn);
                    enDices.Add(dUi); dices.Add(dUi);
                    dUi.Init(dice, Team.Enemy);
                    dUiList.Add(dUi);
                }
                enemyDiceDic.Add(enemies[i], dUiList);
            }
            
            endBtn.SetActive(false);
            buffEvent += () => StartCoroutine(TurnStart(0.7f));
            StartCoroutine(TurnStart(0));
        }
        #endregion

        #region Turn
        public IEnumerator TurnStart(float _delay)
        {
            Debug.Log("Turn Start");

            yield return new WaitForSeconds(_delay);

            //초기화
            for (int i = 0; i < dices.Count;)
            {
                if (dices[i].DiceDieCheck()) dices.RemoveAt(i);
                else i++;
            }

            //섞기
            for (int i = 0; i < 10; i++)
            {
                int rand = Random.Range(0, dices.Count);
                dices[0].transform.SetSiblingIndex(rand);
                dices.Insert(rand + 1, dices[0]);
                dices.RemoveAt(0);
            }
            
            hand.sizeDelta = new Vector2(60 + 180 * dices.Count, 200);
            float sec = 0.075f / dices.Count;

            for (int i = 0; i <= dices.Count * 10; i++)
            {
                yield return new WaitForSeconds(sec);
                //handVisual.sizeDelta = new Vector2(60 + 18 * i, 40);
                if(i % 10 == 8) dices[i / 10].RollDice();
            }
        }

        public void CheckTurn(DiceUi _dice)
        {
            if (_dice != dices[dices.Count - 1]) return;
            CanInteract.interact = true;
        }

        public void EndCheck()
        {
            Character[] _arr = new Character[enDices.Count];

            for (int i = 0; i < enDices.Count; i++)
            {
                Character c = enDices[i].user;
                if (c == null) return;

                _arr[i] = c;

                for (int j = 0; j < i; j++)
                {
                    if (_arr[j] == c)
                    {
                        enDices[j].UserReset();
                        return;
                    }
                }
            }

            endBtn.SetActive(true);
        }

        public void TurnEnd()
        {
            diceLoop = 0;

            for (int i = 0; i < dices.Count; i++)
                if (dices[i].user == null) dices[i].noUsed.SetActive(true);

            endBtn.SetActive(false);
            StartCoroutine(DiceDelay());
        }
        #endregion

        #region Dice
        private DiceUi GetCurrentDice()
        {
            for (int i = 0; i < dices.Count; i++)
            {
                if (dices[i].CanUseCheck()) return dices[i];
            }

            return null;
        }

        private void UseDice()
        {
            EyeSO eye = dices[diceLoop].UseDice();
            dices[diceLoop].user.SkillUse(eye.value, eye.attackWay, minions.ToArray(), enemies.ToArray());
        }

        public void NextAction()
        {
            if(++diceLoop >= dices.Count)
            {
                Debug.Log("모든 다이스 종료");
                buffEvent.Invoke();
                return;
            }

            StartCoroutine(DiceDelay());
        }

        private IEnumerator DiceDelay()
        {
            yield return new WaitForSeconds(0.11f);

            if (dices[diceLoop].user == null) NextAction();
            else
            {
                yield return new WaitForSeconds(0.6f);
                UseDice();
            }
        }
        #endregion

        public void SetCharacterInDice(Character _user)
        {
            DiceUi _dice = GetCurrentDice();

            if(_dice != null)
            {
                if (_dice.team != _user.team) return;

                _dice.CharacterSelect(_user);
            }
        }

        public void CharacterDie(Character _ch)
        {
            if(enemies.Contains(_ch))
            {
                enemies.Remove(_ch);

                if (enemies.Count == 0)
                {
                    SceneChanger.Instance.LoadScene("WorldMap");
                }

                for (int i = 0; i < dices.Count; i++)
                {
                    if(dices[i].user == _ch)
                    {
                        dices[i].UserReset();
                        dices[i].noUsed.SetActive(true);
                    }
                }

                foreach (DiceUi dice in enemyDiceDic[_ch])
                {
                    dice.KillDice();
                    enDices.Remove(dice);
                }
            }
            else
            {
                minions.Remove(_ch);
            }
        }
    }
}
