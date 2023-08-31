using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Threading.Channels;

public class Player {

    private string name;
    public string Name { get { return name; } set { name = value; } }

    private int level;
    public int Level { get { return level; } private set { level = value; } }
    private int cantripsTotal;
    public int CantripsTotal { get { return cantripsTotal; } private set { cantripsTotal = value; } }
    private int spellsTotal;
    public int SpellsTotal { get { return spellsTotal; } private set { spellsTotal = value;  } }
    private int spellTableUpperBoundIndex;
    public int SpellTableUpperBoundIndex { get { return spellTableUpperBoundIndex; } private set { spellTableUpperBoundIndex = value; } }
    private int wildMagicCounter;
    public int WildMagicCounter { get { return wildMagicCounter; } private set { wildMagicCounter = value; } }
    private int sorcPointsUsed;
    public int SorcPointsUsed { get { return sorcPointsUsed;  } private set { sorcPointsUsed = value; } }
    private int maxSorcPoints;
    public int MaxSorcPoints { get { return maxSorcPoints; } private set { maxSorcPoints = value; } }
    private int[] spellSlotsTotal; 
    public int[] SpellSlotsTotal { get { return spellSlotsTotal; } set { spellSlotsTotal = value; } }
    private int[] spellSlotsUsed;
    public int[] SpellSlotsUsed { get { return spellSlotsUsed; } private set { spellSlotsUsed = value; } }

    private Random randomomizer;
    private MagicManager magicManager;
    private LevelChangeManager levelManager;

