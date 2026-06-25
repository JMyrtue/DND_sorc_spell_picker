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
    public List<Spell> Cantrips { get; set; }
    public List<Spell> Spells { get; set; }

    private readonly Random _randomizer;
    private readonly MagicManager _magicManager;
    private readonly LevelChangeManager _levelManager;

    public Player()
    {
        _randomizer = new Random();
        _magicManager = new MagicManager();
        _levelManager = new LevelChangeManager(_magicManager);

        Name = string.Empty;
        SpellSlotsTotal = new int[9];
        SpellSlotsUsed = new int[9];
        Spells = new List<Spell>();
        Cantrips = new List<Spell>();
    }
    public Player(string name)
    {
        _randomizer = new Random();
        _magicManager = new MagicManager();
        _levelManager = new LevelChangeManager(_magicManager);

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
        Spells = new List<Spell>();
        Cantrips = new List<Spell>();
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
        }
    }

    public void LongRest()
    {
        GetCantrips();
        GetSpells();
        WildMagicCounter = 1;
        SorcPointsUsed = 0;
        ResetSpellSlots();
    }


    public ActionResult CastSpell(int spellLevel, bool wildMagicSurgeOccurred)
    {
        var maxLevelSpell = GetCurrentMaxSpellLevel();

        if (spellLevel <= 0 || spellLevel > maxLevelSpell)
        {
             return new ActionResult(false, $"Invalid spell level, must be between 1 and {maxLevelSpell}");
        }

        if (SpellSlotsUsed[spellLevel - 1] >= SpellSlotsTotal[spellLevel - 1])
        {
            return new ActionResult(false, "All spell slots used for this level. Long rest to regain spell slots.");
        } 
        
        SpellSlotsUsed[spellLevel - 1]++;

        if (wildMagicSurgeOccurred)
        {
            WildMagicCounter = 1;
        }
        else
        {
            IncrementWildMagicCounter();
        }
        
        return new ActionResult(true, "Spell cast successfully.");
    }
    
    public Spell? GetSpell(string spellName)
    {
        return Cantrips.FirstOrDefault(s => s.Name.Equals(spellName, StringComparison.OrdinalIgnoreCase)) ??
               Spells.FirstOrDefault(s => s.Name.Equals(spellName, StringComparison.OrdinalIgnoreCase));
    }


    public ActionResult FlexCast_PointsToSlots(int spellSlot)
    {
        var sorcPointsNeeded = SlotToPointConversion(spellSlot);

        if( sorcPointsNeeded > (MaxSorcPoints - SorcPointsUsed) )
        {
            return new ActionResult(false, "Not enough Sorcery Points for the conversion");
        } else
        {
            SorcPointsUsed += sorcPointsNeeded;
            SpellSlotsUsed[spellSlot - 1]--;
            return new ActionResult(true, $"{sorcPointsNeeded} sorcery points converted to level {spellSlot} spellslot!");
        }

    }

    public ActionResult FlexCast_SlotsToPoints(int spellSlot)
    {

        if (SpellSlotsUsed[spellSlot - 1] >= SpellSlotsTotal[spellSlot - 1])
        {
            return new ActionResult(false, $"No spellslots left of level {spellSlot} - conversion cancelled");
        }
        else
        {
            SpellSlotsUsed[spellSlot - 1]++;
            var sorc_points = SlotToPointConversion(spellSlot);
            SorcPointsUsed -= sorc_points;
            return new ActionResult(true, $"Level {spellSlot} spellslot converted to {sorc_points} sorcery points!");
        }
    }

    public ActionResult MetaMagic(int sorcPointsCost)
    {
        if(Level < 3)
        {
            return new ActionResult(false, "Level not high enough for meta magic.");
        }

        if(MaxSorcPoints - SorcPointsUsed >= sorcPointsCost )
        {
            SorcPointsUsed += sorcPointsCost;
             return new ActionResult(true, "Metamagic used");
        } else
        {
            return new ActionResult(false, "Not enough Sorcery Points for the particular Metamagic");
        }
    }

    public void Save()
    {
        var directory = Path.Combine(Directory.GetCurrentDirectory(), "saved_characters");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        var filePath = Path.Combine(directory, $"{Name.ToLower()}.json");

        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(this, options);
        File.WriteAllText(filePath, json);
    }
    
    public static Player Load(string playerName)
    {
        var filepath = Path.Combine(Directory.GetCurrentDirectory(), "saved_characters", $"{playerName.ToLower()}.json");
        if (!File.Exists(filepath))
            throw new FileNotFoundException("Save file not found.", playerName);

        string json = File.ReadAllText(filepath);
        Player? player = JsonSerializer.Deserialize<Player>(json);

        if (player == null)
            throw new Exception("Failed to deserialize player data.");

        // If spells list is empty, it's an old save file, so we give it a long rest to get new spells.
        if (player.Spells.Count == 0 && player.SpellsTotal > 0)
        {
            player.GetSpells();
            player.GetCantrips();
        }

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

        int totalCantrips = _magicManager.GetTotalCantripsCount();

        while (indexList.Count < CantripsTotal)
        {
            index = _randomizer.Next(totalCantrips);
            if (!indexList.Contains(index))
            {
                indexList.Add(index);
            }
        }

        foreach (int i in indexList)
        {
            var cantrip = _magicManager.GetUniqueCantrip(i + 1);
            if (cantrip != null)
            {
                Cantrips.Add(cantrip);
            }
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
            if (spell != null)
            {
                Spells.Add(spell);
            }
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
}

public class ActionResult
{
    public bool Success { get; }
    public string Message { get; }

    public ActionResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}
