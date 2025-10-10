using System.Text.Json;

namespace spell_randomizer;

public class Player {
    public string Name { get; set; }
    public int Level { get; set; }
    public int CantripsTotal { get; set; }
    public int SpellsTotal { get; set; }
    public int SpellTableUpperBoundIndex { get; set; }
    public int WildMagicCounter { get; set; }
    public int SorcPointsUsed { get; set; }
    public int MaxSorcPoints { get; set; }
    public int[] SpellSlotsTotal { get; set; }
    public int[] SpellSlotsUsed { get; set; }
    public List<String> Cantrips { get; set; }
    public List<String> Spells { get; set; }

    private readonly Random _randomizer;
    private readonly MagicManager _magicManager;
    private readonly LevelChangeManager _levelManager;

    public Player()
    {
        _randomizer = new Random();
        _magicManager = new MagicManager();
        _levelManager = new LevelChangeManager();

        Name = string.Empty;
        SpellSlotsTotal = new int[9];
        SpellSlotsUsed = new int[9];
        Spells = new List<string>();
        Cantrips = new List<string>();
    }
    public Player(string name)
    {
        _randomizer = new Random();
        _magicManager = new MagicManager();
        _levelManager = new LevelChangeManager();

        Name = name;
        Level = 1;
        CantripsTotal = _levelManager.GetCantripsTotal(Level);
        SpellsTotal = _levelManager.GetSpellsTotal(Level);
        SpellTableUpperBoundIndex = _levelManager.GetSpellsIndexUpperBound(Level);
        WildMagicCounter = 1;
        SorcPointsUsed = 0;
        MaxSorcPoints = _levelManager.GetMaxSorcPoints(Level);
        SpellSlotsTotal = [2, 0, 0, 0, 0, 0, 0, 0, 0];
        SpellSlotsUsed = [0, 0, 0, 0, 0, 0, 0, 0, 0];
        Spells = new List<string>();
        Cantrips = new List<string>();
        GetSpells();
        GetCantrips();
    }

    public void LevelUp()
    {
        if (Level < 20)
        {
            Level++;
            CantripsTotal = _levelManager.GetCantripsTotal(Level);
            SpellsTotal = _levelManager.GetSpellsTotal(Level);
            SpellTableUpperBoundIndex = _levelManager.GetSpellsIndexUpperBound(Level);
            _levelManager.SetSpellSlots(Level, SpellSlotsTotal);
            MaxSorcPoints = _levelManager.GetMaxSorcPoints(Level);
            DisplayLevel();
        }
        else
        {
            Console.WriteLine("Character is already highest possible level");
        }
    }

    public void LevelDown()
    {
        if (Level > 1)
        {
            Level--;
            CantripsTotal = _levelManager.GetCantripsTotal(Level);
            SpellsTotal = _levelManager.GetSpellsTotal(Level);
            SpellTableUpperBoundIndex = _levelManager.GetSpellsIndexUpperBound(Level);
            _levelManager.SetSpellSlots(Level, SpellSlotsTotal);
            MaxSorcPoints = _levelManager.GetMaxSorcPoints(Level);
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
        var maxLevelSpell = GetCurrentMaxSpellLevel();
         
        Console.WriteLine("What is the level of the spell casted? - keep in mind {0}'s highest spell level is {1}", Name, maxLevelSpell);

        var spellLevel = 0;
        while (!Int32.TryParse(Console.ReadLine(), out spellLevel))
        {
            Console.WriteLine("Invalid input - provide integer for spell level.");
            Console.WriteLine("What is the level of the spell casted? - keep in mind {0}'s highest spell level is {1}", Name, maxLevelSpell);
        }

        while (spellLevel <= 0 || spellLevel > maxLevelSpell)
        {
            Console.WriteLine("Invalid spelllevel, must be between 1 and {0} - try again", maxLevelSpell);
            while (!Int32.TryParse(Console.ReadLine(), out spellLevel))
            {
                Console.WriteLine("Invalid input - provide integer for spell level.");
                Console.WriteLine("What is the level of the spell casted? - keep in mind {0}'s highest spell level is {1}", Name, maxLevelSpell);
            }
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
        var wSinput = "";
        Console.WriteLine("Your current counter for triggering a Wild Magic Surge is {0}", WildMagicCounter +
            "\nDid your spellcast trigger a Wild Magic Surge? (y/n)");
        while (wSinput != "y" && wSinput != "n")
        {
            wSinput = Console.ReadLine() ?? "";
            wSinput = wSinput.ToLower();
        }
        if (wSinput == "y")
        {
            WildMagicCounter = 1;
            Console.WriteLine("Magic Magic counter have been reset");
        } else if (wSinput == "n") {
            IncrementWildMagicCounter();
        } else
        {
            throw new Exception("Unintentional input read during spellcast");
        }

    }

    public void FlexCast_PointsToSlots(int spellSlot)
    {
        var sorcPointsNeeded = SlotToPointConversion(spellSlot);

        if( sorcPointsNeeded > (MaxSorcPoints - SorcPointsUsed) )
        {
            Console.WriteLine("Not enough Sorcery Points for the conversion");
        } else
        {
            SorcPointsUsed += sorcPointsNeeded;
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

    public void MetaMagic(int sorcPointsCost)
    {
        if(Level < 3)
        {
            Console.WriteLine("Level not high enough for meta magic.");
            return;
        }

        if(MaxSorcPoints - SorcPointsUsed >= sorcPointsCost )
        {
            SorcPointsUsed += sorcPointsCost;
        } else
        {
            Console.WriteLine("Not enough Sorcery Points for the particular Metamagic");
        }
    }

    public void Save()
    {
        var filePath = $"../../../saved_characters/{Name.ToLower()}.json";

        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(this, options);
        File.WriteAllText(filePath, json);

        Console.WriteLine($"Player data saved to {filePath}");
    }
    
    public static Player Load(string playerName)
    {
        var filepath = $"../../../saved_characters/{playerName.ToLower()}.json";
        if (!File.Exists(filepath))
            throw new FileNotFoundException("Save file not found.", playerName);

        string json = File.ReadAllText(filepath);
        Player? player = JsonSerializer.Deserialize<Player>(json);

        if (player == null)
            throw new Exception("Failed to deserialize player data.");

        Console.WriteLine($"Player {playerName} loaded!");
        return player;
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
        Cantrips.Clear();
        var indexList = new List<int>();
        int index;

        while (indexList.Count < CantripsTotal)
        {
            index = _randomizer.Next(30);
            if (!indexList.Contains(index))
            {
                indexList.Add(index);
            }
        }

        foreach (int i in indexList)
        {
            var cantrip = _magicManager.GetUniqueCantrip(i + 1);
            Cantrips.Add(cantrip);
            Console.WriteLine(cantrip);
        }
    }

    private void GetSpells()
    {
        Spells.Clear();
        var indexList = new List<int>();

        while (indexList.Count < SpellsTotal)
        {
            var index = _randomizer.Next(SpellTableUpperBoundIndex);
            if (!indexList.Contains(index))
            {
                indexList.Add(index);
            }
        }

        foreach (var i in indexList)
        {
            var spell = _magicManager.GetUniqueSpell(i + 1);
            Spells.Add(spell);
            Console.WriteLine(spell);
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

    public void DisplayCantripsAndSpells()
    {
        Console.WriteLine(">>Cantrips<<");
        foreach (var cantrip in Cantrips)
        {
            Console.WriteLine(cantrip);
        }
        
        Console.WriteLine("\n>>Spells<<:");
        foreach (var spell in Spells)
        {
            Console.WriteLine(spell);
        }

    }
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