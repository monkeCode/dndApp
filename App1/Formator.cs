﻿using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace App1.WorkShop
{
    internal class Formator
    {
        public static void StringtoText(string s, TextBlock textBlock)
        {
            Formating(s, '~', textBlock);
            Formating(s, '`', textBlock);
            Formating(s, '|', textBlock);
        }

        private static void Formating(string s, char formatfactor, TextBlock textBlock)
        {
            List<Run> runs = new List<Run>();
            foreach (Run ru in textBlock.Inlines)
            {
                string[] str = ru.Text.Split(formatfactor);
                for (int i = 0; i < str.Length; i++)
                {
                    Run run = new Run();
                    run.FontWeight = ru.FontWeight;
                    run.TextDecorations = ru.TextDecorations;
                    run.FontStyle = ru.FontStyle;
                    run.Text = str[i];
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
    }
}