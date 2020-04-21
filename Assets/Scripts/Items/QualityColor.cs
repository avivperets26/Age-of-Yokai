using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Quality { Common, Uncommon, Rare, Epic }//Enum for declaring the quality of the item
public static class QualityColor
{
    private static Dictionary<Quality, string> colors = new Dictionary<Quality, string>()
    {
        {Quality.Common, "#ffffffff"},
        {Quality.Uncommon, "#00ff00ff"},
        {Quality.Rare, "#0E6BECFF"},
        {Quality.Epic, "#A712DBFF"},
    };

    public static Dictionary<Quality, string> MyColors
    {
        get
        {
            return colors;
        }
    }
}