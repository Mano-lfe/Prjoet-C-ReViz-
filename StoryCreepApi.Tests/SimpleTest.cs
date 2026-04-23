using Xunit;

public class StoryHelpersTests
{
    // ── TESTS CLEAN ───────────────────────────────────────────

    [Fact]
    public void Clean_ReturnsValue_WhenNotEmpty()
    {
        // Arrange
        string input    = "  Mano  ";
        string fallback = "[non précisé]";

        // Act
        string result = StoryHelpers.Clean(input, fallback);

        // Assert
        Assert.Equal("Mano", result);
    }

    [Fact]
    public void Clean_ReturnsFallback_WhenEmpty()
    {
        // Arrange
        string input    = "";
        string fallback = "[non précisé]";

        // Act
        string result = StoryHelpers.Clean(input, fallback);

        // Assert
        Assert.Equal("[non précisé]", result);
    }

    [Fact]
    public void Clean_ReturnsFallback_WhenNull()
    {
        // Arrange
        string? input   = null;
        string fallback = "[non précisé]";

        // Act
        string result = StoryHelpers.Clean(input, fallback);

        // Assert
        Assert.Equal("[non précisé]", result);
    }

    [Fact]
    public void Clean_ReturnsFallback_WhenWhitespace()
    {
        // Arrange
        string input    = "   ";
        string fallback = "[non précisé]";

        // Act
        string result = StoryHelpers.Clean(input, fallback);

        // Assert
        Assert.Equal("[non précisé]", result);
    }

    // ── TESTS CONTAINSANY ─────────────────────────────────────

    [Fact]
    public void ContainsAny_ReturnsTrue_WhenKeywordPresent()
    {
        // Arrange
        string source = "Je veux tester ma méthode";

        // Act
        bool result = StoryHelpers.ContainsAny(source, "tester");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ContainsAny_ReturnsFalse_WhenNoKeywordPresent()
    {
        // Arrange
        string source = "Je ne sais pas";

        // Act
        bool result = StoryHelpers.ContainsAny(source, "xunit", "nunit", "mstest");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsAny_IsCaseInsensitive()
    {
        // Arrange
        string source = "J'utilise XUNIT pour mes tests";

        // Act
        bool result = StoryHelpers.ContainsAny(source, "xunit");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ContainsAny_ReturnsTrue_WhenOneOfManyKeywordsPresent()
    {
        // Arrange
        string source = "On arrange les données avant le test";

        // Act
        bool result = StoryHelpers.ContainsAny(source, "arrange", "act", "assert");

        // Assert
        Assert.True(result);
    }

    // ── TESTS CALCULATESCORE ──────────────────────────────────

    [Fact]
    public void CalculateScore_Returns5_WhenAllAnswersCorrect()
    {
        // Arrange
        string unitTest  = "Un test qui vérifie une méthode isolée";
        string aaa       = "Arrange les données, Act exécute, Assert vérifie";
        string mock      = "Simuler une dépendance externe";
        string di        = "Injecter une fausse implémentation pour tester";
        string framework = "xUnit";

        // Act
        int score = StoryHelpers.CalculateScore(unitTest, aaa, mock, di, framework);

        // Assert
        Assert.Equal(5, score);
    }

    [Fact]
    public void CalculateScore_Returns0_WhenAllAnswersEmpty()
    {
        // Arrange
        string unitTest  = "[réponse vide]";
        string aaa       = "[réponse vide]";
        string mock      = "[réponse vide]";
        string di        = "[réponse vide]";
        string framework = "[réponse vide]";

        // Act
        int score = StoryHelpers.CalculateScore(unitTest, aaa, mock, di, framework);

        // Assert
        Assert.Equal(0, score);
    }

    [Fact]
    public void CalculateScore_Returns3_WhenThreeCorrect()
    {
        // Arrange
        string unitTest  = "vérifier une méthode";
        string aaa       = "arrange act assert";
        string mock      = "simuler une dépendance";
        string di        = "je ne sais pas";
        string framework = "je ne sais pas";

        // Act
        int score = StoryHelpers.CalculateScore(unitTest, aaa, mock, di, framework);

        // Assert
        Assert.Equal(3, score);
    }

    [Fact]
    public void CalculateScore_DetectsFramework_NUnit()
    {
        // Arrange
        string framework = "J'utilise NUnit pour mes tests";

        // Act
        int score = StoryHelpers.CalculateScore(
            "vérifier", "arrange act assert", "simuler", "injecter", framework);

        // Assert
        Assert.Equal(5, score);
    }

    [Fact]
    public void CalculateScore_DetectsFramework_MSTest()
    {
        // Arrange
        string framework = "MSTest est mon framework préféré";

        // Act
        int score = StoryHelpers.CalculateScore(
            "vérifier", "arrange act assert", "simuler", "injecter", framework);

        // Assert
        Assert.Equal(5, score);
    }
}