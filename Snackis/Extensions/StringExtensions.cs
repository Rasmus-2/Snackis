using System.Text.RegularExpressions;

namespace Snackis.Extensions
{
    public static class StringExtensions
    {
        public static string LimitLength(this string str, int maxLength)
        {
            if (str != null)
            {
                if (str.Length <= maxLength)
                {
                    return str;
                }
                return str.Substring(0, maxLength);
            }
            return str;
        }

        public static string Censor(this string stringToCensor)
        {
            string pattern = @"(jäkla|helvete|jävla|skit|fan)(s?)(r?)";
            Regex regex = new Regex(pattern);

            string[] splitString = Regex.Split(stringToCensor, " ");
            string[] newStrings = new string[splitString.Length];

            for (int i = 0; i < splitString.Length; i++)
            {

                if (regex.IsMatch(splitString[i].ToLower()))
                {
                    int numLetter = splitString[i].Length;
                    string stars = "";
                    for (int j = 0; j < numLetter; j++)
                    {
                        stars += "*";
                    }
                    string censoredString = regex.Replace(splitString[i].ToLower(), stars);
                    newStrings[i] = censoredString;
                }
                else
                {
                    newStrings[i] = splitString[i];
                }
            }
            string finalString = string.Join(" ", newStrings);
            return finalString;
        }
    }
}
