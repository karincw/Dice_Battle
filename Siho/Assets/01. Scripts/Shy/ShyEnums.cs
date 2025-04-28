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

    [System.Serializable]
    public enum ActionWay
    {
        None,
        Self,
        Opposite,//������
        Random, //������
        All, //��ü
        LessHp,
        MoreHp,
        Already,
        Fast,
        Slow,
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
       Buff,
       DmgText,
       end
    }

    public enum AttackMotion
    {
        Near = 0,
        Long,
        Self,
        PetSelf,
        PetAndShort,
        PetAndLong,
        SummonAndShort,
        SummonAndLong,
        DropAttack,
    }
}
