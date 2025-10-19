using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;

public class LevelChangeManager{
    
    public LevelChangeManager()
    {

    }

    public int GetSpellsIndexUpperBound(int level)
    {
        var indexUpperBound = level switch
        {
            1 or 2 => 34,                // 34 1st level spells
            3 or 4 => 86,                // 52 2nd level spells
            5 or 6 => 127,               // 41 3rd level spells
            7 or 8 => 151,               // 24 4th level spells
            9 or 10 => 175,              // 24 5th level spells
            11 or 12 => 197,             // 22 6th level spells
            13 or 14 => 210,             // 13 7th level spells
            15 or 16 => 217,             // 7 8th level spells
            17 or 18 or 19 or 20 => 225, // 8 9th level spells
            _ => throw new Exception("Undefined level for determining index bound of spell retrieval"),
        };
        return indexUpperBound;
    }

    public int GetCantripsTotal(int level)
    {
        int temp;

        if (level >= 1 && level < 4)
        {
            temp = 4;
        }
        else if (level >= 4 && level < 10)
        {
            temp = 5;
        }
        else if (level >= 10 && level <= 20)
        {
            temp = 6;
        }
        else
        {
            throw new Exception("Unvalid level provided");
        }
        return temp;
    }

    public int GetSpellsTotal(int level)
    {
        var totalSpells = level switch
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
            17 or 18 or 19 or 20 => 15,
            _ => throw new Exception("Undefined level"),
        };
        return totalSpells;
    }

    public void SetSpellSlots(int level, int[] spellSlotsTotal)
    {
        switch (level) //Each case shortend to only change the spell slots, that could be affected from leveling up or down
        {
            case 1:
                spellSlotsTotal[0] = 2;
                break;
            case 2:
                spellSlotsTotal[0] = 3;
                spellSlotsTotal[1] = 0;
                break;
            case 3:
                spellSlotsTotal[0] = 4;
                spellSlotsTotal[1] = 2;
                break;
            case 4:
                spellSlotsTotal[1] = 3;
                spellSlotsTotal[2] = 0;
                break;
            case 5:
                spellSlotsTotal[2] = 2;
                break;
            case 6:
                spellSlotsTotal[2] = 3;
                spellSlotsTotal[3] = 0;
                break;
            case 7:
                spellSlotsTotal[3] = 1;
                break;
            case 8:
                spellSlotsTotal[3] = 2;
                spellSlotsTotal[4] = 0;
                break;
            case 9:
                spellSlotsTotal[3] = 3;
                spellSlotsTotal[4] = 1;
                break;
            case 10:
                spellSlotsTotal[4] = 2;
                spellSlotsTotal[5] = 0;
                break;
            case 11:
                spellSlotsTotal[5] = 1;
                break;
            case 12:
                spellSlotsTotal[6] = 0;
                break;
            case 13:
                spellSlotsTotal[6] = 1;
                break;
            case 14:
                spellSlotsTotal[7] = 0;
                break;
            case 15:
                spellSlotsTotal[7] = 1;
                break;
            case 16:
                spellSlotsTotal[8] = 0;
                break;
            case 17:
                spellSlotsTotal[4] = 2;
                spellSlotsTotal[8] = 1;
                break;
            case 18:
                spellSlotsTotal[4] = 3;
                spellSlotsTotal[5] = 1;
                break;
            case 19:
                spellSlotsTotal[5] = 2;
                spellSlotsTotal[6] = 1;
                break;
            case 20:
                spellSlotsTotal[6] = 2;
                break;
            default: throw new Exception(string.Format("Invalid level: {0} used for getting spellSlots", level));
        }


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
