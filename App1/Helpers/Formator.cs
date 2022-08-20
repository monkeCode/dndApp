using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace App1.WorkShop
{
    internal class Formator
    {
        private static Regex headerRegex = new Regex(@"^\s*(?<!\\|\#)(?<Header>\#{1,6})(?<text>[^#]*)\k<Header>?", RegexOptions.Compiled);
        private static Regex ItalicRegex = new Regex(@"(?<!\\)[\*]", RegexOptions.Compiled | RegexOptions.Multiline);
        private static Regex BoldRegex = new Regex(@"(?<!\\)[\*]{2}", RegexOptions.Compiled | RegexOptions.Multiline);
        private static Regex hypeRegex = new Regex(@"(?<!\\)(\[)(?<name>.*?)(\])", RegexOptions.Compiled);
        private static Regex listRegex = new Regex(@"^(?<space>\s{0,4})(?<!\\)[\*\-\+](\s|\t)", RegexOptions.Compiled | RegexOptions.Multiline);
        private static Regex ignoreSymvolRegex = new Regex(@"\\(?<Sym>.)");
        public static void StringtoText(TextBlock textBlock, string s = null)
        {
            if (!string.IsNullOrEmpty(s))
                textBlock.Text = s;
            SplitByLines(textBlock);
            CreateList(textBlock);
            Formating(BoldRegex, run => run.FontWeight = FontWeights.Bold , textBlock);
            Formating(ItalicRegex, run => run.FontStyle = FontStyle.Italic, textBlock);
            AddHyperlink(textBlock);
            AddHeaders(headerRegex, textBlock);
            TextIgnoring(ignoreSymvolRegex, textBlock);
            CreateDiceRoll(textBlock);
        }
        private static void Formating(Regex rg, Action<Run> action, TextBlock textBlock)
        {
            List<Inline> runs = new List<Inline>();
            foreach (Inline inline in textBlock.Inlines)
            {
                Run ru = inline as Run;
                if (ru == null)
                {
                    runs.Add(inline);
                    continue;
                }
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

        private static void AddHeaders(Regex rg, TextBlock textBlock)
        {
            ResourceDictionary myResourceDictionary = new ResourceDictionary();
            myResourceDictionary.Source = new Uri("ms-appx:///Dictionary.xaml");
            foreach (Inline inline in textBlock.Inlines)
            {
                Run ru = inline as Run;
                if (ru == null || !rg.IsMatch(ru.Text))
                {
                    continue;
                }
                
                var headerType = rg.Match(ru.Text).Groups["Header"].Value.Length;
                ru.Text = rg.Match(ru.Text).Groups["text"].Value;
                switch (headerType)
                    {
                        case 1:
                            ru.FontWeight = FontWeights.Bold;
                            ru.FontSize = 24;
                            ru.Foreground = myResourceDictionary["Header"] as SolidColorBrush;
                            break;
                        case 2:
                            ru.FontWeight = FontWeights.Bold;
                            ru.FontSize = 24;
                            break;
                        case 3:
                            ru.FontWeight = FontWeights.Bold;
                            ru.FontStyle = FontStyle.Italic;
                            ru.FontStretch = FontStretch.ExtraExpanded;
                            break;
                        case 4:
                            ru.FontWeight = FontWeights.Bold;
                            ru.FontStyle = FontStyle.Italic;
                            ru.FontStretch = FontStretch.ExtraExpanded;
                            ru.Foreground = myResourceDictionary["Header"] as SolidColorBrush;
                            break;
                        case 5:
                            ru.FontWeight = FontWeights.Bold;
                            ru.FontStyle = FontStyle.Italic;
                            ru.FontStretch = FontStretch.ExtraExpanded;
                            ru.Foreground = myResourceDictionary["Header"] as SolidColorBrush;
                            break;
                    }
            }

        }
        
        private static void TextIgnoring(Regex rg, TextBlock text)
        {
            foreach (Inline inline in text.Inlines)
            {
                Run ru = inline as Run;
                if (ru != null)
                {
                    ru.Text = rg.Replace(ru.Text, "${Sym}");
                }
            }
        }

        private static void AddHyperlink(TextBlock textBlock)
        {
            List<Inline> runs = new List<Inline>();
            foreach (Inline inline in textBlock.Inlines)
            {
                Run ru = inline as Run;
                if (ru == null)
                {
                    runs.Add(inline);
                    continue;
                }

                var matches = hypeRegex.Matches(ru.Text);
                var arr = hypeRegex.Replace(ru.Text, "REGEXREPLACE").Split("REGEXREPLACE");
                for (int i = 0; i < Math.Max(matches.Count, arr.Length); i++)
                {
                    if (i < arr.Length)
                        runs.Add(new Run
                        {
                            FontWeight = ru.FontWeight,
                            TextDecorations = ru.TextDecorations,
                            FontStyle = ru.FontStyle,
                            Text = arr[i]
                        });
                    if (i < matches.Count)
                    {
                        runs.Add(new Hyperlink
                        {
                            Inlines =
                            {
                                new Run()
                                {
                                    FontWeight = ru.FontWeight,
                                    TextDecorations = ru.TextDecorations,
                                    FontStyle = ru.FontStyle,
                                    Text = matches[i].Groups["name"].Value
                                }
                            }
                        });

                    }
                }
            }
            textBlock.Text = string.Empty;
            foreach (var r in runs)
                textBlock.Inlines.Add(r);
        }

        private static void SplitByLines(TextBlock textBlock)
        {
            var text = textBlock.Text.Split("\r");
            textBlock.Text = "";
            foreach(var line in text)
            {
               // if (line.Length > 0)
                {
                    textBlock.Inlines.Add(new Run { Text = line });
                    textBlock.Inlines.Add(new LineBreak());
                }
            }

        }
        private static void CreateList(TextBlock textBlock)
        {
            foreach(var inline in textBlock.Inlines)
            {
                var run = (inline as Run);
                if (run == null)
                    continue;
                run.Text = listRegex.Replace(run.Text, "    • ");
            }
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
                else if(ru.GetType() == typeof(Hyperlink))
                {
                    s = ((ru as Hyperlink).Inlines[0] as Run).Text;
                }
                else
                {
                    runs.Add(ru);
                    continue;
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
                            Foreground = ru.Foreground,
                            FontSize = ru.FontSize
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
                            Foreground = ru.Foreground,
                            FontSize = ru.FontSize
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
                            FontSize = ru.FontSize,
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
            s = s.Replace("'", "''");
            return s;
        }
    }
}