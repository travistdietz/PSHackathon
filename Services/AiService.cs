using OpenAI.Chat;
using OpenAI.Models;

namespace PSProductService.Services;

public interface IAiService
{
    Task<string> AskQuestion(string question);
}

public class AiService : IAiService
{
    readonly IConfiguration _configuration;

    public AiService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> AskQuestion(string question)
    {
        var apiKey = _configuration.GetValue<string>("OpenAI:ApiKey");
        var client = new OpenAI.OpenAIClient(apiKey);

        var messages = new List<Message>
        {
            new Message(Role.User, question)
        };
        var chatRequest = new ChatRequest(messages, Model.GPT3_5_Turbo);
        var result = await client.ChatEndpoint.GetCompletionAsync(chatRequest);
        return result.FirstChoice.Message;
    }
}