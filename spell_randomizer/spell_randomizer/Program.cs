using System.ComponentModel.Design;

class Program {
    static void Main()
    {
        var mig = new Player();
        var playing = true;
        string input;

        Console.WriteLine("Welcome! What is the name of your sorcerer?");
        mig.Name = Console.ReadLine();
        Console.Clear();

        while (playing)
        {
            Console.WriteLine("\nProvide an input for next action:");
            input = Console.ReadLine().ToLower();
            Console.Clear();

            switch (input){
                case "quit": playing = false; break;
                case "ding": mig.LevelUp(); break;
                case "delevel": mig.LevelDown(); break;
                case "rest":
                case "longrest":
                case "sleep": mig.LongRest(); break;
                case "cast": mig.CastSpell(); break;
                case "spellslots":
                case "ss": mig.DisplaySpellSlots(); break;
                case "points": mig.DisplaySorcPoints(); break;
                case "+spellslot":
                case "+ss": mig.FlexCast_PointsToSlots(); break;
                case "-spellslot":
                case "-ss": mig.FlexCast_SlotsToPoints(); break;
                case "commands":
                    Console.WriteLine("Possible input are as follows:\n" +
                        "ding:                 increases character level\n" +
                        "delevel:              decreases character level\n" +
                        "rest/longrest/sleep:  provides a new set of cantrips and spells\n" +
                        "cast:                 casts a spell\n" +
                        "spellslots/ss:        displays remaining spellslots\n" +
                        "points:               displays remaining sorcery points\n" +
                        "+spellslot/+ss:       convert sorcery points to a spellslot\n" +
                        "-spellslot/-ss:       convert a spellslot to sorcery points\n" +
                        "quit:                 terminates the program\n"
                        ); break;

                default: Console.WriteLine("No valid input detected - try the \'commands\' input for further info on valid inputs.\n"); break;
            }

        }
    }
}
