namespace PSProductService.Services;

public interface IQuestionGenerator
{
    string GenerateProductForEvent(string eventType, string productList);
}

public class QuestionGenerator : IQuestionGenerator
{
    public string GenerateProductForEvent(string eventType, string productList)
    {
        //return $"Give me 2 products from the following list that would be the most applicable for a " +
        //       $"{eventType} in a json array for each selected product. Include the name and a description indicating why the " +
        //       $"product is good for {eventType}: {productList}";

        return $"Give me 2 products from the following list that would be the most applicable for a " +
               $"{eventType}: {productList}.  Return the results in the following json " +
               $"format: {{ \"products\": [ {{ \"name\": {{name}}, \"description\": {{description}} ]}} " +
               $"where description is the reason the product would be good for a {eventType}.";
    }
}