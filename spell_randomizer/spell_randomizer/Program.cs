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

            switch (input){
                case "quit": playing = false; break;
                case "ding": mig.levelUp(); break;
                case "delevel": mig.levelDown(); break;
                case "rest":
                case "longrest":
                case "sleep": mig.longRest(); break;
                case "cast": mig.castSpell(); break;
                case "spellslots": mig.displaySpellSlots(); break;
                case "commands":
                    Console.WriteLine("Possible input are as follows:\n" +
                        "ding:                 increases character level\n" +
                        "delevel:              decreases character level\n" +
                        "rest/longrest/sleep:  provides a new set of cantrips and spells\n" +
                        "cast:                 casts a spell\n" +
                        "spellslots:           shows remaining spellslots\n" +
                        "quit:                 terminates the program\n"
                        ); break;

                default: Console.WriteLine("No valid input detected - try the \'commands\' input for further info on valid inputs.\n"); break;
            }

        }
    }
}
