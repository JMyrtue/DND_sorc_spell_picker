using System.Security.Cryptography;

public class Player {

    private string name;
    private int level;
    private int cantripsTotal;
    private int spellsTotal;
    private int spellTableUpperBoundÍndex;
    private int wildMagicCounter;
    private int[] spellSlotsTotal;
    private int[] spellSlotsUsed;
    private Random randomomizer;
    private MagicManager magicManager;



    public string Name{ get { return name; } set { name = value; } }
    public Player()
    {
        level = 1;
        cantripsTotal = getCantripsTotal();
        spellsTotal = getSpellsTotal();
        spellTableUpperBoundÍndex = getSpellsIndexUpperBound();
        wildMagicCounter = 1;
        spellSlotsTotal = new int[] { 2, 0, 0, 0, 0, 0, 0, 0, 0 };
        spellSlotsUsed = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        randomomizer = new Random();
        magicManager = new MagicManager();
    }

    public void longRest()
    {
        Console.WriteLine("The new magical abilities of " + name + " are:\n");
        Console.WriteLine(">>Cantrips:<<");
        getCantrips();
        Console.WriteLine("\n>>Spells:<<");
        getSpells();
        Console.WriteLine();

        wildMagicCounter = 1;

        resetSpellSlots();
    }

    private void resetSpellSlots()
    {
        for (int i = 0; i < spellSlotsUsed.Length; i++)
        {
            spellSlotsUsed[i] = 0;
        }
    }

    private void incrementWildMagicCounter()
    {
        wildMagicCounter++;
    }

    public void castSpell()
    {
        int currentMaxSpell = 0;
        int highestIndex = spellSlotsTotal.Length;

        while(currentMaxSpell == 0)
        {
            highestIndex--;
            currentMaxSpell = spellSlotsTotal[highestIndex];
        }
        var MaxLevelSpell = highestIndex + 1;

        Console.WriteLine("What is the level of the spell casted? - keep in mind {0}'s highest spell level is {1}", name, MaxLevelSpell);
        var spellLevel = Convert.ToInt32(Console.ReadLine());

        while(spellLevel < 0 || spellLevel > MaxLevelSpell)
        {
            Console.WriteLine("Invalid spelllevel, must be between 1 and {0} - try again", MaxLevelSpell);
            spellLevel = Convert.ToInt32(Console.ReadLine());
        }

        if (spellSlotsUsed[spellLevel - 1] == spellSlotsTotal[spellLevel - 1])
        {
            Console.WriteLine("All spell slots used for this level - longrest to gain new spellslots");
        } else
        {
            spellSlotsUsed[spellLevel - 1]++;
        }

        Console.Clear();
        var WSinput = "";
        Console.WriteLine("Your current counter for triggering a Wild Magic Surge is >>{0}<<\n", wildMagicCounter +
            "Did your spellcast trigger a Wild Magic Surge? (y/n)");
        while(WSinput != "y" && WSinput != "n")
        {
            WSinput = Console.ReadLine();
            WSinput = WSinput.ToLower();
        }
        if(WSinput == "y")
        {
            wildMagicCounter = 1;
            Console.WriteLine("Magic Magic counter have been reset");
        } else if (WSinput == "n"){
            incrementWildMagicCounter();
        } else
        {
            throw new Exception("Unintentional input read during spellcast");
        }

    }

    public void displayLevel()
    {
        Console.WriteLine(name + " is now level " + level + "\n");
    }
    public void levelUp()
    {
        if (level <= 20)
        {
            level++;
            cantripsTotal = getCantripsTotal();
            spellsTotal = getSpellsTotal();
            spellTableUpperBoundÍndex = getSpellsIndexUpperBound();
            setSpellSlots();
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
            setSpellSlots();
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
        } else if (level >= 10 && level <= 20)
        { 
            temp = 6;
        } else
        {
            throw new Exception("Unvalid level provided");
        }
        return temp;
    }

