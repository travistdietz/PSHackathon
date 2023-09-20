namespace PSProductService.Models;

public class AIResponse
{
    public List<AIProduct> products { get; set; }
}

public class AIProduct
{
    public string name { get; set; }
    public string description { get; set; }
}