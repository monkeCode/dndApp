using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace App1
{
    public class Dice
    {
        public string Roll { get; set; }
        public int Result { get; set; }

        public const string DICE_PATTERN = @"\d*[[dkдк]\d+\d*(\s*[+-]?\s*\d+\d*)?";
        public Dice(string roll)
        {
            Roll = roll;
            Result = Calculate(roll);
        }

        private int Calculate(string roll)
        {
            if (Regex.IsMatch(roll.Trim().ToLower(), @"[\D-[d\|k\|д\|к\|\+\|\-\|\s]]"))
                throw new ArgumentException();
            if (Regex.IsMatch(roll.Trim().ToLower(), @"^\d*[d\|k\|д\|к]\d*$"))
            {
                string[] str = Regex.Split(roll.ToLower(), @"[d\|k\|д\|к]");
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
            string first = l[0];
            string second = "";
            for (int i = 1; i < l.Count; i++)
            {
                second += l[i];
                if (i < l.Count - 1)
                    second += nextoperat;
            }

            return first == roll || second == roll ? throw new ArgumentException() :
                nextoperat == '+' ? Calculate(first) + Calculate(second) : Calculate(first) - Calculate(second);
        }
    }
}