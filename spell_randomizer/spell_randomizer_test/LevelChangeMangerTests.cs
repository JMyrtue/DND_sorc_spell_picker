using Xunit.Sdk;

namespace spell_randomizer_test
{
    public class LevelChangeMangerTests
    {
        [Theory]
        [InlineData(1, 34)]
        [InlineData(5, 127)]
        [InlineData(8, 151)]
        [InlineData(13, 210)]
        [InlineData(17, 225)]
        [InlineData(20, 225)]
        public void GetSpellIndexUpperBound_ProvideLevel_AssertBound(int level, int expectedSpellBound)
        {
            // Arrange
            var levelManager = new LevelChangeManager();

            // Act
            var actualSpellBound = levelManager.GetSpellsIndexUpperBound(level);

            // Assert
            Assert.Equal(expectedSpellBound, actualSpellBound);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(21)]
        [InlineData(100)]
        public void GetSpellIndexUpperBound_ProvideInvalidLevel_AssertException(int level)
        {
            // Arrange
            var levelManager = new LevelChangeManager();

            // Act + Assert
            Assert.Throws<Exception>(() => levelManager.GetSpellsIndexUpperBound(level));
        }

        [Theory]
        [InlineData(1, 4)]
        [InlineData(5, 5)]
        [InlineData(8, 5)]
        [InlineData(13, 6)]
        [InlineData(20, 6)]
        public void GetCantripsTotal_ProvideLevel_AssertCantripAmount(int level, int expectedCantripAmount)
        {
            // Arrange
            var levelManager = new LevelChangeManager();

            // Act
            var actualCantripAmount = levelManager.GetCantripsTotal(level);

            // Assert
            Assert.Equal(expectedCantripAmount, actualCantripAmount);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(21)]
        [InlineData(100)]
        public void GetCantripsTotal_ProvideInvalidLevel_AssertException(int level)
        {
            // Arrange
            var levelManager = new LevelChangeManager();

            // Act + Assert
            Assert.Throws<Exception>(() => levelManager.GetCantripsTotal(level));
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(5, 6)]
        [InlineData(8, 9)]
        [InlineData(13, 13)]
        [InlineData(20, 15)]
        public void GetSpellsTotal_ProvideLevel_AssertCantripAmount(int level, int expectedSpellAmount)
        {
            // Arrange
            var levelManager = new LevelChangeManager();

            // Act
            var actualSpellAmount = levelManager.GetSpellsTotal(level);

            // Assert
            Assert.Equal(expectedSpellAmount, actualSpellAmount);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(21)]
        [InlineData(100)]
        public void GetSpellsTotal_ProvideInvalidLevel_AssertException(int level)
        {
            // Arrange
            var levelManager = new LevelChangeManager();

            // Act + Assert
            Assert.Throws<Exception>(() => levelManager.GetSpellsTotal(level));
        }

        [Fact]
        public void SetSpellSlots_AssertingSpellSlotsAfterEachLevelUp()
        {
            // The SetSpellSlots() method is called whenever a character levels up - not called directly in this test case.
            
            // Level 1
            var player= new Player();
            var expectedSpellSlots = new int[] { 2, 0, 0, 0, 0, 0, 0, 0, 0 };
            var actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 2
            expectedSpellSlots[0] = 3;
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 3
            expectedSpellSlots[0] = 4;
            expectedSpellSlots[1] = 2;
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 4
            expectedSpellSlots[1] = 3;
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 5
            expectedSpellSlots[2] = 2;
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 6
            expectedSpellSlots[2] = 3;
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 7
            expectedSpellSlots[3] = 1;
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 8
            expectedSpellSlots[3] = 2;
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 9
            expectedSpellSlots[3] = 3;
            expectedSpellSlots[4] = 1;
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 10
            expectedSpellSlots[4] = 2;
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 11
            expectedSpellSlots[5] = 1;
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 12
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 13
            expectedSpellSlots[6] = 1;
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 14
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 15
            expectedSpellSlots[7] = 1;
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 16
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 17
            expectedSpellSlots[8] = 1;
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 18
            expectedSpellSlots[4] = 3;
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 19
            expectedSpellSlots[5] = 2;
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 20
            expectedSpellSlots[6] = 2;
            player.LevelUp();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
        }

        [Fact]
        public void SetSpellSlots_AssertingSpellSlotsAfterEachLevelDown()
        {
            var player = new Player();

            for(int i = 1; i < 20; i++)
            {
                player.LevelUp();
            }

            // Level 20
            var expectedSpellSlots = new int[] { 4, 3, 3, 3, 3, 2, 2, 1, 1};
            var actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 19
            expectedSpellSlots[6] = 1;
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 18
            expectedSpellSlots[5] = 1;
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 17
            expectedSpellSlots[4] = 2;
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 16
            expectedSpellSlots[8] = 0;
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 15
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 14
            expectedSpellSlots[7] = 0;
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 13
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 12
            expectedSpellSlots[6] = 0;
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 11
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 10
            expectedSpellSlots[5] = 0;
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 9
            expectedSpellSlots[4] = 1;
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 8
            expectedSpellSlots[4] = 0;
            expectedSpellSlots[3] = 2;
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 7
            expectedSpellSlots[3] = 1;
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);

            // Level 6
            expectedSpellSlots[3] = 0;
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 5
            expectedSpellSlots[2] = 2;
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 4
            expectedSpellSlots[2] = 0;
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 3
            expectedSpellSlots[1] = 2;
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 2
            expectedSpellSlots[1] = 0;
            expectedSpellSlots[0] = 3;
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
            
            // Level 1
            expectedSpellSlots[0] = 2;
            player.LevelDown();
            actualSpellSlots = player.SpellSlotsTotal;
            Assert.Equal(expectedSpellSlots, actualSpellSlots);
        }

    }
}