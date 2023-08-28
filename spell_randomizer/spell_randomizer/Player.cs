using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Threading.Channels;

public class Player {

    private string name;
    private int level;
    private int cantripsTotal;
    private int spellsTotal;
    private int spellTableUpperBoundÍndex;
    private int wildMagicCounter;
    private int SorcPointsUsed;
    private int maxSorcPoints;
    private int[] spellSlotsTotal;
    private int[] spellSlotsUsed;
    private Random randomomizer;
    private MagicManager magicManager;
    private LevelChangeManager levelManager;

    public string Name { get { return name; } set { name = value; } }
    public int[] SpellSlotsTotal
    {
        get { return spellSlotsTotal; }
        set { spellSlotsTotal = value; }
    }


    public Player()
    {
        randomomizer = new Random();
        magicManager = new MagicManager();
        levelManager = new LevelChangeManager();

        level = 1;
        cantripsTotal = levelManager.GetCantripsTotal(level);
        spellsTotal = levelManager.GetSpellsTotal(level);
        spellTableUpperBoundÍndex = levelManager.GetSpellsIndexUpperBound(level);
        wildMagicCounter = 1;
        SorcPointsUsed = 0;
        maxSorcPoints = 0;
        spellSlotsTotal = new int[] { 2, 0, 0, 0, 0, 0, 0, 0, 0 };
        spellSlotsUsed = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    public void LevelUp()
    {
        if (level < 20)
        {
            level++;
            cantripsTotal = levelManager.GetCantripsTotal(level);
            spellsTotal = levelManager.GetSpellsTotal(level);
            spellTableUpperBoundÍndex = levelManager.GetSpellsIndexUpperBound(level);
            levelManager.SetSpellSlots(level, SpellSlotsTotal);
            maxSorcPoints = level switch {
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
            level--;
            cantripsTotal = levelManager.GetCantripsTotal(level);
            spellsTotal = levelManager.GetSpellsTotal(level);
            spellTableUpperBoundÍndex = levelManager.GetSpellsIndexUpperBound(level);
            levelManager.SetSpellSlots(level, SpellSlotsTotal);
            DisplayLevel();
        }
        else
        {
            Console.WriteLine("Character is the lowest level; down leveling isn't possible.");
        }
    }

    public void LongRest()
    {
        Console.WriteLine("The new magical abilities of " + name + " are:\n");
        Console.WriteLine(">>Cantrips:<<");
        GetCantrips();
        Console.WriteLine("\n>>Spells:<<");
        GetSpells();
        Console.WriteLine();

        wildMagicCounter = 1;
        SorcPointsUsed = 0;
        ResetSpellSlots();

    }


    public void CastSpell()
    {
        var MaxLevelSpell = GetCurrentMaxSpellLevel();

        Console.WriteLine("What is the level of the spell casted? - keep in mind {0}'s highest spell level is {1}", name, MaxLevelSpell);
        var spellLevel = Convert.ToInt32(Console.ReadLine());

        while (spellLevel < 0 || spellLevel > MaxLevelSpell)
        {
            Console.WriteLine("Invalid spelllevel, must be between 1 and {0} - try again", MaxLevelSpell);
            spellLevel = Convert.ToInt32(Console.ReadLine());
        }

        if (spellSlotsUsed[spellLevel - 1] == spellSlotsTotal[spellLevel - 1])
        {
            Console.WriteLine("All spell slots used for this level - longrest to gain new spellslots");
            return;
        } else
        {
            spellSlotsUsed[spellLevel - 1]++;
        }

        Console.Clear();
        var WSinput = "";
        Console.WriteLine("Your current counter for triggering a Wild Magic Surge is {0}", wildMagicCounter +
            "\nDid your spellcast trigger a Wild Magic Surge? (y/n)");
        while (WSinput != "y" && WSinput != "n")
        {
            WSinput = Console.ReadLine();
            WSinput = WSinput.ToLower();
        }
        if (WSinput == "y")
        {
            wildMagicCounter = 1;
            Console.WriteLine("Magic Magic counter have been reset");
        } else if (WSinput == "n") {
            IncrementWildMagicCounter();
        } else
        {
            throw new Exception("Unintentional input read during spellcast");
        }

    }

    public void FlexCast_PointsToSlots()
    {
        var spellSlot = getConversionSpellSlot();
        if(spellSlot == 0)
        {
            Console.WriteLine("Conversion cancelled");
            return;
        }

        var SorcPointsNeeded = SlotToPointConversion(spellSlot);

        if( SorcPointsNeeded > (maxSorcPoints - SorcPointsUsed) )
        {
            Console.WriteLine("Not enough Sorcery Points for the conversion");
        } else
        {
            SorcPointsUsed += SorcPointsNeeded;
            spellSlotsUsed[spellSlot - 1]--;
        }

    }

    public void FlexCast_SlotsToPoints()
    {
        var spellSlot = getConversionSpellSlot();
        if (spellSlot == 0)
        {
            Console.WriteLine("Conversion cancelled");
            return;
        }

        if (spellSlotsUsed[spellSlot - 1] >= spellSlotsTotal[spellSlot - 1])
        {
            Console.WriteLine("No spellslots left of level {0} - conversion cancelled", spellSlot);
        }
        else
        {
            spellSlotsUsed[spellSlot - 1]++;
            SorcPointsUsed -= SlotToPointConversion(spellSlot);
        }
    }

    private int getConversionSpellSlot()
    {
        Console.WriteLine("What spellslot level are you converting to/from?");
        int slot = Convert.ToInt32(Console.ReadLine());

        while (slot > 5 && slot < 0)
        {
            Console.Clear();
            Console.WriteLine("Only permitted to convert with spellslots of levels 1-5.\n" +
                "If you wish to cancel the conversion give a '0' input.");
            slot = Convert.ToInt32(Console.ReadLine());
        }

        return slot;
    }

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

    //Private helper functions
    private void GetCantrips()
    {
        var indexList = new List<int>();
        int index;

        while (indexList.Count < cantripsTotal)
        {
            index = randomomizer.Next(30);
            if (!indexList.Contains(index))
            {
                indexList.Add(index);
            }
        }

        foreach (int i in indexList)
        {
            magicManager.GetUniqueCantrip(i + 1);
        }
    }

    private void GetSpells()
    {
        var indexList = new List<int>();
        int index;

        while (indexList.Count < spellsTotal)
        {
            index = randomomizer.Next(spellTableUpperBoundÍndex);
            if (!indexList.Contains(index))
            {
                indexList.Add(index);
            }
        }

        foreach (var i in indexList)
        {
            magicManager.GetUniqueSpell(i + 1);
        }
    }

    private void ResetSpellSlots()
    {
        for (int i = 0; i < spellSlotsUsed.Length; i++)
        {
            spellSlotsUsed[i] = 0;
        }
    }

    private void IncrementWildMagicCounter()
    {
        wildMagicCounter++;
    }

    private int GetCurrentMaxSpellLevel()
    {
        int highestindex = this.spellSlotsTotal.Length;
        var maxFound = false;

        while (!maxFound)
        {
            highestindex--;
            if (spellSlotsTotal[highestindex] != 0)
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
            Console.WriteLine("Spellslot level: {0} - {1} available spellslots", i + 1, spellSlotsTotal[i] - spellSlotsUsed[i]);
        }
    }

    public void DisplaySorcPoints()
    {
        Console.WriteLine("{0} has {1} remaing Sorcery Points.", name, maxSorcPoints - SorcPointsUsed);
    }

    public void DisplayLevel()
    {
        Console.WriteLine(this.Name + " is now level " + level);
    }
}