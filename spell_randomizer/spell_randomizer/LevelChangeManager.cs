namespace spell_randomizer;

public class LevelChangeManager{
    
    private readonly MagicManager _magicManager;

    public LevelChangeManager(MagicManager magicManager)
    {
        _magicManager = magicManager;
    }

    public int GetSpellsIndexUpperBound(int level)
    {
        int maxSpellLevel = level switch
        {
            >= 17 => 9,
            >= 15 => 8,
            >= 13 => 7,
            >= 11 => 6,
            >= 9 => 5,
            >= 7 => 4,
            >= 5 => 3,
            >= 3 => 2,
            _ => 1
        };
        return _magicManager.GetSpellsIndexUpperBound(maxSpellLevel);
    }

    public int GetCantripsTotal(int level)
    {
        return level switch
        {
            >= 10 => 6,
            >= 4 => 5,
            >= 1 => 4,
            _ => throw new Exception("Unvalid level provided")
        };
    }

    public int GetSpellsTotal(int level)
    {
        return level switch
        {
            1 => 2,
            2 => 3,
            3 => 4,
            4 => 5,
            5 => 6,
            6 => 7,
            7 => 8,
            8 => 9,
            9 => 10,
            10 => 11,
            11 or 12 => 12,
            13 or 14 => 13,
            15 or 16 => 14,
            >= 17 => 15,
            _ => throw new Exception("Undefined level"),
        };
    }

    public void SetSpellSlots(int level, int[] spellSlotsTotal)
    {
        // Reset all slots to 0 before setting them.
        for (int i = 0; i < spellSlotsTotal.Length; i++)
        {
            spellSlotsTotal[i] = 0;
        }

        if (level >= 1) spellSlotsTotal[0] = 2;
        if (level >= 2) spellSlotsTotal[0] = 3;
        if (level >= 3) { spellSlotsTotal[0] = 4; spellSlotsTotal[1] = 2; }
        if (level >= 4) spellSlotsTotal[1] = 3;
        if (level >= 5) spellSlotsTotal[2] = 2;
        if (level >= 6) spellSlotsTotal[2] = 3;
        if (level >= 7) spellSlotsTotal[3] = 1;
        if (level >= 8) spellSlotsTotal[3] = 2;
        if (level >= 9) { spellSlotsTotal[3] = 3; spellSlotsTotal[4] = 1; }
        if (level >= 10) spellSlotsTotal[4] = 2;
        if (level >= 11) spellSlotsTotal[5] = 1;
        if (level >= 13) spellSlotsTotal[6] = 1;
        if (level >= 15) spellSlotsTotal[7] = 1;
        if (level >= 17) spellSlotsTotal[8] = 1;
        if (level >= 18) spellSlotsTotal[4] = 3;
        if (level >= 19) spellSlotsTotal[5] = 2;
        if (level >= 20) spellSlotsTotal[6] = 2;
    }

    public int GetMaxSorcPoints(int level)
    {
        return level switch
        {
            1 => 0,
            _ => level
        };
    }
}
