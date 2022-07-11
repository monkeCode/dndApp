using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace App1.WorkShop
{
    internal class Formator
    {
        public static void StringtoText(TextBlock textBlock, string s = null)
        {
            if (!string.IsNullOrEmpty(s))
                textBlock.Text = s;
            Formating(BoldRegex, run => run.FontWeight = FontWeights.Bold , textBlock);
            Formating(ItalicRegex, run => run.FontStyle = FontStyle.Italic, textBlock);
            //Formating('|', textBlock);
            AddHyperlink(textBlock);
            CreateDiceRoll(textBlock);
        }

        private static Regex ItalicRegex = new Regex(@"\*", RegexOptions.Compiled | RegexOptions.Multiline);
        private static Regex BoldRegex = new Regex(@"\*\*", RegexOptions.Compiled | RegexOptions.Multiline);
        private static void Formating(Regex rg, Action<Run> action, TextBlock textBlock)
        {
            List<Run> runs = new List<Run>();
            foreach (Run ru in textBlock.Inlines)
            {
                string[] str = rg.Split(ru.Text);
                for (int i = 0; i < str.Length; i++)
                {
                    Run run = new Run
                    {
                        FontWeight = ru.FontWeight,
                        TextDecorations = ru.TextDecorations,
                        FontStyle = ru.FontStyle,
                        Text = str[i]
                    };

                    if (i % 2 != 0)
                    {
                        action(run);
                    }

                    runs.Add(run);
                }
            }

            textBlock.Text = string.Empty;
            foreach (var r in runs)
                textBlock.Inlines.Add(r);
        }

        private static Regex hypeRegex = new Regex(@"[\[\]]", RegexOptions.Compiled);
        private static void AddHyperlink(TextBlock textBlock)
        {
            List<Inline> runs = new List<Inline>();
            foreach (Run ru in textBlock.Inlines)
            {
                //string[] str = ru.Text.Split("^");
                string[] str = hypeRegex.Split(ru.Text);
                for (int i = 0; i < str.Length; i++)
                {
                    Run run = new Run
                    {
                        FontWeight = ru.FontWeight,
                        TextDecorations = ru.TextDecorations,
                        FontStyle = ru.FontStyle,
                        Text = str[i]
                    };
                    if (i % 2 != 0)
                    {
                        Hyperlink hyperlink = new Hyperlink();
                        hyperlink.Inlines.Add(run);
                        runs.Add(hyperlink);
                    }
                    else
                        runs.Add(run);
                }
            }

            textBlock.Text = string.Empty;
            foreach (var r in runs)
                textBlock.Inlines.Add(r);
        }

        private static void CreateDiceRoll(TextBlock textBlock)
        {
            Regex regex = new Regex(Dice.DICE_PATTERN);
            List<Inline> runs = new List<Inline>();
            foreach (Inline ru in textBlock.Inlines)
            {
                string s;
                if (ru.GetType() == typeof(Run))
                    s = (ru as Run).Text;
                else
                {
                    s = ((ru as Hyperlink).Inlines[0] as Run).Text;
                }
                MatchCollection matches = regex.Matches(s);
                s = regex.Replace(s, "regex");
                string[] str = s.Split("regex");
                IEnumerator strEnum = str.GetEnumerator();
                IEnumerator matchEnum = matches.GetEnumerator();
                for (int i = 0; ; i++)
                {
                    Inline run;
                    if (ru.GetType() == typeof(Run))
                    {
                        run = new Run
                        {
                            FontWeight = ru.FontWeight,
                            TextDecorations = ru.TextDecorations,
                            FontStyle = ru.FontStyle,
                        };
                    }
                    else
                    {
                        run = new Hyperlink();
                        (run as Hyperlink).Inlines.Add(new Run()
                        {
                            FontWeight = ru.FontWeight,
                            TextDecorations = ru.TextDecorations,
                            FontStyle = ru.FontStyle,
                        });
                    }

                    if (i % 2 != 0)
                    {
                        if (!matchEnum.MoveNext())
                            continue;
                        Hyperlink hyperlink = new Hyperlink();
                        hyperlink.Inlines.Add(new Run()
                        {
                            FontWeight = run.FontWeight,
                            TextDecorations = run.TextDecorations,
                            FontStyle = run.FontStyle,
                            Text = ((Match)matchEnum.Current).Value
                        });
                        runs.Add(hyperlink);
                    }
                    else
                    {
                        if (!strEnum.MoveNext())
                            break;
                        if (run is Run r)
                            r.Text = strEnum.Current.ToString();
                        else
                        {
                            ((run as Hyperlink).Inlines[0] as Run).Text = strEnum.Current.ToString();
                        }
                        runs.Add(run);
                    }
                }
            }

            textBlock.Text = string.Empty;
            foreach (var r in runs)
                textBlock.Inlines.Add(r);

        }

        public static string CreateDbValidStr(string s)
        {
            if (s == null)
                return "";
            s = s.Replace("'", "\\'");
            return s;
        }
    }
}