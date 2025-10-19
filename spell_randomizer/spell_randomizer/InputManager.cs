namespace spell_randomizer;

public class InputManager
{
    private readonly Player _mig;

    public InputManager(Player mig)
    {
        _mig = mig ?? throw new ArgumentNullException(nameof(mig));
    }

    public void play()
    {
        var playing = true;

        while (playing)
        {
            Console.WriteLine("\nProvide an input for next action:");
            string input = Console.ReadLine()!.ToLower();
            Console.Clear();

            var splitInput = input.Split(' ');

            var action = splitInput[0];
            var parameter = CheckActionParameter(action, splitInput);
            if (parameter == 1337)
            {
                continue;
            }
            playing = DetermineAction(action, parameter);
        }
    }

    private int CheckActionParameter(string action, string[] input)
    {
        if (action == "+ss" || action == "-ss" || action == "+spellslot" ||
            action == "-spellslot")
        {
            if (!(input.Length > 1))
            {
                Console.WriteLine("Missing spellslots level!");
                return 1337;
            }

            var parameter = Convert.ToInt32(input[1]);
            if (parameter > 5 || parameter < 1)
            {
                Console.WriteLine(
                    "Invalid level provided for Flexibility Casting - can only convert between spell slots of level 1-5.");
                return 1337;
            }

        }

        if (action == "mm" || action  == "metamagic")
        {
            if (!(input.Length > 1))
            {
                Console.WriteLine("Missing cost of metamagic!");
                return 1337;
            }

            return Convert.ToInt32(input[1]);
        }
        
        
        return input.Length < 2 ? 0 : Convert.ToInt32(input[1]);
    }
    
    private bool DetermineAction(string action, int parameter)
    {
        switch (action){
                case "quit":
                    _mig.Save(); return false;
                case "ding": _mig.LevelUp(); break;
                case "delevel": _mig.LevelDown(); break;
                case "rest":
                case "longrest":
                case "sleep": _mig.LongRest(); break;
                case "cast": _mig.CastSpell(); break;
                case "spellslots":
                case "ss": _mig.DisplaySpellSlots(); break;
                case "points": _mig.DisplaySorcPoints(); break;
                case "+spellslot":
                case "+ss": _mig.FlexCast_PointsToSlots(parameter); break;
                case "-spellslot":
                case "-ss": _mig.FlexCast_SlotsToPoints(parameter); break;
                case "mm":
                case "metamagic": _mig.MetaMagic(parameter); break;
                case "spells": _mig.DisplayCantripsAndSpells(); break;
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
                                      "                      - where 'X' is the cost of the specific metamagic\n" +
                                      "quit:                 terminates the program\n"
                    ); break;

                default: Console.WriteLine("No valid input detected - try the \'commands\' input for further info on valid inputs.\n"); break;
        }

        return true;
    }
}