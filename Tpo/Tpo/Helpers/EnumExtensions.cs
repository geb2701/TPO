using System.ComponentModel;

public static class EnumExtensions
{
    public static string GetEnumDescription(this Enum enumValue)
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
        var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

        return descriptionAttributes.Length > 0 ? (descriptionAttributes[0]).Description : enumValue.ToString();
    }

    public static EnumResponse ToEnumResponse(this Enum enumValue)
    {
        if (enumValue == null)
            return new EnumResponse("Error", 99);
        return new EnumResponse(enumValue.GetEnumDescription(), Convert.ToInt32(enumValue));
    }

    public static int GetValue<TEnum>(this TEnum enumValue) where TEnum : Enum
    {
        return Convert.ToInt32(enumValue);
    }

    public static TEnum ToEnum<TEnum>(this int value) where TEnum : Enum
    {
        if (Enum.IsDefined(typeof(TEnum), value))
        {
            return (TEnum)(object)value;
        }
        else
        {
            throw new ArgumentException($"Invalid value {value} for {typeof(TEnum).Name}", nameof(value));
        }
    }
}

public class EnumResponse
{
    public string Name { get; set; }
    public int Value { get; set; }

    public EnumResponse(string name, int value)
    {
        this.Name = name;
        this.Value = value;
    }
}
