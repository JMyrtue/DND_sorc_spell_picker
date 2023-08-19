public class Player {

    private int level;
    private int cantripsTotal;
    private int spellsTotal;
    private Random randomomizer;

    public Player()
    {
        level = 1;
        cantripsTotal = 4;
        spellsTotal = 2;
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
        int temp;

        while(indexList.Count < cantripsTotal)
        {
            temp = randomomizer.Next(30);
            if (!indexList.Contains(temp))
            {
                indexList.Add(temp);
            }
        }

        foreach (int index in indexList)
        {
            getUniqueCantrip(index + 1);
        }
    }

    private void getUniqueCantrip(int index)
    {
        string cantrip = "";
        switch (index)
        {
            case 1: cantrip = "Acid Splash";break;
            case 2: cantrip = "Blade Ward"; break;
            case 3: cantrip = "Booming Blade"; break;
            case 4: cantrip = "Chill Touch"; break;
            case 5: cantrip = "Control Flames"; break;
            case 6: cantrip = "Create Bonfire"; break;
            case 7: cantrip = "Dancing Lights"; break;
            case 8: cantrip = "Fire Bolt"; break;
            case 9: cantrip = "Friends"; break;
            case 10: cantrip = "Frostbite"; break;
            case 11: cantrip = "Green-Flame Blade"; break;
            case 12: cantrip = "Gust"; break;
            case 13: cantrip = "Infestation"; break;
            case 14: cantrip = "Light"; break;
            case 15: cantrip = "Lightning Lure"; break;
            case 16: cantrip = "Mage Hand"; break;
            case 17: cantrip = "Mending"; break;
            case 18: cantrip = "Message"; break;
            case 19: cantrip = "Mind Sliver"; break;
            case 20: cantrip = "Minor Illusion"; break;
            case 21: cantrip = "Mold Earth"; break;
            case 22: cantrip = "On/Off (UA)"; break;
            case 23: cantrip = "Poison Spray"; break;
            case 24: cantrip = "Prestidigitation"; break;
            case 25: cantrip = "Ray of Frost"; break;
            case 26: cantrip = "Shape Water"; break;
            case 27: cantrip = "Shocking Grasp"; break;
            case 28: cantrip = "Sword Burst"; break;
            case 29: cantrip = "Thunderclap"; break;
            case 30: cantrip = "True Strike"; break;
            default: throw new Exception("Invalid index provided for cantrip retrival");
        }
        Console.WriteLine(cantrip);
    }

    private int getSpellsTotal()
    {
        var temp = level switch
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
        return temp;
    }

} 
