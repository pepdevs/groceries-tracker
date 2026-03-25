namespace FoodTracker.Domain.Enums;

public enum MatchMethod
{
    ExactStoreAlias,
    ExactGlobalAlias,
    FuzzyAlias,
    StructuredHeuristic,
    AiSuggested,
    Manual
}
