using OhMyWoodWorkerSimulator.Commands;
using OhMyWoodWorkerSimulator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OhMyWoodWorkerSimulator.ViewModels
{
    public class ControlWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            handler(this, new PropertyChangedEventArgs(name));
        }

        private Hacksaw _myHacsaw = new Hacksaw();
        private bool _visibilityMoveBuutons = false;
        private bool _visibilityStrings = false;
        private bool _visibilityOtherComponents = false;
        private bool _isAutoMode = false;
        private bool _isHandMode = false;
        private string _stateString = String.Empty;

        /// <summary>
        /// Выполнение кода при нажатаии на радиобатон в "Автоматическая"
        /// </summary>
        public ICommand workAutoMode
        {
            get
            {
                return new delegateCommand(new Action(() =>
                {
                    StateString = "Выбран автоматический режим работы";
                    IsAutoMode = true;
                    IsHandMode = false;
                    VisibilityMoveButons = false;
                    VisibilityStrings = true;
                    VisibilityOtherComponents = true;
                }));
            }
        }

        /// <summary>
        /// Выполнение кода при нажатаии на радиобатон в "Ручной"
        /// </summary>
        public ICommand workHandMode
        {
            get
            {
                return new delegateCommand(new Action(() =>
                {
                    StateString = "Выбран ручной режим работы";
                    IsAutoMode = false;
                    IsHandMode = true;
                    VisibilityMoveButons = true;
                    VisibilityStrings = false;
                    VisibilityOtherComponents = true;
                }));
            }
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
        /// <summary>
        /// Параметр отображать или не отображать кнопки ручного движения рубанка
        /// </summary>
        public bool VisibilityMoveButons { get => _visibilityMoveBuutons; set { _visibilityMoveBuutons = value; OnPropertyChanged("VisibilityMoveButons"); } }
        /// <summary>
        /// Параметр отображать или не отображать строк конечных координат
        /// </summary>
        public bool VisibilityStrings { get => _visibilityStrings; set { _visibilityStrings = value; OnPropertyChanged("VisibilityStrings"); } }
        /// <summary>
        /// Параметр отображать или не отображать общие строки для всех режимов
        /// </summary>
        public bool VisibilityOtherComponents { get => _visibilityOtherComponents; set { _visibilityOtherComponents = value; OnPropertyChanged("VisibilityOtherComponents"); } }
        /// <summary>
        /// Строка с начальной координатой Х
        /// </summary>
        public float BeginX { get => MyHacsaw.X0; set { MyHacsaw.X0 = value; OnPropertyChanged("BeginX"); } }
        /// <summary>
        /// Строка с начальной координатой Y
        /// </summary>
        public float BeginY { get => MyHacsaw.Y0; set { MyHacsaw.Y0 = value; OnPropertyChanged("BeginY"); } }
        /// <summary>
        /// Строка с конечной координатой X
        /// </summary>
        public float EndX { get => MyHacsaw.XEnd; set { MyHacsaw.XEnd = value; OnPropertyChanged("EndX"); } }
        /// <summary>
        /// Строка с конечной координатой Y
        /// </summary>
        public float EndY { get => MyHacsaw.YEnd; set { MyHacsaw.YEnd = value; OnPropertyChanged("EndY"); } }
        /// <summary>
        /// Строка с шириной рубанка
        /// </summary>
        public float Width { get => MyHacsaw.Width; set { MyHacsaw.Width = value; OnPropertyChanged("Width"); } }
        /// <summary>
        /// Строка с длинной шага
        /// </summary>
        public float LengthStep { get => MyHacsaw.LengthStep; set { MyHacsaw.LengthStep = value; OnPropertyChanged("LengthStep"); } }
        /// <summary>
        /// Объект рубанок
        /// </summary>
        internal Hacksaw MyHacsaw { get => _myHacsaw; set => _myHacsaw = value; }
        /// <summary>
        /// Флаг, определяющий включен ли автоматический режим  
        /// </summary>
        public bool IsAutoMode { get => _isAutoMode; set => _isAutoMode = value; }
        /// <summary>
        /// Флаг, определяющий включен ли ручной режим  
        /// </summary>
        public bool IsHandMode { get => _isHandMode; set => _isHandMode = value; }
        /// <summary>
        /// Строка отображающая, в каком сейчас режиме находится рубанок
        /// </summary>
        public string StateString { get => _stateString; set { _stateString = value; OnPropertyChanged("StateString"); } }
    }
}
