using MVVMProject.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TODO_list
{

    internal class ViewModel : ViewModelBase
    {
        public RelayCommand AddCommand => new RelayCommand(execute => AddQuest());
        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteQuest(), canExecute => SelectedQuest != null);
        public ObservableCollection<Quest> Quests { get; set; }
        public ViewModel()
        {
            Quests = new ObservableCollection<Quest>();
        }

        private Quest selectedQuest;
        public Quest SelectedQuest
        {
            get { return selectedQuest; }
            set { selectedQuest = value; OnPropertyChanged(); }
        }

        private string inputtedText;
        public string InputtedText
        {
            get { return inputtedText; }
            set { inputtedText = value; OnPropertyChanged(); }
        }
        private DateTime inputtedDeadline;
		public DateTime InputtedDeadline
		{
			get { return inputtedDeadline; }
			set { inputtedDeadline = value; OnPropertyChanged(); }
		}
		private int inputtedImportance;
		public int InputtedImportance
		{
			get { return inputtedImportance; }
			set { inputtedImportance = value; OnPropertyChanged(); }
		}

		public void AddQuest()
        {
            Quests.Add(new Quest(InputtedText, InputtedDeadline, InputtedImportance));
        }
        public void DeleteQuest()
        {
            Quests.Remove(selectedQuest);
        }
    }
}
