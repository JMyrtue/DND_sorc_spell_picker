public class Player {

    private int level;
    private int cantripsTotal;
    private int spellsTotal;
    private int spellTableUpperBoundÍndex;
    private Random randomomizer;

    public Player()
    {
        level = 1;
        cantripsTotal = 4;
        spellsTotal = 2;
        spellTableUpperBoundÍndex = 34;
        randomomizer = new Random();
    }

    public void displayLevel()
    {
        Console.WriteLine("Character is currently level " + level);
    }
    public void levelUp()
    {
        if (level <= 20)
        {
            level++;
            cantripsTotal = getCantripsTotal();
            spellsTotal = getSpellsTotal();
            spellTableUpperBoundÍndex = getSpellsIndexUpperBound();
            displayLevel();
        } else
        {
            Console.WriteLine("Character is already highest possible level");
        }
    }

    public void levelDown()
    {
        if(level >= 1) 
        { 
            level--;
            cantripsTotal = getCantripsTotal();
            spellsTotal = getSpellsTotal();
            spellTableUpperBoundÍndex = getSpellsIndexUpperBound();
            displayLevel();
        } else
        {
            Console.WriteLine("Character is the lowest level; down leveling isn't possible.");
        }
    }

    private int getCantripsTotal()
    {
        int temp;

        if (level < 4)
        {
            temp = 4;
        } else if (level >= 4 && level < 10) 
        {
            temp = 5;
        } else if (level >= 10 &&  level <= 20)
        { 
            temp = 6;
        } else
        {
            throw new Exception("Unvalid level provided");
        }
        return temp;
    }

    public void getCantrips()
    {
        var indexList = new List<int>();
        int index;

        while(indexList.Count < cantripsTotal)
        {
            index = randomomizer.Next(30);
            if (!indexList.Contains(index))
            {
                indexList.Add(index);
            }
        }

        foreach (int i in indexList)
        {
            getUniqueCantrip(i + 1);
        }
    }

    private void getUniqueCantrip(int index)
    {
        string cantrip = "";
        cantrip = index switch
        {
            1 => "Acid Splash",
            2 => "Blade Ward",
            3 => "Booming Blade",
            4 => "Chill Touch",
            5 => "Control Flames",
            6 => "Create Bonfire",
            7 => "Dancing Lights",
            8 => "Fire Bolt",
            9 => "Friends",
            10 => "Frostbite",
            11 => "Green-Flame Blade",
            12 => "Gust",
            13 => "Infestation",
            14 => "Light",
            15 => "Lightning Lure",
            16 => "Mage Hand",
            17 => "Mending",
            18 => "Message",
            19 => "Mind Sliver",
            20 => "Minor Illusion",
            21 => "Mold Earth",
            22 => "On/Off (UA)",
            23 => "Poison Spray",
            24 => "Prestidigitation",
            25 => "Ray of Frost",
            26 => "Shape Water",
            27 => "Shocking Grasp",
            28 => "Sword Burst",
            29 => "Thunderclap",
            30 => "True Strike",
            _ => throw new Exception("Invalid index provided for cantrip retrival"),
        };
        Console.WriteLine(cantrip);
    }

    private int getSpellsTotal()
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

    private int getSpellsIndexUpperBound()
    {
        var indexUpperBound = level switch
        {
            1 or 2 => 34,                // 34 1st level spells
            3 or 4 => 87,                // 53 2nd level spells
            5 or 6 => 128,               // 41 3rd level spells
            7 or 8 => 153,               // 25 4th level spells
            9 or 10 => 178,              // 25 5th level spells
            11 or 12 => 201,             // 23 6th level spells
            13 or 14 => 215,             // 14 7th level spells
            15 or 16 => 222,             // 7 8th level spells
            17 or 18 or 19 or 20 => 230, // 8 9th level spells
            _ => throw new Exception("Undefined level for determining index bound of spell retrieval"),
        };
        return indexUpperBound;
    }

    public void getSpells()
    {
        var indexList = new List<int>();
        int index;

        while(indexList.Count < spellsTotal)
        {
            index = randomomizer.Next(spellTableUpperBoundÍndex);
            if (!indexList.Contains(index))
            {
                indexList.Add(index);
            }

        }

        foreach (var i in indexList)
        {
            getUniqueSpell(i + 1);
        }
    }

    private void getUniqueSpell(int index)
    {
        string spell = index switch
        {
            1 => "Absorb Elements - Spell level 1",
            2 => "Acid Stream (UA) - Spell level 1",
            3 => "Burning Hands - Spell level 1",
            4 => "Catapult - Spell level 1",
            5 => "Chaos Bolt - Spell level 1",
            6 => "Charm Person - Spell level 1",
            7 => "Chromatic Orb - Spell level 1",
            8 => "Color Spray - Spell level 1",
            9 => "Comprehend Languages - Spell level 1",
            10 => "Detect Magic - Spell level 1",
            11 => "Disguise Self - Spell level 1",
            12 => "Distort Value - Spell level 1",
            13 => "Earth Tremor - Spell level 1",
            14 => "Expeditious Retreat - Spell level 1",
            15 => "False Life - Spell level 1",
            16 => "Feather Fall - Spell level 1",
            17 => "Fog Cloud - Spell level 1",
            18 => "Grease - Spell level 1",
            19 => "Ice Knife - Spell level 1",
            20 => "Id Insinuation (UA) - Spell level 1",
            21 => "Infallible Relay (UA) - Spell level 1",
            22 => "Jump - Spell level 1",
            23 => "Mage Armor - Spell level 1",
            24 => "Magic Missile - Spell level 1",
            25 => "Ray of Sickness - Spell level 1",
            26 => "Remote Access (UA) - Spell level 1",
            27 => "Shield - Spell level 1",
            28 => "Silent Image - Spell level 1",
            29 => "Silvery Barbs - Spell level 1",
            30 => "Sleep - Spell level 1",
            31 => "Sudden Awakening (UA) - Spell level 1",
            32 => "Tasha's Caustic Brew - Spell level 1",
            33 => "Thunderwave - Spell level 1",
            34 => "Witch Bolt - Spell level 1",
        }; 
        Console.WriteLine(spell);
    }
} 
