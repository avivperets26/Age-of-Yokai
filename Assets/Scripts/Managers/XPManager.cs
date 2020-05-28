using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class XPManager
{
    public static int CalculateXP(Enemy e)
    {
        int baseXP = (Hero.MyInstance.MyLevel * 5) + 45;

        int greyLevel = CalculateGrayLevel();

        int totalXP = 0;

        if (e.MyLevel >= Hero.MyInstance.MyLevel)
        {
            totalXP = (int)(baseXP * (1 + 0.05 * (e.MyLevel - Hero.MyInstance.MyLevel)));
        }
        else if (e.MyLevel > greyLevel)
        {
            totalXP = (baseXP) * (1 - (Hero.MyInstance.MyLevel - e.MyLevel) / ZeroDifference());
        }

        return totalXP;
    }

    public static int CalculateXP(Quest e)
    {
        if (Hero.MyInstance.MyLevel <= e.MyLevel + 5)
        {
            return e.MyXp;
        }
        if (Hero.MyInstance.MyLevel == e.MyLevel + 6)
        {
            return (int)(e.MyXp * 0.8 / 5) * 5;
        }
        if (Hero.MyInstance.MyLevel == e.MyLevel + 7)
        {
            return (int)(e.MyXp * 0.6 / 5) * 5;
        }
        if (Hero.MyInstance.MyLevel == e.MyLevel + 8)
        {
            return (int)(e.MyXp * 0.4 / 5) * 5;
        }
        if (Hero.MyInstance.MyLevel == e.MyLevel + 9)
        {
            return (int)(e.MyXp * 0.2 / 5) * 5;
        }
        if (Hero.MyInstance.MyLevel >= e.MyLevel + 10)
        {
            return (int)(e.MyXp * 0.1 / 5) * 5;
        }
        return 0;
    }

    private static int ZeroDifference()
    {
        if (Hero.MyInstance.MyLevel <= 7)
        {
            return 5;
        }
        if (Hero.MyInstance.MyLevel >= 8 && Hero.MyInstance.MyLevel <= 9)
        {
            return 6;
        }
        if (Hero.MyInstance.MyLevel >= 10 && Hero.MyInstance.MyLevel <= 7)
        {
            return 7;
        }
        if (Hero.MyInstance.MyLevel >= 12 && Hero.MyInstance.MyLevel <= 15)
        {
            return 8;
        }
        if (Hero.MyInstance.MyLevel >= 16 && Hero.MyInstance.MyLevel <= 19)
        {
            return 9;
        }
        if (Hero.MyInstance.MyLevel >= 20 && Hero.MyInstance.MyLevel <= 29)
        {
            return 11;
        }
        if (Hero.MyInstance.MyLevel >= 30 && Hero.MyInstance.MyLevel <= 39)
        {
            return 12;
        }
        if (Hero.MyInstance.MyLevel >= 40 && Hero.MyInstance.MyLevel <= 44)
        {
            return 13;
        }
        if (Hero.MyInstance.MyLevel >= 45 && Hero.MyInstance.MyLevel <= 49)
        {
            return 14;
        }
        if (Hero.MyInstance.MyLevel >= 50 && Hero.MyInstance.MyLevel <= 54)
        {
            return 15;
        }
        if (Hero.MyInstance.MyLevel >= 55 && Hero.MyInstance.MyLevel <= 59)
        {
            return 16;
        }

        return 17;
    }

    public static int CalculateGrayLevel()
    {
        if (Hero.MyInstance.MyLevel <= 5)
        {
            return 0;
        }
        else if (Hero.MyInstance.MyLevel >= 6 && Hero.MyInstance.MyLevel <= 49)
        {
            return Hero.MyInstance.MyLevel - (Hero.MyInstance.MyLevel / 10) - 5;
        }
        else if (Hero.MyInstance.MyLevel == 50)
        {
            return Hero.MyInstance.MyLevel - 10;
        }
        else if (Hero.MyInstance.MyLevel >= 51 && Hero.MyInstance.MyLevel <= 59)
        {
            return Hero.MyInstance.MyLevel - (Hero.MyInstance.MyLevel / 5) - 1;
        }

        return Hero.MyInstance.MyLevel - 9;
    }
}

