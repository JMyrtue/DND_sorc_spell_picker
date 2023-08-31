namespace spell_randomizer_test
{
    public class MagicManagerTests
    {
        [Theory]
        [InlineData(1, "Absorb Elements - Spell level 1")]
        [InlineData(15, "False Life - Spell level 1")]
        [InlineData(37, "Alter Self - Spell level 2")]
        [InlineData(51, "Enlarge/Reduce - Spell level 2")]
        [InlineData(100, "Flame Arrows - Spell level 3")]
        [InlineData(121, "Summon Warrior Spirit (UA) - Spell level 3")]
        [InlineData(225, "Wish - Spell level 9")]

        public void GetUniqueSpell_ReturnsCorrectSpell_AssertSpell(int index, string expectedSpell)
        {
            // Arrange
            var magicManager = new MagicManager();

            // Act
            var actualSpell = magicManager.GetUniqueSpell(index);

            // Assert
            Assert.Equal(expectedSpell, actualSpell);
        }

        [Fact]
        public void GetUniqueSpell_InvalidIndex_AssertException() 
        {
            // Arrange
            var magicManager = new MagicManager();

            // Act + Assert
            Assert.Throws<Exception>(()  => magicManager.GetUniqueSpell(300));
        }

        [Theory]
        [InlineData(1, "Acid Splash")]
        [InlineData(5, "Control Flames")]
        [InlineData(12, "Gust")]
        [InlineData(21, "Mold Earth")]
        [InlineData(30, "True Strike")]
        public void GetUniqueCantrip_ReturnsCorrectCantrip_AssertCantrip(int index, string expectedCantrip)
        {
            // Arrange
            var magicManager = new MagicManager();

            // Act 
            var actualCantrip = magicManager.GetUniqueCantrip(index);

            // Assert
            Assert.Equal(expectedCantrip, actualCantrip);
        }

        [Fact]
        public void GetUniqueCantrip_InvalidIndex_AssertException()
        {
            // Arrange
            var magicManager = new MagicManager();

            // Act + Assert
            Assert.Throws<Exception>(() => magicManager.GetUniqueCantrip(50));
        }
    }
}