namespace PSProductService.Models;

public class RefinedChatRequest
{
    public string RefiningQuestion { get; set; }
    public List<ChatRequest> ChatLog { get; set; }
}

public class ChatRequest
{
    public bool IsQuestion { get; set; }
    public string Text { get; set; }
}