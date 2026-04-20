using Microsoft.Xna.Framework;
using System.Reflection;

namespace RichTextExtended.Banks;

public class ColorBank : Bank<Color>
{
    public ColorBank()
    {
        var props = typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static);

        foreach (var prop in props)
        {
            if (prop.PropertyType == typeof(Color))
            {
                Add(prop.Name.ToLowerInvariant(), (Color)prop.GetValue(null));
            }
        }
    }
}
