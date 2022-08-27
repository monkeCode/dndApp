using App.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace App.Quest
{
    internal class QuestsMV : INotifyPropertyChanged
    {
        public IObservable<QuestModel> ActiveQuests { get; set; }
        public IObservable<QuestModel> CompletedQuests { get; set; }
        public IObservable<QuestModel> NonActiveQuests { get; set; }
        private QuestModel _selectedQuest;

        public QuestModel SelectedQuest
        {
            get => _selectedQuest;
            set
            {
                _selectedQuest = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
