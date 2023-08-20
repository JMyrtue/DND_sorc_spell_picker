using System.ComponentModel.Design;

class Program {
    static void Main()
    {
        var mig = new Player();
        var playing = true;
        string input;

        Console.WriteLine("Welcome! What is the name of your sorcerer?");
        mig.Name = Console.ReadLine();

        while (playing)
        {
            Console.WriteLine("Provide an input for next action:");
            input = Console.ReadLine().ToLower();
            Console.Clear();

            if (input == "quit")
            {
                playing = false;
            } else if (input == "ding") 
            {
                mig.levelUp();
            } else if (input == "delevel")
            {
                mig.levelDown();
            } else if (input == "rest" || input == "longrest" || input == "sleep")
            {
                mig.longRest();
            } else if(input == "commands")
            {
                Console.WriteLine("Possible input are as follows:\n" +
                    "ding:                 increases character level\n" +
                    "delevel:              decreases character level\n" +
                    "rest/longrest/sleep:  provides a new set of cantrips and spells\n" +
                    "quit:                 terminates the program\n");
            } else
            {
                Console.WriteLine("No valid input detected - try the \'commands\' input for further info on valid inputs.\n");
            }

        }
        

    }
}
