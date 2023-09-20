using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using ChatRequest = PSProductService.Models.ChatRequest;

namespace PSProductService.Services;

public interface IAiService
{
    Task<string> AskQuestion(string question);
    Task<string> AskQuestionWithPreviousContext(string refiningQuestion, IEnumerable<ChatRequest> chats);
}

public class AiService : IAiService
{
    readonly IConfiguration _configuration;
    readonly OpenAIClient Client;

    public AiService(IConfiguration configuration)
    {
        _configuration = configuration;
        var apiKey = _configuration.GetValue<string>("OpenAI:ApiKey");
        Client = new OpenAI.OpenAIClient(apiKey);
    }

    public async Task<string> AskQuestion(string question)
    {
        var messages = new List<Message>
        {
            new Message(Role.User, question)
        };
        var chatRequest = new OpenAI.Chat.ChatRequest(messages, Model.GPT3_5_Turbo);
        var result = await Client.ChatEndpoint.GetCompletionAsync(chatRequest);
        return result.FirstChoice.Message;
    }

    public async Task<string> AskQuestionWithPreviousContext(string refiningQuestion, IEnumerable<ChatRequest> chats)
    {
        var messages = new List<Message>();
        foreach (var chat in chats)
        {
            messages.Add(new Message(chat.IsQuestion ? Role.User : Role.System, chat.Text));
        }

        messages.Add(new Message(Role.User, refiningQuestion));
        var chatRequest = new OpenAI.Chat.ChatRequest(messages, Model.GPT3_5_Turbo);
        var result = await Client.ChatEndpoint.GetCompletionAsync(chatRequest);
        return result.FirstChoice.Message;
    }
}