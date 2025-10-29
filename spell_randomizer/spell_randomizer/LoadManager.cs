namespace spell_randomizer
{
    public class LoadManager
    {
        public Player determineCharacter()
        {
            Console.WriteLine("Are you starting a new sorcerer?");
            var input = Console.ReadLine() ?? "";
            input = input.ToLower();
            Console.Clear();
            while (input != "y" && input != "n" && input != "yes" && input != "no")
            {
                Console.WriteLine("Incorrect input - type y/n.");
                Console.WriteLine("Are you starting a new character?");
                input = Console.ReadLine() ?? "";
                input = input.ToLower();
                Console.Clear();
            }
            if (input == "y" || input == "yes")
            {
                Console.WriteLine("What is the name of your sorcerer?");
                var newName = Console.ReadLine() ?? "";
                Console.Clear();
                if (newName == "")
                {
                    Console.WriteLine("Your sorcerer must have a name, please enter a name.");
                    newName = Console.ReadLine() ?? "";
                    Console.Clear();
                }
                return new Player(newName);
            }
            Console.WriteLine("What is the name of your sorcerer?");
            var name = Console.ReadLine() ?? "";
            Console.Clear();
            if (name == "")
            {
                Console.WriteLine("Your sorcerer must have a name, please enter a name.");
                name = Console.ReadLine() ?? "";
                Console.Clear();
            }
            return Player.Load(name);
        }
    }
}
