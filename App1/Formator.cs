using System.Collections.Generic;
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
            Formating('~', textBlock);
            Formating('`', textBlock);
            Formating('|', textBlock);
            AddHyperlink(textBlock);
        }

        private static void Formating(char formatfactor, TextBlock textBlock)
        {
            List<Run> runs = new List<Run>();
            foreach (Run ru in textBlock.Inlines)
            {
                string[] str = ru.Text.Split(formatfactor);
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
                        switch (formatfactor)
                        {
                            case '~':
                                run.FontWeight = Windows.UI.Text.FontWeights.Bold;
                                break;

                            case '|':
                                run.TextDecorations = Windows.UI.Text.TextDecorations.Underline;
                                break;

                            case '`':
                                run.FontStyle = Windows.UI.Text.FontStyle.Italic;
                                break;
                        }
                    }
                    runs.Add(run);
                }
            }
            textBlock.Text = string.Empty;
            foreach (var r in runs)
                textBlock.Inlines.Add(r);
        }

        private static void AddHyperlink(TextBlock textBlock)
        {
            List<Inline> runs = new List<Inline>();
            foreach (Run ru in textBlock.Inlines)
            {
                string[] str = ru.Text.Split("^");
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
                        hyperlink.Inlines.Add(new Run() { Text = run.Text });
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
    }
}