
using System.Diagnostics;

class Program {
    static void Main()
    {
        var mig = new Player();
    }
}

public class Player {

    private int level;
    private int cantripTotal;

    public Player()
    {
        level = 1;
        cantripTotal = 4;
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
            getCantripTotal();
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
            getCantripTotal();
            displayLevel();
        } else
        {
            Console.WriteLine("Character is the lowest level; down leveling isn't possible.");
        }
    }

    private int getCantripTotal()
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

} 
