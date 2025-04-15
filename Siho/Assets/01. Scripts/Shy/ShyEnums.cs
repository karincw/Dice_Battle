namespace Shy
{
    public enum Team
    {
        None,
        Player,
        Enemy
    }

    public enum EventType
    {
        AttackEvent,
        ShieldEvent,
        HealEvent,
        BuffEvent
    }

    public enum BuffEvent
    {
        Brave = 0,
        Scare,
        Poison,
        Burn,
    }

    public enum ActionWay
    {
        None,
        Self,
        Opposite,//������
        Select, //����
        Random, //������
        All //��ü
    }

    public enum StatEnum
    {
        MaxHp,
        Hp,
        Str,
        Def
    }

    public enum PoolingType
    {
       Dice = 0,
       Buff,
       DmgText,
       end
    }
}
