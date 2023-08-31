using Xunit.Sdk;

namespace spell_randomizer_test
{
    public class PlayerTests
    {
        [Fact]
        public void PlayerLevelUp_AssertLevel()
        {
            // Arrange
            var player = new Player();
            var expectedLevel = 1 + 1;

            // Act
            player.LevelUp();
            var actualLevel = player.Level;

            // Assert
            Assert.Equal(expectedLevel, actualLevel);
        }

        [Fact]
        public void PlayerLevelDown_AssertLevel()
        {
            // Arrange
            var player = new Player();
            var expectedLevel =  1 + 1 - 1;

            // Act
            player.LevelUp();
            player.LevelDown();
            var actualLevel = player.Level;

            // Assert
            Assert.Equal(expectedLevel, actualLevel);
        }
        
        [Fact]
        public void FlexCast_LevelUpConvertToSpellSlotLVL1_ExpectSorcPointsAndSpellSlots()
        {
            // Arrange 
            var player = new Player();
            player.LevelUp(); //Leveling up in order to gain 2 Sorcery points
            var expectedSorcPoints = 2 - 2;
            var expectedLvl1SpellSlots = 3 + 1;

            // Act
            player.FlexCast_PointsToSlots(1);
            var actualSorcPoints = player.MaxSorcPoints - player.SorcPointsUsed;
            var actualLevel1SpellSlots = player.SpellSlotsTotal[0] - player.SpellSlotsUsed[0];

            // Assert
            Assert.Equal(expectedSorcPoints, actualSorcPoints);
            Assert.Equal(expectedLvl1SpellSlots, actualLevel1SpellSlots);
        }

        [Fact]
        public void FlexCast_LevelUpConvertFromSpellSlotLVL1_ExpectSorcPointsAndSpellSlots()
        {
            // Arrange
            var player = new Player();
            player.LevelUp();
            var expectedSorcPoints = 2 + 2;
            var expectedLvl1SpellSlots = 3 - 1;

            // Act
            player.FlexCast_SlotsToPoints(1);
            var actualSorcPoints = player.MaxSorcPoints - player.SorcPointsUsed;
            var actualLevel1SpellSlots = player.SpellSlotsTotal[0] - player.SpellSlotsUsed[0];

            // Assert
            Assert.Equal(expectedSorcPoints, actualSorcPoints);
            Assert.Equal(expectedLvl1SpellSlots, actualLevel1SpellSlots);
        }

        [Fact]
        public void FlexCast_Level10ConvertToSpellSlotLVL5_ExpectSorcPointsAndSpellSlots()
        {
            // Arrange 
            var player = new Player();
            for(int i = 1; i < 10; i++)
            {
                player.LevelUp();

            }
            var expectedSorcPoints = 10 - 7;
            var expectedLvl5SpellSlots = 2 + 1;

            // Act
            player.FlexCast_PointsToSlots(5);
            var actualSorcPoints = player.MaxSorcPoints - player.SorcPointsUsed;
            var actualLevel1SpellSlots = player.SpellSlotsTotal[4] - player.SpellSlotsUsed[4];

            // Assert
            Assert.Equal(expectedSorcPoints, actualSorcPoints);
            Assert.Equal(expectedLvl5SpellSlots, actualLevel1SpellSlots);
        }

        [Fact]
        public void FlexCast_Level10ConvertFromSpellSlotLVL5_ExpectSorcPointsAndSpellSlots()
        {
            // Arrange 
            var player = new Player();
            for (int i = 1; i < 10; i++)
            {
                player.LevelUp();

            }
            var expectedSorcPoints = 10 + 7;
            var expectedLvl5SpellSlots = 2 - 1;

            // Act
            player.FlexCast_SlotsToPoints(5);
            var actualSorcPoints = player.MaxSorcPoints - player.SorcPointsUsed;
            var actualLevel1SpellSlots = player.SpellSlotsTotal[4] - player.SpellSlotsUsed[4];

            // Assert
            Assert.Equal(expectedSorcPoints, actualSorcPoints);
            Assert.Equal(expectedLvl5SpellSlots, actualLevel1SpellSlots);
        }

        [Fact]
        public void Longrest_SorcPointsReset()
        {
            // Arrange
            var player = new Player();
            player.LevelUp();
            player.FlexCast_PointsToSlots(1);
            var expectedSorcPointsAfterRest = 0 + 2;

            // Act
            player.LongRest();
            var actualSorcPointsAfterRest = player.MaxSorcPoints - player.SorcPointsUsed;

            // Assert
            Assert.Equal(expectedSorcPointsAfterRest, actualSorcPointsAfterRest);
        }
    }
}