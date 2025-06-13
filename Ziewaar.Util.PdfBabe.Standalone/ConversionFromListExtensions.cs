// See https://aka.ms/new-console-template for more information
public static class ConversionFromListExtensions
{
    public static double GetDoubleFrom(this SortedList<string, object> keyValuePairs, string key, double fallback)
    {
        if (keyValuePairs.TryGetValue(key, out var val))
        {
            try
            {
                return Convert.ToDouble(val);
            }
            catch (Exception)
            {
                return fallback;
            }
        }
        else
        {
            return fallback;
        }
    }
    public static bool GetBoolFrom(this SortedList<string, object> keyValuePairs, string key, bool fallback)
    {
        if (keyValuePairs.TryGetValue(key, out var val))
        {
            try
            {
                return Convert.ToBoolean(val);
            }
            catch (Exception)
            {
                return fallback;
            }
        }
        else
        {
            return fallback;
        }
    }
    public static char GetCharFrom(this SortedList<string, object> keyValuePairs, string key, char fallback)
    {
        if (keyValuePairs.TryGetValue(key, out var val))
        {
            if (val is string str)
            {
                if (str.Length > 0)
                    return str[0];
                else
                    return fallback;
            }
            try
            {
                return Convert.ToChar(val);
            }
            catch (Exception)
            {
                return fallback;
            }
        }
        else
        {
            return fallback;
        }
    }
}
