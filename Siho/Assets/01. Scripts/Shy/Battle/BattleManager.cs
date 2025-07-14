using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Shy.Event;

namespace Shy
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance;

        public Character player, enemy;
        private float playerCurrentTime, enemyCurrentTime, regenerationTimer;
        [SerializeField] private GameObject battlePanel;
        [Tooltip("x : player / y : enemy")]
        [SerializeField] private Vector2Int eventPercent;

        private bool nowFight = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            battlePanel.SetActive(false);
        }

        #region Characters
        public Character GetCharacter(Team _target) => (_target == Team.Player) ? player : enemy;
        public Character[] GetCharacters() => new[] { player, enemy };
        #endregion

        #region Event
        private IEnumerator CheckEvent(int _v)
        {
            yield return new WaitForSeconds(1.5f);

            if (_v <= eventPercent.x)
            {
                EventManager.Instance.SetBattleEvent(_v);
            }
            else if (_v > 20 - eventPercent.y)
            {

                EventManager.Instance.ShowMessage("���۽����� Ƣ��� ������ ���ݴ��ߴ�.");
                StartCoroutine(Delay(() => SurpriseAttack(enemy, player), 1.5f));
                StartCoroutine(Delay(EventManager.Instance.HideMessage, 2.3f));
            }
            else
            {
                EventManager.Instance.HideAllPanel();
                BeginBattle();
            }
        }

        public void UserBattleEvent(BattleEvent _bEvent, int _per)
        {
            bool _success = Random.Range(0, 101) <= _per;

            switch (_bEvent)
            {
                case BattleEvent.Run:
                    if (_success)
                    {
                        EventManager.Instance.ShowMessage("���� �˾������� ����\n�ڸ��� ���ߴ�.");
                        StartCoroutine(Delay(EndBattle, 0.5f));
                    }
                    else
                    {
                        EventManager.Instance.ShowMessage("����ĥ ���� ã�� ��\n������ �߰��Ǿ���.");
                        StartCoroutine(Delay(() => BeginBattle(), 2f));
                    }
                    break;

                case BattleEvent.Surprise:
                    if (_success)
                    {
                        EventManager.Instance.ShowMessage("����� ������� ����\n�˾������� ���� �����ߴ�.");
                        StartCoroutine(Delay(() => SurpriseAttack(player, enemy), 1.1f));
                    }
                    else
                    {
                        EventManager.Instance.ShowMessage("�����Ͽ����� ���� ����� ���������� ���ߴ�.");
                        StartCoroutine(Delay(BeginBattle, 1.5f));
                    }
                    break;

                case BattleEvent.Talk:
                    if (_success)
                    {
                        if (Random.Range(0, 10) < 3)
                        {
                            //����
                            EventManager.Instance.ShowMessage("���� ���� �԰�\n�����ƴ�.");
                            StartCoroutine(Delay(EndBattle, 0.5f));
                        }
                        else
                        {
                            //���
                            EventManager.Instance.ShowMessage("�����ִ� ��뿡��\n���� ������ ���ߴ�.");
                            StartCoroutine(Delay(() => SurpriseAttack(player, enemy), 1.1f));
                        }
                    }
                    else
                    {
                        EventManager.Instance.ShowMessage("���� ���� �Ͽ�����\n���ҷο�� ���⸸ �ߴ�.");
                        StartCoroutine(Delay(() => BeginBattle(), 2f));
                    }
                    break;
            }
        }

        #endregion

        #region Battle
        public void InitBattle(CharacterDataSO _player, CharacterDataSO _enemy)
        {
            battlePanel.SetActive(true);

            player.Init(_player);
            enemy.Init(_enemy.Init());

            player.UseSynergy();
            enemy.UseSynergy();

            UnityEvent<int> _diceEvent = new();
            _diceEvent.AddListener((int _v) => StartCoroutine(CheckEvent(_v)));

            StartCoroutine(Delay(() => EventManager.Instance.DiceRoll(_diceEvent), 1.5f));
        }

        private void BeginBattle()
        {
            EventManager.Instance.HideMessage();

            SetNextAttackTime(Team.All);
            nowFight = true;
            regenerationTimer = Time.time + 1;
        }
        
        private void EndBattle(Team _winner)
        {
            nowFight = false;

            if (_winner == Team.Player)
            {
                EndBattle();
            }
            else
            {
                EndingManager.Instance.PlayerDead(enemy.transform);
            }
        }

        private void EndBattle()
        {
            nowFight = false;
            StartCoroutine(BattlePanelOff());
        }

        private IEnumerator BattlePanelOff()
        {
            yield return new WaitForSeconds(3f);

            battlePanel.SetActive(false);
        }

        private void Update()
        {
            if (nowFight)
            {
                if (playerCurrentTime < Time.time)
                {
                    Attack(player, enemy);
                }
                else if (enemyCurrentTime < Time.time)
                {
                    Attack(enemy, player);
                }

                if (nowFight && Time.time > regenerationTimer)
                {
                    player.Regeneration();
                    enemy.Regeneration();
                    regenerationTimer = Time.time + 1;
                }
            }
        }
        #endregion

        #region Attack
        private void SurpriseAttack(Character _user, Character _target)
        {
            Attack result = new();

            _user.VisualUpdate(false);
            StartCoroutine(Delay(() => _user.VisualUpdate(true), 0.35f));
            result.dmg = GetDamage(_user, _target);

            _target.HitEvent(result);

            if (_target.DieCheck())
            {
                EndBattle(_user.team);
            }
            else
            {
                BeginBattle();
            }
        }

        public void SetNextAttackTime(Team _team)
        {
            if (_team != Team.Player) enemyCurrentTime = Time.time + (1 / enemy.GetNowStat(SubStatEnum.AtkSpd));
            if (_team != Team.Enemy) playerCurrentTime = Time.time + (1 / player.GetNowStat(SubStatEnum.AtkSpd));
        }

        private float DefCalc(float _dmg, float _def, float _reduceDmg)
        {
            _dmg = _dmg - (_def * 0.5f);
            _dmg *= (1 - _reduceDmg);
            return _dmg;
        }

        private float GetDamage(Character _user, Character _target)
        {
            float _dmg = _user.GetNowStat(SubStatEnum.Atk);
            _dmg *= (1 + _user.GetNowStat(SubStatEnum.AddDmg));

            _dmg = DefCalc(_dmg, _target.GetNowStat(SubStatEnum.Def), _target.GetNowStat(SubStatEnum.RedDmg));
            _dmg += _user.GetNowStat(SubStatEnum.TrueDmg);

            return _dmg;
        }

        private Attack GetAttackData(Character _user, Character _target)
        {
            Attack result = new();

            float hitValue = _user.GetNowStat(SubStatEnum.HitChance) - _target.GetNowStat(SubStatEnum.DodgeChance);

            if(Random.Range(0, 10000) < hitValue * 100)
            {
                result.attackResult = AttackResult.Dodge;
                return result;
            }

            float dmg = GetDamage(_user, _target);

            if(dmg < 0)
            {
                result.attackResult = AttackResult.Block;
                return result;
            }

            if(Random.Range(0, 10000) < _user.GetNowStat(SubStatEnum.CriChance) * 100)
            {
                result.attackResult = AttackResult.Critical;
                dmg *= (1 + _user.GetNowStat(SubStatEnum.CriDmg));
            }

            result.dmg = dmg;
            return result;
        }

        private void Attack(Character _user, Character _target)
        {
            Attack result = GetAttackData(_user, _target);
            _user.VisualUpdate(false);
            StartCoroutine(Delay(() => _user.VisualUpdate(true), 0.5f));

            _target.HitEvent(result);

            if(!_target.characteristic.isNotBlood) _user.Drain(result.dmg);

            if (_target.DieCheck()) EndBattle(_user.team);

            if(_target.Counter())
            {
                switch (_target.team)
                {
                    case Team.Player:
                        playerCurrentTime = 0;
                        break;
                    case Team.Enemy:
                        enemyCurrentTime = 0;
                        break;
                }
            }

            SetNextAttackTime(_user.team);
        }
        #endregion

        private IEnumerator Delay(UnityAction _action, float _delay)
        {
            yield return new WaitForSeconds(_delay);
            _action.Invoke();
        }
    }
}