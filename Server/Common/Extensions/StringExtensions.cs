using System.Text;

namespace Server.Common.Extensions;

static class StringExtensions
{
    public static string RemoveAccents(this string str)
    {
        Dictionary<char, char> replacements = new()
        {
            { 'à', 'a' },
            { 'ç', 'c' },
            { 'é', 'e' },
            { 'è', 'e' },
            { 'ê', 'e' },
            { 'ë', 'e' },
            { 'ô', 'o' },
            { 'û', 'u' },
            { 'ù', 'u' }
        };

        Dictionary<char, string> strReplacements = new()
        {
            { 'œ', "oe" },
            { 'Œ', "Oe" }
        };

        StringBuilder stringBuilder = new();

        foreach (char c in str)
        {
            if (replacements.TryGetValue(c, out char r))
            {
                stringBuilder.Append(r);
            }
            else if (strReplacements.TryGetValue(c, out string? s))
            {
                stringBuilder.Append(s);
            }
            else
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString();
    }

    public static int GetStableHashCode(this string str)
    {
        unchecked
        {
            int hash1 = 5381;
            int hash2 = hash1;

            for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
            {
                hash1 = (hash1<<5) + hash1 ^ str[i];
                if (i == str.Length - 1 || str[i + 1] == '\0')
                {
                    break;
                }
                hash2 = (hash2<<5) + hash2 ^ str[i + 1];
            }

            return hash1 + hash2 * 1566083941;
        }
    }
}
