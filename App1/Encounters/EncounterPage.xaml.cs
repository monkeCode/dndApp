﻿using App.Directories;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App.Encounters
{

    public sealed partial class EncounterPage : Page
    {

        private List<EncountingMonster> _dragableItems = new List<EncountingMonster>();
        private ListView _originalSource;
        public EncounterPage()
        {
            DataContext = new EncounterModelView();
            this.InitializeComponent();
        }

        private void EncounterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView list = sender as ListView;
            (DataContext as EncounterModelView).SelectionEncountersChanged(list.SelectedItems.GetEnumerator());
        }


        private void SourceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as EncounterModelView).MonsterModel.SelectedSource = (sender as ListView).SelectedItems.Select(it => it.ToString()).ToList();
            (DataContext as EncounterModelView).GetMonsterData();
        }

        private void HabbiatList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as EncounterModelView).MonsterModel.SelectedHabitat = (sender as ListView).SelectedItems.Select(it => it.ToString()).ToList();
            (DataContext as EncounterModelView).GetMonsterData();
        }

        private void SizeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as EncounterModelView).MonsterModel.SelectedSize = (sender as ListView).SelectedItems.Select(it => it.ToString()).ToList();
            (DataContext as EncounterModelView).GetMonsterData();
        }

        private void ChallengeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as EncounterModelView).MonsterModel.SelectedRate = (sender as ListView).SelectedItems.Select(it => it.ToString()).ToList();
            (DataContext as EncounterModelView).GetMonsterData();
        }

        private void TypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as EncounterModelView).MonsterModel.SelectedType = (sender as ListView).SelectedItems.Select(it => it.ToString()).ToList();
            (DataContext as EncounterModelView).GetMonsterData();
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            (DataContext as EncounterModelView).MonsterModel.SubstringFilter = (sender as TextBox).Text;
            (DataContext as EncounterModelView).GetMonsterData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }


        private void ListView_DragOver(object sender, DragEventArgs e)
        {
            if (_originalSource != sender)
                e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
        }



        private void ListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {

            _dragableItems.Clear();
            foreach (var i in e.Items)
            {
                _dragableItems.Add((EncountingMonster)i);
                _originalSource = sender as ListView;
            }
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            ListView lv = sender as ListView;

            foreach (var i in _dragableItems)
            {
                if (((lv.DataContext) as Encounter).Monsters.FirstOrDefault(monster => monster.Monster.Id == i.Monster.Id) == null)
                    ((lv.DataContext) as Encounter).Monsters.Add(new EncountingMonster() { Monster = i.Monster, Quantity = i.Quantity });
                else
                    ((lv.DataContext) as Encounter).Monsters.First(monster => monster.Monster.Id == i.Monster.Id).Quantity++;
                var obj = _originalSource.DataContext as Encounter;
                if (obj != null)
                    ((_originalSource.DataContext) as Encounter).Monsters.Remove(i);

            }

        }

        private void ListView_Drop_1(object sender, DragEventArgs e)
        {


            foreach (var i in _dragableItems)
            {
                ((_originalSource.DataContext) as Encounter).Monsters.Remove(i);

            }
        }

        private void ListView_DragOver_1(object sender, DragEventArgs e)
        {

            if (sender != _originalSource)
                e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            (DataContext as EncounterModelView).Dispose();
            base.OnNavigatedFrom(e);
        }

        private void IncrementMonster(object sender, RoutedEventArgs e)
        {
            ((sender as Button).DataContext as EncountingMonster).Quantity++;
        }

        private void DecrementMonster(object sender, RoutedEventArgs e)
        {
            ((sender as Button).DataContext as EncountingMonster).Quantity--;
        }

        private void DeleteEncounter(object sender, RoutedEventArgs e)
        {
            var enc = ((sender as Button).DataContext as Encounter);
            (DataContext as EncounterModelView).DeleteEncounter(enc);
        }

        private void SwipeToMonsters(object sender, RoutedEventArgs e)
        {
            SwitchPresenter.Value = "Monsters";
        }

        private void ListView_MosnterClick(object sender, ItemClickEventArgs e)
        {
            monsterFrame.Navigate(typeof(MonsterPage), (e.ClickedItem as EncountingMonster).Monster.Id);
            SwitchPresenter.Value = "Monster";
        }

        private void Hyperlink_MonsterNameClick(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            var monster = (from m in (DataContext as EncounterModelView).MonsterList
                           where m.Monster.Name == (sender.Inlines[0] as Run).Text select m.Monster).First();
            monsterFrame.Navigate(typeof(MonsterPage), monster.Id);
            SwitchPresenter.Value = "Monster";
        }

        private async void ChangeGroup(object sender, RoutedEventArgs e)
        {
            GroupSelector groupSelector = new GroupSelector();
            var result = await groupSelector.ShowAsync();
            if(result == ContentDialogResult.Primary)
            {
                (DataContext as EncounterModelView).ChangeGroup(groupSelector.selectedGroup.Id);
            }
        }
    }
}