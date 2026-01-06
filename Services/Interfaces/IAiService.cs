using ChatRequest = PSProductService.Models.ChatRequest;

namespace PSProductService.Services.Interfaces;

public interface IAiService
{
    Task<string> AskQuestion(string question);
    Task<string> AskQuestionWithPreviousContext(string refiningQuestion, IEnumerable<ChatRequest> chats);
}
