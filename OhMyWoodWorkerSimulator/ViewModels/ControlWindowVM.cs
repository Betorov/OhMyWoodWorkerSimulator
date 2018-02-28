using OhMyWoodWorkerSimulator.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OhMyWoodWorkerSimulator.ViewModels
{
    class ControlWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            handler(this, new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// Выполнение кода при нажатии кнопки "Применить"
        /// </summary>
        public ICommand clickBtn_Accept
        {
            get
            {
                return new delegateCommand(new Action(() =>
                {
                    
                }));
            }
        }
        /// <summary>
        /// Выполнение кода при нажатии кнопки "Стоп"
        /// </summary>
        public ICommand clickBtn_Stop
        {
            get
            {
                return new delegateCommand(new Action(() =>
                {

                }));
            }
        }
    }
}