    private void getCantrips()
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
            magicManager.getUniqueCantrip(i + 1);
        }
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

    private void setSpellSlots()
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

    private int getSpellsIndexUpperBound()
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

    private void getSpells()
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
            magicManager.getUniqueSpell(i + 1);
        }
    }

    class MagicManager
    {
        public MagicManager() { }
        public void getUniqueSpell(int index)
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
                35 => "Aganazzar's Scorcher - Spell level 2",
                36 => "Air Bubble - Spell level 2",
                37 => "Alter Self - Spell level 2",
                38 => "Arcane Hacking (UA) - Spell level 2",
                39 => "Blindness/Deafness - Spell level 2",
                40 => "Blur - Spell level 2",
                41 => "Cloud of Daggers - Spell level 2",
                42 => "Crown of Madness - Spell level 2",
                43 => "Darkness - Spell level 2",
                44 => "Darkvision - Spell level 2",
                45 => "Detect Thoughts - Spell level 2",
                46 => "Digital Phantom (UA) - Spell level 2",
                47 => "Dragon's Breath - Spell level 2",
                48 => "Dust Devil - Spell level 2",
                49 => "Earthbind - Spell level 2",
                50 => "Enhance Ability - Spell level 2",
                51 => "Enlarge/Reduce - Spell level 2",
                52 => "Find Vehicle (UA) - Spell level 2",
                53 => "Flame Blade - Spell level 2",
                54 => "Flaming Sphere - Spell level 2",
                55 => "Gust of Wind - Spell level 2",
                56 => "Hold Person - Spell level 2",
                57 => "Icingdeath's Frost (UA) - Spell level 2",
                58 => "Invisibility - Spell level 2",
                59 => "Kinetic Jaunt - Spell level 2",
                60 => "Knock - Spell level 2",
                61 => "Levitate - Spell level 2",
                62 => "Magic Weapon - Spell level 2",
                63 => "Maximillian's Earthen Grasp - Spell level 2",
                64 => "Mental Barrier (UA) - Spell level 2",
                65 => "Mind Spike - Spell level 2",
                66 => "Mind Thrust (UA) - Spell level 2",
                67 => "Mirror Image - Spell level 2",
                68 => "Misty Step - Spell level 2",
                69 => "Nathair's Mischief - Spell level 2",
                70 => "Phantasmal Force - Spell level 2",
                71 => "Pyrotechnics - Spell level 2",
                72 => "Rime's Binding Ice - Spell level 2",
                73 => "Scorching Ray - Spell level 2",
                74 => "See Invisibility - Spell level 2",
                75 => "Shadow Blade - Spell level 2",
                76 => "Shatter - Spell level 2",
                77 => "Snilloc's Snowball Storm - Spell level 2",
                78 => "Spider Climb - Spell level 2",
                79 => "Spray of Cards (UA) - Spell level 2",
                80 => "Suggestion - Spell level 2",
                81 => "Tasha's Mind Whip - Spell level 2",
                82 => "Thought Shield (UA) - Spell level 2",
                83 => "Vortex Warp - Spell level 2",
                84 => "Warding Wind - Spell level 2",
                85 => "Web - Spell level 2",
                86 => "Wither and Bloom - Spell level 2",
                87 => "Antagonize (UA) - Spell level 3",
                88 => "Ashardalon's Stride - Spell level 3",
                89 => "Blink - Spell level 3",
                90 => "Catnap - Spell level 3",
                91 => "Clairvoyance - Spell level 3",
                92 => "Conjure Lesser Demon (UA) - Spell level 3",
                93 => "Counterspell - Spell level 3",
                94 => "Daylight - Spell level 3",
                95 => "Dispel Magic - Spell level 3",
                96 => "Enemies Abound - Spell level 3",
                97 => "Erupting Earth - Spell level 3",
                98 => "Fear - Spell level 3",
                99 => "Fireball - Spell level 3",
                100 => "Flame Arrows - Spell level 3",
                101 => "Flame Stride (UA) - Spell level 3",
                102 => "Fly - Spell level 3",
                103 => "Freedom of the Waves (HB) - Spell level 3",
                104 => "Gaseous Form - Spell level 3",
                105 => "Haste - Spell level 3",
                106 => "Haywire (UA) - Spell level 3",
                107 => "House of Cards (UA) - Spell level 3",
                108 => "Hypnotic Pattern - Spell level 3",
                109 => "Incite Greed - Spell level 3",
                110 => "Intellect Fortress - Spell level 3",
                111 => "Invisibility To Cameras (UA) - Spell level 3",
                112 => "Lightning Bolt - Spell level 3",
                113 => "Major Image - Spell level 3",
                114 => "Melf's Minute Meteors - Spell level 3",
                115 => "Protection from Ballistics (UA) - Spell level 3",
                116 => "Protection from Energy - Spell level 3",
                117 => "Psionic Blast (UA) - Spell level 3",
                118 => "Sleet Storm - Spell level 3",
                119 => "Slow - Spell level 3",
                120 => "Stinking Cloud - Spell level 3",
                121 => "Summon Warrior Spirit (UA) - Spell level 3",
                122 => "Thunder Step - Spell level 3",
                123 => "Tongues - Spell level 3",
                124 => "Vampiric Touch - Spell level 3",
                125 => "Wall of Water - Spell level 3",
                126 => "Water Breathing - Spell level 3",
                127 => "Water Walk - Spell level 3",
                128 => "Banishment - Spell level 4",
                129 => "Blight - Spell level 4",
                130 => "Charm Monster - Spell level 4",
                131 => "Confusion - Spell level 4",
                132 => "Conjure Barlgura (UA) - Spell level 4",
                133 => "Conjure Knowbot (UA) - Spell level 4",
                134 => "Conjure Shadow Demon (UA) - Spell level 4",
                135 => "Dimension Door - Spell level 4",
                136 => "Dominate Beast - Spell level 4",
                137 => "Ego Whip (UA) - Spell level 4",
                138 => "Fire Shield - Spell level 4",
                139 => "Greater Invisibility - Spell level 4",
                140 => "Ice Storm - Spell level 4",
                141 => "Polymorph - Spell level 4",
                142 => "Raulothim's Psychic Lance - Spell level 4",
                143 => "Sickening Radiance - Spell level 4",
                144 => "Spirit of Death (UA) - Spell level 4",
                145 => "Stoneskin - Spell level 4",
                146 => "Storm Sphere - Spell level 4",
                147 => "Synchronicity (UA) - Spell level 4",
                148 => "System Backdoor (UA) - Spell level 4",
                149 => "Vitriolic Sphere - Spell level 4",
                150 => "Wall of Fire - Spell level 4",
                151 => "Watery Sphere - Spell level 4",
                152 => "Animate Objects - Spell level 5",
                153 => "Bigby's Hand - Spell level 5",
                154 => "Cloudkill - Spell level 5",
                155 => "Commune with City (UA) - Spell level 5",
                156 => "Cone of Cold - Spell level 5",
                157 => "Conjure Vrock (UA) - Spell level 5",
                158 => "Control Winds - Spell level 5",
                159 => "Creation - Spell level 5",
                160 => "Dominate Person - Spell level 5",
                161 => "Enervation - Spell level 5",
                162 => "Far Step - Spell level 5",
                163 => "Freedom of the Winds (HB) - Spell level 5",
                164 => "Hold Monster - Spell level 5",
                165 => "Immolation - Spell level 5",
                166 => "Insect Plague - Spell level 5",
                167 => "Seeming - Spell level 5",
                168 => "Shutdown (UA) - Spell level 5",
                169 => "Skill Empowerment - Spell level 5",
                170 => "Summon Draconic Spirit - Spell level 5",
                171 => "Synaptic Static - Spell level 5",
                172 => "Telekinesis - Spell level 5",
                173 => "Teleportation Circle - Spell level 5",
                174 => "Wall of Light - Spell level 5",
                175 => "Wall of Stone - Spell level 5",
                176 => "Arcane Gate - Spell level 6",
                177 => "Chain Lightning - Spell level 6",
                178 => "Circle of Death - Spell level 6",
                179 => "Disintegrate - Spell level 6",
                180 => "Eyebite - Spell level 6",
                181 => "Fizban's Platinum Shield - Spell level 6",
                182 => "Flesh to Stone - Spell level 6",
                183 => "Globe of Invulnerability - Spell level 6",
                184 => "Investiture of Flame - Spell level 6",
                185 => "Investiture of Ice - Spell level 6",
                186 => "Investiture of Stone - Spell level 6",
                187 => "Investiture of Wind - Spell level 6",
                188 => "Mass Suggestion - Spell level 6",
                189 => "Mental Prison - Spell level 6",
                190 => "Move Earth - Spell level 6",
                191 => "Otherworldly Form (UA) - Spell level 6",
                192 => "Otiluke's Freezing Sphere - Spell level 6",
                193 => "Psychic Crush (UA) - Spell level 6",
                194 => "Scatter - Spell level 6",
                195 => "Sunbeam - Spell level 6",
                196 => "Tasha's Otherworldly Guise - Spell level 6",
                197 => "True Seeing - Spell level 6",
                198 => "Conjure Hezrou (UA) - Spell level 7",
                199 => "Crown of Stars - Spell level 7",
                200 => "Delayed Blast Fireball - Spell level 7",
                201 => "Draconic Transformation - Spell level 7",
                202 => "Dream of the Blue Veil - Spell level 7",
                203 => "Etherealness - Spell level 7",
                204 => "Finger of Death - Spell level 7",
                205 => "Fire Storm - Spell level 7",
                206 => "Plane Shift - Spell level 7",
                207 => "Power Word: Pain - Spell level 7",
                208 => "Prismatic Spray - Spell level 7",
                209 => "Reverse Gravity - Spell level 7",
                210 => "Teleport - Spell level 7",
                211 => "Abi-Dalzim's Horrid Wilting - Spell level 8",
                212 => "Demiplane - Spell level 8",
                213 => "Dominate Monster - Spell level 8",
                214 => "Earthquake - Spell level 8",
                215 => "Incendiary Cloud - Spell level 8",
                216 => "Power Word: Stun - Spell level 8",
                217 => "Sunburst - Spell level 8",
                218 => "Blade of Disaster - Spell level 9",
                219 => "Gate - Spell level 9",
                220 => "Mass Polymorph - Spell level 9",
                221 => "Meteor Swarm - Spell level 9",
                222 => "Power Word: Kill - Spell level 9",
                223 => "Psychic Scream - Spell level 9",
                224 => "Time Stop - Spell level 9",
                225 => "Wish - Spell level 9",
            };
            Console.WriteLine(spell);
        }

        public void getUniqueCantrip(int index)
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
    }

} 