using Xunit;

public class StoryHelpersTests
{


    [Fact]
    public void Clean_ReturnsValue_WhenNotEmpty()
    {
      
        string input    = "  Mano  ";
        string fallback = "[non précisé]";

    
        string result = StoryHelpers.Clean(input, fallback);

    
        Assert.Equal("Mano", result);
    }

    [Fact]
    public void Clean_ReturnsFallback_WhenEmpty()
    {
      
        string input    = "";
        string fallback = "[non précisé]";

  
        string result = StoryHelpers.Clean(input, fallback);

       
        Assert.Equal("[non précisé]", result);
    }

    [Fact]
    public void Clean_ReturnsFallback_WhenNull()
    {
       
        string? input   = null;
        string fallback = "[non précisé]";

        
        string result = StoryHelpers.Clean(input, fallback);

        Assert.Equal("[non précisé]", result);
    }

    [Fact]
    public void Clean_ReturnsFallback_WhenWhitespace()
    {
    
        string input    = "   ";
        string fallback = "[non précisé]";


        string result = StoryHelpers.Clean(input, fallback);

      
        Assert.Equal("[non précisé]", result);
    }


    [Fact]
    public void ContainsAny_ReturnsTrue_WhenKeywordPresent()
    {
     
        string source = "Je veux tester ma méthode";

     
        bool result = StoryHelpers.ContainsAny(source, "tester");

      
        Assert.True(result);
    }

    [Fact]
    public void ContainsAny_ReturnsFalse_WhenNoKeywordPresent()
    {
   
        string source = "Je ne sais pas";

       
        bool result = StoryHelpers.ContainsAny(source, "xunit", "nunit", "mstest");

        Assert.False(result);
    }

    [Fact]
    public void ContainsAny_IsCaseInsensitive()
    {
        string source = "J'utilise XUNIT pour mes tests";

        bool result = StoryHelpers.ContainsAny(source, "xunit");

   
        Assert.True(result);
    }

    [Fact]
    public void ContainsAny_ReturnsTrue_WhenOneOfManyKeywordsPresent()
    {
    
        string source = "On arrange les données avant le test";

        bool result = StoryHelpers.ContainsAny(source, "arrange", "act", "assert");

     
        Assert.True(result);
    }


    [Fact]
    public void CalculateScore_Returns5_WhenAllAnswersCorrect()
    {
    
        string unitTest  = "Un test qui vérifie une méthode isolée";
        string aaa       = "Arrange les données, Act exécute, Assert vérifie";
        string mock      = "Simuler une dépendance externe";
        string di        = "Injecter une fausse implémentation pour tester";
        string framework = "xUnit";


        int score = StoryHelpers.CalculateScore(unitTest, aaa, mock, di, framework);


        Assert.Equal(5, score);
    }

    [Fact]
    public void CalculateScore_Returns0_WhenAllAnswersEmpty()
    {
  
        string unitTest  = "[réponse vide]";
        string aaa       = "[réponse vide]";
        string mock      = "[réponse vide]";
        string di        = "[réponse vide]";
        string framework = "[réponse vide]";


        int score = StoryHelpers.CalculateScore(unitTest, aaa, mock, di, framework);


        Assert.Equal(0, score);
    }

    [Fact]
    public void CalculateScore_Returns3_WhenThreeCorrect()
    {

        string unitTest  = "vérifier une méthode";
        string aaa       = "arrange act assert";
        string mock      = "simuler une dépendance";
        string di        = "je ne sais pas";
        string framework = "je ne sais pas";


        int score = StoryHelpers.CalculateScore(unitTest, aaa, mock, di, framework);

        Assert.Equal(3, score);
    }

    [Fact]
    public void CalculateScore_DetectsFramework_NUnit()
    {

        string framework = "J'utilise NUnit pour mes tests";


        int score = StoryHelpers.CalculateScore(
            "vérifier", "arrange act assert", "simuler", "injecter", framework);

        Assert.Equal(5, score);
    }

    [Fact]
    public void CalculateScore_DetectsFramework_MSTest()
    {

        string framework = "MSTest est mon framework préféré";
        int score = StoryHelpers.CalculateScore(
            "vérifier", "arrange act assert", "simuler", "injecter", framework);


        Assert.Equal(5, score);
    }
}