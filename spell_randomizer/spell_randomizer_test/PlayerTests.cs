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
        public void FlexCast_LevelUpAndConvertToSpellSlotLVL1_SorcPointsReset()
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
        public void FlexCast_LevelUpAndConvertFromSpellSlotLVL1_SorcPointsReset()
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
    }
}