    public Player()
    {
        randomomizer = new Random();
        magicManager = new MagicManager();
        levelManager = new LevelChangeManager();

        Level = 1;
        CantripsTotal = levelManager.GetCantripsTotal(level);
        SpellsTotal = levelManager.GetSpellsTotal(level);
        SpellTableUpperBoundIndex = levelManager.GetSpellsIndexUpperBound(level);
        WildMagicCounter = 1;
        SorcPointsUsed = 0;
        MaxSorcPoints = 0;
        SpellSlotsTotal = new int[] { 2, 0, 0, 0, 0, 0, 0, 0, 0 };
        SpellSlotsUsed = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    public void LevelUp()
    {
        if (level < 20)
        {
            Level++;
            CantripsTotal = levelManager.GetCantripsTotal(Level);
            SpellsTotal = levelManager.GetSpellsTotal(Level);
            SpellTableUpperBoundIndex = levelManager.GetSpellsIndexUpperBound(Level);
            levelManager.SetSpellSlots(Level, SpellSlotsTotal);
            MaxSorcPoints = Level switch {
                1 => 0,
                _ => level
            };
            DisplayLevel();
        }
        else
        {
            Console.WriteLine("Character is already highest possible level");
        }
    }

    public void LevelDown()
    {
        if (level > 1)
        {
            Level--;
            CantripsTotal = levelManager.GetCantripsTotal(Level);
            SpellsTotal = levelManager.GetSpellsTotal(Level);
            SpellTableUpperBoundIndex = levelManager.GetSpellsIndexUpperBound(Level);
            levelManager.SetSpellSlots(Level, SpellSlotsTotal);
            MaxSorcPoints = Level switch
            {
                1 => 0,
                _ => level
            };
            DisplayLevel();
        }
        else
        {
            Console.WriteLine("Character is the lowest level; down leveling isn't possible.");
        }
    }

    public void LongRest()
    {
        Console.WriteLine("The new magical abilities of " + Name + " are:\n");
        Console.WriteLine(">>Cantrips:<<");
        GetCantrips();
        Console.WriteLine("\n>>Spells:<<");
        GetSpells();
        Console.WriteLine();

        WildMagicCounter = 1;
        SorcPointsUsed = 0;
        ResetSpellSlots();
    }


    public void CastSpell()
    {
        var MaxLevelSpell = GetCurrentMaxSpellLevel();

        Console.WriteLine("What is the level of the spell casted? - keep in mind {0}'s highest spell level is {1}", Name, MaxLevelSpell);
        var spellLevel = Convert.ToInt32(Console.ReadLine());

        while (spellLevel < 0 || spellLevel > MaxLevelSpell)
        {
            Console.WriteLine("Invalid spelllevel, must be between 1 and {0} - try again", MaxLevelSpell);
            spellLevel = Convert.ToInt32(Console.ReadLine());
        }

        if (SpellSlotsUsed[spellLevel - 1] == SpellSlotsTotal[spellLevel - 1])
        {
            Console.WriteLine("All spell slots used for this level - longrest to gain new spellslots");
            return;
        } else
        {
            SpellSlotsUsed[spellLevel - 1]++;
        }

        Console.Clear();
        var WSinput = "";
        Console.WriteLine("Your current counter for triggering a Wild Magic Surge is {0}", WildMagicCounter +
            "\nDid your spellcast trigger a Wild Magic Surge? (y/n)");
        while (WSinput != "y" && WSinput != "n")
        {
            WSinput = Console.ReadLine();
            WSinput = WSinput.ToLower();
        }
        if (WSinput == "y")
        {
            WildMagicCounter = 1;
            Console.WriteLine("Magic Magic counter have been reset");
        } else if (WSinput == "n") {
            IncrementWildMagicCounter();
        } else
        {
            throw new Exception("Unintentional input read during spellcast");
        }

    }

    public void FlexCast_PointsToSlots(int spellSlot)
    {
        var SorcPointsNeeded = SlotToPointConversion(spellSlot);

        if( SorcPointsNeeded > (MaxSorcPoints - SorcPointsUsed) )
        {
            Console.WriteLine("Not enough Sorcery Points for the conversion");
        } else
        {
            SorcPointsUsed += SorcPointsNeeded;
            SpellSlotsUsed[spellSlot - 1]--;
        }

    }

    public void FlexCast_SlotsToPoints(int spellSlot)
    {

        if (SpellSlotsUsed[spellSlot - 1] >= SpellSlotsTotal[spellSlot - 1])
        {
            Console.WriteLine("No spellslots left of level {0} - conversion cancelled", spellSlot);
        }
        else
        {
            SpellSlotsUsed[spellSlot - 1]++;
            SorcPointsUsed -= SlotToPointConversion(spellSlot);
        }
    }

    //Private helper functions
    private int SlotToPointConversion(int slot)
    {
        return slot switch
        {
            1 => 2,
            2 => 3,
            3 => 5,
            4 => 6,
            5 => 7,
            _ => 0
        };
    }

    private void GetCantrips()
    {
        var indexList = new List<int>();
        int index;

        while (indexList.Count < CantripsTotal)
        {
            index = randomomizer.Next(30);
            if (!indexList.Contains(index))
            {
                indexList.Add(index);
            }
        }

        foreach (int i in indexList)
        {
            Console.WriteLine(magicManager.GetUniqueCantrip(i + 1));
        }
    }

    private void GetSpells()
    {
        var indexList = new List<int>();
        int index;

        while (indexList.Count < SpellsTotal)
        {
            index = randomomizer.Next(SpellTableUpperBoundIndex);
            if (!indexList.Contains(index))
            {
                indexList.Add(index);
            }
        }

        foreach (var i in indexList)
        {
            Console.WriteLine(magicManager.GetUniqueSpell(i + 1));
        }
    }

    private void ResetSpellSlots()
    {
        for (int i = 0; i < SpellSlotsUsed.Length; i++)
        {
            SpellSlotsUsed[i] = 0;
        }
    }

    private void IncrementWildMagicCounter()
    {
        WildMagicCounter++;
    }

    private int GetCurrentMaxSpellLevel()
    {
        int highestindex = SpellSlotsTotal.Length;
        var maxFound = false;

        while (!maxFound)
        {
            highestindex--;
            if (SpellSlotsTotal[highestindex] != 0)
            {
                maxFound = true;
            }
        }

        return highestindex + 1;
    }

    // Inner 'Printer' class
    public void DisplaySpellSlots()
    {
        var currentMax = GetCurrentMaxSpellLevel();
        for (int i = 0; i < currentMax; i++)
        {
            Console.WriteLine("Spellslot level: {0} - {1} available spellslots", i + 1, SpellSlotsTotal[i] - SpellSlotsUsed[i]);
        }
    }

    public void DisplaySorcPoints()
    {
        Console.WriteLine("{0} has {1} remaing Sorcery Points.", Name, MaxSorcPoints - SorcPointsUsed);
    }

    public void DisplayLevel()
    {
        Console.WriteLine(this.Name + " is now level " + Level);
    }
}