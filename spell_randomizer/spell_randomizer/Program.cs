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

            var handledInput = input.Split(' ');
            int parameter = 0;

            var action = handledInput[0];
            if (handledInput[0] == "+ss" || handledInput[0] == "-ss" || handledInput[0] == "+spellslot" || handledInput[0] == "-spellslot")
            {
                if(!(handledInput.Length > 1))
                {
                    Console.WriteLine("Missing spellslots level!");
                    continue;
                }
                parameter = Convert.ToInt32(handledInput[1]);
                if (parameter > 5 || parameter < 1)
                {
                    Console.WriteLine("Invalid level provided for Flexibility Casting - can only convert between spellslots of level 1-5.");
                    continue;
                }

            }

            if (handledInput[0] == "mm" || handledInput[0] == "metamagic")
            {
                if (!(handledInput.Length > 1))
                {
                    Console.WriteLine("Missing cost of metamagic!");
                    continue;
                }
                parameter = Convert.ToInt32(handledInput[1]);
            }

            switch (action){
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
                case "+ss": mig.FlexCast_PointsToSlots(parameter); break;
                case "-spellslot":
                case "-ss": mig.FlexCast_SlotsToPoints(parameter); break;
                case "mm":
                case "metamagic": mig.MetaMagic(parameter); break;
                case "commands":
                    Console.WriteLine("Possible input are as follows:\n" +
                        "ding:                 increases character level\n" +
                        "delevel:              decreases character level\n" +
                        "rest/longrest/sleep:  provides a new set of cantrips and spells\n" +
                        "cast:                 casts a spell\n" +
                        "spellslots/ss:        displays remaining spellslots\n" +
                        "points:               displays remaining sorcery points\n" +
                        "+spellslot/+ss X:     convert sorcery points to a spellslot\n" +
                        "                      - where 'X' is the spellslot level converted to\n" +
                        "-spellslot/-ss X:     convert a spellslot to sorcery points\n" +
                        "                      - where 'X' is the spellslot level converted from\n" +
                        "metamagic/mm X:       cast metamagic" +
                        "                      - where 'X' is the cost of the specific metamagic" +
                        "quit:                 terminates the program\n"
                        ); break;

                default: Console.WriteLine("No valid input detected - try the \'commands\' input for further info on valid inputs.\n"); break;
            }

        }
    }
}
