using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/ping", () => "API OK");

app.MapPost("/api/generate", (StoryRequest request) =>
{
    string name = StoryHelpers.Clean(request.Name, "[non précisé]");
    string unitTest = StoryHelpers.Clean(request.UnitTestDefinition, "[réponse vide]");
    string aaa = StoryHelpers.Clean(request.ArrangeActAssert, "[réponse vide]");
    string mock = StoryHelpers.Clean(request.MockDefinition, "[réponse vide]");
    string di = StoryHelpers.Clean(request.DependencyInjection, "[réponse vide]");
    string framework = StoryHelpers.Clean(request.FrameworkName, "[réponse vide]");

    int score = 0;
    var feedback = new List<string>();

    if (StoryHelpers.ContainsAny(unitTest, "tester", "vérifier", "verifier", "fonction", "méthode", "methode", "unité", "unite", "comportement", "isoler", "isolé", "isole"))
    {
        score++;
        feedback.Add("Définition du test unitaire : idée globalement correcte.");
    }
    else
    {
        feedback.Add("Test unitaire : pense à parler d’une petite partie du code testée de façon isolée.");
    }

    if (StoryHelpers.ContainsAny(aaa, "arrange") && StoryHelpers.ContainsAny(aaa, "act") && StoryHelpers.ContainsAny(aaa, "assert"))
    {
        score++;
        feedback.Add("AAA : les trois étapes essentielles sont présentes.");
    }
    else
    {
        feedback.Add("AAA : il manque probablement une ou plusieurs étapes (Arrange, Act, Assert).");
    }

    if (StoryHelpers.ContainsAny(mock, "simuler", "remplacer", "fausse", "faux", "dépendance", "dependance", "isoler", "stub"))
    {
        score++;
        feedback.Add("Mock : bonne idée, tu évoques bien la simulation d’une dépendance.");
    }
    else
    {
        feedback.Add("Mock : pense à expliquer qu’il sert à remplacer une dépendance réelle pendant le test.");
    }

    if (StoryHelpers.ContainsAny(di, "inject", "remplacer", "dépendance", "dependance", "tester", "isoler", "couplage", "découpl", "implementation", "implémentation"))
    {
        score++;
        feedback.Add("Injection de dépendances : ta réponse va dans le bon sens.");
    }
    else
    {
        feedback.Add("Injection de dépendances : parle du fait qu’on peut injecter une autre implémentation pour tester.");
    }

    if (StoryHelpers.ContainsAny(framework, "xunit", "nunit", "mstest"))
    {
        score++;
        feedback.Add("Framework : exemple valide reconnu en C#.");
    }
    else
    {
        feedback.Add("Framework : cite par exemple xUnit, NUnit ou MSTest.");
    }

    string level =
        score <= 1 ? "Niveau débutant : les bases sont encore fragiles."
      : score <= 3 ? "Niveau intermédiaire : tu as plusieurs notions, mais elles doivent être consolidées."
      : "Bon niveau : tes réponses montrent une compréhension correcte des tests unitaires.";

    string time = DateTime.Now.ToString("HH:mm");
    string date = DateTime.Now.ToString("dd/MM/yyyy");

    string story = $@"
Récapitulatif de tes réponses sur C# et les tests unitaires ({date} à {time}) :

1) Définition du test unitaire
   Ta réponse : {unitTest}
   
2) Méthode Arrange / Act / Assert
   Ta réponse : {aaa}

3) Rôle d’un mock
   Ta réponse : {mock} 

4) Intérêt de l’injection de dépendances pour les tests
   Ta réponse : {di}

5) Framework de test C# que tu connais
   Ta réponse : {framework}

Rappel général :
- Un test unitaire vérifie un comportement précis, sur une petite partie du code.
- Les tests doivent être isolés, lisibles et déterministes.
- Arrange prépare les données et le contexte.
- Act exécute la méthode à tester.
- Assert vérifie le résultat attendu.
- Les mocks/simulations permettent de remplacer les dépendances réelles.
- L’injection de dépendances facilite le remplacement d’implémentation pour tester.

Synthèse de ton niveau actuel :
{level}
";

    return Results.Ok(new StoryResponse(
        Story: story,
        GeneratedAt: $"{date} {time}",
        Score: score,
        MaxScore: 5,
        Level: level,
        Feedback: feedback
    ));
});

app.Run();

public static class StoryHelpers
{
    public static string Clean(string? value, string fallback)
        => string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();

    public static bool ContainsAny(string source, params string[] keywords)
        => keywords.Any(k => source.ToLowerInvariant().Contains(k.ToLowerInvariant()));

    public static int CalculateScore(
        string unitTest,
        string aaa,
        string mock,
        string di,
        string framework)
    {
        int score = 0;

        if (StoryHelpers.ContainsAny(unitTest, "tester", "vérifier", "verifier", "méthode",
            "methode", "unité", "unite", "comportement", "automatisé",
            "automatise", "isolée", "isolee", "petite partie"))
            score++;

        if (StoryHelpers.ContainsAny(aaa, "arrange") && StoryHelpers.ContainsAny(aaa, "act") && StoryHelpers.ContainsAny(aaa, "assert"))
            score++;

        if (StoryHelpers.ContainsAny(mock, "simuler", "remplacer", "fausse", "faux",
            "dépendance", "dependance", "isoler", "simulation"))
            score++;

        if (StoryHelpers.ContainsAny(di, "inject", "remplacer", "dépendance", "dependance",
            "tester", "isoler", "couplage", "découpl", "implémentation", "implementation"))
            score++;

        if (StoryHelpers.ContainsAny(framework, "xunit", "nunit", "mstest"))
            score++;

        return score;
    }
}

public record StoryRequest(
    string Name,
    string UnitTestDefinition,
    string ArrangeActAssert,
    string MockDefinition,
    string DependencyInjection,
    string FrameworkName
);

public record StoryResponse(
    string Story,
    string GeneratedAt,
    int Score,
    int MaxScore,
    string Level,
    List<string> Feedback
);// À ajouter à la fin du fichier Program.cs
public partial class Program { } 