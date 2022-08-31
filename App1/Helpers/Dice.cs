using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace App
{
    public class Dice
    {
        public enum Dices { D2, D4, D6, D8, D10, D12, D20, D100 }
        public string Roll { get; set; }
        public int Result { get; set; }

        public const string DICE_PATTERN = @"\d*[[dkдк]\d{1,}(\s*[+*-]?\s*\d{1,})?";
        public Dice(string roll)
        {
            Roll = roll;
            Result = Calculate(roll);
        }
        public static bool ContainDice(string str)
        {
            return Regex.IsMatch(str, DICE_PATTERN);
        }
        private int Calculate(string roll)
        {
            roll = roll.Replace(" ", "");
            if (Regex.IsMatch(roll.Trim().ToLower(), @"[\D-[dkдк+*-]]"))
                throw new ArgumentException();
            if (Regex.IsMatch(roll.Trim().ToLower(), @"^\d*[dkдк]\d*$"))
            {
                if (!char.IsNumber(roll[0]))
                {
                    roll = "1" + roll;
                }
                string[] str = Regex.Split(roll.ToLower(), @"[dkдк]");
                int count = int.Parse(str[0]);
                int dice = int.Parse(str[1]);
                int result = 0;
                for (int i = 0; i < count; i++)
                    result += new Random().Next(1, dice + 1);
                return result;
            }

            if (int.TryParse(roll, out int res))
            {
                return res;
            }
            char nextoperat;
            List<string> l = roll.Split('+').ToList();
            nextoperat = '+';
            if (l.Count < 2)
            {
                l = roll.Split('-').ToList();
                nextoperat = '-';
            }
            if (l.Count < 2)
            {
                l = roll.Split('*').ToList();
                nextoperat = '*';
            }
            string first = l[0];
            string second = "";
            for (int i = 1; i < l.Count; i++)
            {
                second += l[i];
                if (i < l.Count - 1)
                    second += nextoperat;
            }

            return first == roll || second == roll
                ? throw new ArgumentException()
                : nextoperat switch
                {
                    '+' => Calculate(first) + Calculate(second),
                    '-' => Calculate(first) - Calculate(second),
                    '*' => Calculate(first) * Calculate(second)

                };
        }

        public static int RollDice(Dices dice, int count)
        {
            int val = 0;
            for (int i = 0; i < count; i++)
            {
                val += dice switch
                {
                    Dices.D2 => new Random().Next(1, 3),
                    Dices.D4 => new Random().Next(1, 5),
                    Dices.D6 => new Random().Next(1, 7),
                    Dices.D8 => new Random().Next(1, 9),
                    Dices.D10 => new Random().Next(1, 11),
                    Dices.D12 => new Random().Next(1, 13),
                    Dices.D20 => new Random().Next(1, 21),
                    Dices.D100 => new Random().Next(1, 101),
                    _ => throw new ArgumentException()
                };

            }
            return val;
        }
    }
}