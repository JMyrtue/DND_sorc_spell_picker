using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace spell_randomizer
{
    public class LoadManager
    {
        public LoadManager() 
        { 
        }

        public Player determineCharacter()
        {
            Console.WriteLine("Are you starting a new sorcerer?");
            var input = Console.ReadLine();
            input = input.ToLower();
            while(input != "y" && input != "n" && input != "yes" && input != "no")
            {
                Console.WriteLine("Incorrect input - type y/n.");
                Console.WriteLine("Are you starting a new character?");
                input = Console.ReadLine();
                input.ToLower();
            }

            if (input == "y" || input == "yes")
            {
                return new Player();
            }

            var level = 0;
            while (level < 1 && level > 20)
            {
                Console.WriteLine("What level is your sorcerer?");
                var levelInput = Console.ReadLine();
                while(Int32.TryParse(levelInput, out level))
                {
                    Console.WriteLine("Give input as integer.");
                    Console.WriteLine("What level is your sorcerer?");
                    levelInput = Console.ReadLine();
                }

            }

            var wildMagicCounter = 0;
            while (wildMagicCounter < 1 && wildMagicCounter > 20) 
            {
                Console.WriteLine("What is your current Wild Magic counter?");
                var WMinput = Console.ReadLine();
                while(Int32.TryParse(WMinput, out wildMagicCounter))
                {
                    Console.WriteLine("Give input as integer.");
                    Console.WriteLine("What is your current Wild Magic counter?");
                    WMinput = Console.ReadLine();
                }
            }

            int sorcPointsUsed;
            Console.WriteLine("How many Sorcery Points have you currently used?");
            var usedPoints = Console.ReadLine();
            while(Int32.TryParse(usedPoints, out sorcPointsUsed))
            {
                Console.WriteLine("Give input as integer.");
                Console.WriteLine("How many Sorcery Points have you currently used?");
                usedPoints = Console.ReadLine();
            }
        }
    }
}
