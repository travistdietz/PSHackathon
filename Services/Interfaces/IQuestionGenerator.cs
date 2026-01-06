namespace PSProductService.Services.Interfaces;

public interface IQuestionGenerator
{
    string GenerateProductForEvent(string eventType, string productList);
    string GenerateRefinedQuestion(string refiningQuestion);
}
