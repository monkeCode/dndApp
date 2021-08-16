using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace App1
{
    class NumericalTextBox:TextBox
    {
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if (char.IsNumber((char)e.Key) || e.Key == Windows.System.VirtualKey.Enter || e.Key ==Windows.System.VirtualKey.Back)
            {
                return;
            }
            e.Handled = true;
        }
    }
	}
