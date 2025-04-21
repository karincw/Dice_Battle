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
        DropAttack,
    }
}
