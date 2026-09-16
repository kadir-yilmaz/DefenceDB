using System.Text;

namespace DefenceDB.EL.Helpers;

public static class Base62Converter
{
    private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    public static string Encode(int value)
    {
        if (value <= 0) return "0";

        var sb = new StringBuilder();
        int current = value;

        while (current > 0)
        {
            sb.Insert(0, Alphabet[current % 62]);
            current /= 62;
        }

        return sb.ToString();
    }

    public static bool TryDecode(string? input, out int value)
    {
        value = 0;
        if (string.IsNullOrWhiteSpace(input))
            return false;

        long result = 0;
        foreach (char c in input.Trim())
        {
            int index = Alphabet.IndexOf(c);
            if (index == -1)
                return false;

            result = result * 62 + index;
            if (result > int.MaxValue)
                return false;
        }

        value = (int)result;
        return value > 0;
    }
}
