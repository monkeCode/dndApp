using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DataBaseLib;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.DefaultFileExtension = ".db";
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Database file", new List<string>() { ".db" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "DataBase";
            var newFile = await savePicker.PickSaveFileAsync();
            if (newFile != null)
            {

               var db = await StorageFile.GetFileFromPathAsync(DataAccess.DbPath);
               if (db != null)
               {
                  var buffer = await FileIO.ReadBufferAsync(db);
                  await FileIO.WriteBufferAsync(newFile, buffer);
               }
            }
        }

        private async void UpLoadDataBase(object sender, RoutedEventArgs e)
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.SuggestedStartLocation = 
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".db");
            
            var newFile = await openPicker.PickSingleFileAsync();
            if (newFile != null)
            {
                StorageFile databaseFile = await StorageFile.GetFileFromPathAsync(DataAccess.DbPath);
                //await newFile.RenameAsync(DataAccess.DB_NAME);
                await newFile.CopyAndReplaceAsync(databaseFile);

            }
        }
    }
}
