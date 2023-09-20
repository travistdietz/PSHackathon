namespace PSProductService.Services;

public interface IQuestionGenerator
{
    string GenerateProductForEvent(string eventType, string productList);
}

public class QuestionGenerator : IQuestionGenerator
{
    public string GenerateProductForEvent(string eventType, string productList)
    {
        //var randomProducts = GetStringArrayOfRandomData(100);
        return $"Give me 2 products from the following list: {productList}. " +
               $"Products should be the most applicable for the following type of event: {eventType}.  " +
               $"Return the results in the following json format: " +
               $"{{ \"products\": [ {{ \"name\": {{name}}, \"description\": {{description}} ]}} " +
               $"where description is the reason the product is good for this event.";
    }

    string GetStringArrayOfRandomData(int numberToGenerate)
    {
        // Initialize a random number generator
        Random random = new Random();

        // Define the length of each random string
        int stringLength = 10; // Change this to your desired length

        // Create an array to store the random strings
        string[] randomStrings = new string[numberToGenerate]; // Change 100 to the desired number of strings

        // Characters that can be used in the random strings
        string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        for (int i = 0; i < randomStrings.Length; i++)
        {
            // Generate a random string
            char[] stringChars = new char[stringLength];
            for (int j = 0; j < stringLength; j++)
            {
                stringChars[j] = characters[random.Next(characters.Length)];
            }
            randomStrings[i] = new string(stringChars);
        }

        return string.Join(", ", randomStrings);
    }
}