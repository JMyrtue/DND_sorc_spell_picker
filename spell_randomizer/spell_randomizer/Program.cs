namespace spell_randomizer;

class Program {
    static void Main()
    {
        var loadManager = new LoadManager();
        var mig = loadManager.determineCharacter();
        var inputManager = new InputManager(mig);

        try
        {
            inputManager.play();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            mig.Save();
            throw;
        }
    }
}