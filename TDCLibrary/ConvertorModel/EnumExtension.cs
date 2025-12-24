using System.ComponentModel;

namespace TDCLibrary.ConvertorModel;

public class EnumExtension
{
    public static string GetDescription<T>(T value) where T : Enum
    {
        var fieldInfo = value.GetType().GetField(value.ToString());

        if (fieldInfo == null)
        {
            return value.ToString();
        }

        var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attributes.Length > 0 ? attributes[0].Description : value.ToString();
    }
}