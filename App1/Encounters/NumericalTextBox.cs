using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace App1
{
    class NumericalTextBox : TextBox
    {
        static readonly List<Windows.System.VirtualKey> _alllowedKeys = new()
        {
            Windows.System.VirtualKey.Enter,
            Windows.System.VirtualKey.Clear,
            Windows.System.VirtualKey.Accept,
            Windows.System.VirtualKey.Left,
            Windows.System.VirtualKey.Right,
            Windows.System.VirtualKey.Back

        };
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {

            if (char.IsNumber((char)e.Key) || _alllowedKeys.IndexOf(e.Key) != -1)
            {
                e.Handled = false;
                base.OnKeyDown(e);
                return;
            }
            e.Handled = true;
        }
    }
}
