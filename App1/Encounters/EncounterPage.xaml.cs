using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1.Encounters
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class EncounterPage : Page
    {

        private List<BattleMonster> dragableItems = new List<BattleMonster>();
        ListView originalSource;
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
            (DataContext as EncounterModelView).MonsterModel.SelectedSource = (sender as ListView).SelectedItems;
            (DataContext as EncounterModelView).GetMonsterData();
        }

        private void HabbiatList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as EncounterModelView).MonsterModel.SelectedHabitat = (sender as ListView).SelectedItems;
            (DataContext as EncounterModelView).GetMonsterData();
        }

        private void SizeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as EncounterModelView).MonsterModel.SelectedSize = (sender as ListView).SelectedItems;
            (DataContext as EncounterModelView).GetMonsterData();
        }

        private void ChallengeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as EncounterModelView).MonsterModel.SelectedRate = (sender as ListView).SelectedItems;
            (DataContext as EncounterModelView).GetMonsterData();
        }

        private void TypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as EncounterModelView).MonsterModel.SelectedType = (sender as ListView).SelectedItems;
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
            if (originalSource != sender)
                e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
        }



        private void ListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {

            dragableItems.Clear();
            foreach (var i in e.Items)
            {
                dragableItems.Add((BattleMonster)i);
                originalSource = sender as ListView;
            }
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            ListView lv = sender as ListView;

            foreach (var i in dragableItems)
            {
                if (((lv.DataContext) as Encounter).Monsters.FirstOrDefault(monster => monster.Monster.Id == i.Monster.Id) == null)
                    ((lv.DataContext) as Encounter).Monsters.Add(new BattleMonster() { Monster = i.Monster, Quantity = i.Quantity });
                else
                    ((lv.DataContext) as Encounter).Monsters.First(monster => monster.Monster.Id == i.Monster.Id).Quantity++;
                var obj = originalSource.DataContext as Encounter;
                if (obj != null)
                    ((originalSource.DataContext) as Encounter).Monsters.Remove(i);

            }

        }

        private void ListView_Drop_1(object sender, DragEventArgs e)
        {


            foreach (var i in dragableItems)
            {
                ((originalSource.DataContext) as Encounter).Monsters.Remove(i);

            }
        }

        private void ListView_DragOver_1(object sender, DragEventArgs e)
        {

            if (sender != originalSource)
                e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            (DataContext as EncounterModelView).Dispose();
            base.OnNavigatedFrom(e);
        }

    }
}