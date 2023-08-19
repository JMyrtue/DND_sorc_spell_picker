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
            temp = randomomizer.Next(1, 31);
            if (!indexList.Contains(temp))
            {
                indexList.Add(temp);
            }
        }

        foreach (int index in indexList)
        {
            Console.WriteLine(index);
        }
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
