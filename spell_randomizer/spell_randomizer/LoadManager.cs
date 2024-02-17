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
            Console.Clear();
            while (input != "y" && input != "n" && input != "yes" && input != "no")
            {
                Console.WriteLine("Incorrect input - type y/n.");
                Console.WriteLine("Are you starting a new character?");
                input = Console.ReadLine();
                input.ToLower();
                Console.Clear();
            }
            Console.WriteLine("What is the name of your new sorcerer?");
            var name = Console.ReadLine();
            Console.Clear();
            if (name == "")
            {
                Console.WriteLine("Your sorcerer must have a name, please enter a name.");
                name = Console.ReadLine();
                Console.Clear();
            }
            if (input == "y" || input == "yes")
            {
                return new Player(name);
            }

            var level = 0;
            while (level < 1 || level > 20)
            {
                Console.WriteLine("What level is your sorcerer?");
                var levelInput = Console.ReadLine();
                while(!Int32.TryParse(levelInput, out level))
                {
                    Console.WriteLine("Give input as integer.");
                    Console.WriteLine("What level is your sorcerer?");
                    levelInput = Console.ReadLine();
                    Console.Clear();
                }

            }
            var character = new Player(name);
            for(; level > 1; level--)
            {
                character.LevelUp();
            }
            Console.Clear();

            var wildMagicCounter = 0;
            while (wildMagicCounter < 1 || wildMagicCounter > 20) 
            {
                Console.WriteLine("What is your current Wild Magic counter?");
                var WMinput = Console.ReadLine();
                Console.Clear();
                while (!Int32.TryParse(WMinput, out wildMagicCounter))
                {
                    Console.WriteLine("Give input as integer.");
                    Console.WriteLine("What is your current Wild Magic counter?");
                    WMinput = Console.ReadLine();
                    Console.Clear();
                }
            }
            character.WildMagicCounter = wildMagicCounter;

            int sorcPointsRemaining;
            Console.WriteLine("How many Sorcery Points do you have left?");
            var remainingPoints = Console.ReadLine();
            while(!Int32.TryParse(remainingPoints, out sorcPointsRemaining))
            {
                Console.WriteLine("Give input as integer.");
                Console.WriteLine("How many Sorcery Points do you have left?");
                remainingPoints = Console.ReadLine();
                Console.Clear();
            }
            character.SorcPointsUsed = character.MaxSorcPoints - sorcPointsRemaining;

            for(int i = 1; i <= character.SpellSlotsTotal.Length; i++)
            {
                if(character.SpellSlotsTotal[i - 1] > 0)
                {
                    Console.WriteLine("How many spell slots of level {0} do you have left",i);
                    int SpellSlotRemaining;
                    var tempSpellSlotRemaining = Console.ReadLine();
                    Console.Clear();
                    while (!Int32.TryParse(tempSpellSlotRemaining, out SpellSlotRemaining))
                    {
                        Console.WriteLine("Give input as integer.");
                        Console.WriteLine("How many spell slots of level {0} do you have left", i);
                        tempSpellSlotRemaining = Console.ReadLine();
                        Console.Clear();
                    }
                    character.SpellSlotsUsed[i - 1] = character.SpellSlotsTotal[i - 1] - SpellSlotRemaining;
                }
            }
            return character;
        }
    }
}
