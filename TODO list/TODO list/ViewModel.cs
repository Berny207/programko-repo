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
        public RelayCommand AddCommand => new RelayCommand(execute => AddTask());
        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteTask(), canExecute => SelectedTask != null);
        public ObservableCollection<Item> Tasks { get; set; }
        public ViewModel()
        {
            Tasks = new ObservableCollection<Item>();
        }

        private Item selectedTask;
        public Item SelectedTask
        {
            get { return selectedTask; }
            set { selectedTask = value; OnPropertyChanged(); }
        }

        public void AddTask()
        {
            if(SelectedTask == null)
            {
                Tasks.Add(new Item());
                return;
            }
            Tasks.Add(SelectedTask);

        }
        public void DeleteTask()
        {
            Tasks.Remove(selectedTask);
        }
    }
}
