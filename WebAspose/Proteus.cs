using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace WebAspose;

/// <summary>
///     Provides utility methods for object comparison, serialization, and state validation.
/// </summary>
public static class Proteus
{
    public static bool CompareObjects<T1, T2>(T1 obj1, T2 obj2)
    {
        if (obj1 == null || obj2 == null)
            throw new ArgumentNullException("Gli oggetti non possono essere null.");

        // Ottieni tutte le proprietà decorate con [ToCompare] per il primo oggetto
        var properties1 = typeof(T1).GetProperties()    
            .Where(p => p.GetCustomAttribute<ToCompareAttribute>() != null);

        // Ottieni tutte le proprietà del secondo oggetto
        var properties2 = typeof(T2).GetProperties();

        // Confronta le proprietà comuni (stesso nome e tipo)
        foreach (var prop1 in properties1)
        {
            var prop2 = properties2.FirstOrDefault(p => p.Name == prop1.Name && p.PropertyType == prop1.PropertyType);
            if (prop2 == null)
                continue; // Se non c'è una proprietà corrispondente, ignorala

            var value1 = prop1.GetValue(obj1);
            var value2 = prop2.GetValue(obj2);

            // Confronta i valori (gestisce anche i null)
            if (!Equals(value1, value2))
                return false;
        }

        return true; // Tutte le proprietà corrispondenti sono uguali
    }
    
    /// <summary>
    ///     Serializes an object into a hash string for change detection.
    /// </summary>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>A hash string representing the object's state, or empty string if null.</returns>
    public static string Serialize(object obj)
    {
        // Initial value
        var serialized = string.Empty;

        // If the obj object exists
        if (Exists(obj))
        {
            var json = JsonSerializer.Serialize(obj);
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(json);
                var hashBytes = md5.ComputeHash(inputBytes);
                serialized = Convert.ToHexString(hashBytes); // 32-char hex string
            }
        }

        // Return value
        return serialized;
    }


    /// <summary>
    ///     Compares an object's current state to a previously serialized hash.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <param name="hash">The hash string to compare against.</param>
    /// <returns>True if the object matches the hash (unchanged), false otherwise.</returns>
    public static bool Compare(object obj, string hash)
    {
        // Initial value
        var isMatch = false;

        // if the object exists and the hash exists
        if (Exists(obj) && Exists(hash))
        {
            // Get the new hash
            var newHash = Serialize(obj);

            // Is this a match
            isMatch = IsEqual(newHash, hash);
        }

        // Return value
        return isMatch;
    }


    /// <summary>
    ///     This method returns true if the two strings given are equal.
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="sourceString2">The string to be compared to.</param>
    /// <param name="textMustExist">
    ///     This defaults to true; if true both strings must exist or false is returned even if the
    ///     strings match.
    /// </param>
    /// <param name="caseMustMatch">
    ///     This defaults to false; if true the comparison will be a case sensitive comparison.
    ///     <returns></returns>
    public static bool IsEqual(string sourceString, string sourceString2, bool textMustExist = true,
        bool caseMustMatch = false)
    {
        // initial value
        var isEqual = false;

        // local s
        var abort = false;
        var compare = 0;

        // if the text must exist
        if (textMustExist)
            // if both strings do not exist than abort will be set to true
            abort = !Exists(sourceString, sourceString2);

        //if we should continue
        if (!abort)
        {
            // if we need to a Case Sensitive Comparison
            if (caseMustMatch)
                // the case must match
                compare = string.Compare(sourceString, sourceString2, false);
            else
                // the case must match
                compare = string.Compare(sourceString, sourceString2, true);

            // set the return value
            isEqual = compare == 0;
        }

        // return value
        return isEqual;
    }

    /// <summary>
    ///     This method returns true if string given exists
    /// </summary>
    public static bool Exists(string string1)
    {
        // initial value
        var exists = false;

        // test if the string exists
        exists = !string.IsNullOrEmpty(string1);

        // return value
        return exists;
    }

    /// <summary>
    ///     This method returns true if BOTH strings given exist
    /// </summary>
    public static bool Exists(string string1, string string2)
    {
        // initial value
        var exists = false;

        // this method returns true if both strings exist
        var string1Exists = Exists(string1);
        var string2Exists = Exists(string2);

        // set the return value
        exists = string1Exists && string2Exists;

        // return value
        return exists;
    }

    public static bool Exists(object item)
    {
        // initial value
        var exists = item != null;

        // return value
        return exists;
    }
}