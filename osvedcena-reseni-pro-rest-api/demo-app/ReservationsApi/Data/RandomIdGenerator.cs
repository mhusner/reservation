using System.Text;

namespace ReservationsApi.Data;

public static class RandomIdGenerator
{
    public static string Create(int length = 6)
    {
        Random random = new Random(Guid.NewGuid().GetHashCode());
        string allowedCharacters = "qwertzuioplkjhgfdsayxcvbnm0987654321";
        StringBuilder id = new StringBuilder();
        for (int i = 1; i <= length; i++)
        {
            id.Append(allowedCharacters[random.Next(0, allowedCharacters.Length - 1)]);
        }

        return id.ToString();
    }
}