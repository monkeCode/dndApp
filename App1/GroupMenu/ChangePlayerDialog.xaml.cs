using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Model;
using Windows.UI.Xaml.Controls;

namespace App.GroupMenu
{
    public sealed partial class ChangePlayerDialog : ContentDialog, INotifyPropertyChanged
    {
        public Player Player { get; set; } = new();


        public ChangePlayerDialog()
        {
            InitializeComponent();
        }

        public ChangePlayerDialog(Player player)
        {
            Player = player;
            InitializeComponent();
        }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {

    }

    private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {

    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    }
}
