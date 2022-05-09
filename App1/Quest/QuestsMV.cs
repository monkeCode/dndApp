using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using App1.Annotations;

namespace App1.Quest
{
    internal class QuestsMV: INotifyPropertyChanged
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
