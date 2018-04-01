using ExchangeChannel.Network;
using OhMyWoodWorkerSimulator.Commands;
using OhMyWoodWorkerSimulator.Models;
using OhMyWoodWorkerSimulator.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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

        private bool isConnected = false;
        private Exchanger _myExchanger;

        private Hacksaw _myHacsaw = new Hacksaw();
        private bool _visibilityMoveBuutons = false;
        private bool _visibilityStrings = false;
        private bool _visibilityOtherComponents = false;
        private bool _isAutoMode = false;
        private bool _isHandMode = false;
        private bool _isWork = false;
        private string _stateString = String.Empty;
        private bool _flagException = false;

        private string _xBegin = String.Empty;
        private string _yBegin = String.Empty;
        private string _xEnd = String.Empty;
        private string _yEnd = String.Empty;
        private string _width = String.Empty;
        private string _lengthStep = String.Empty;

        public ControlWindowVM()
        {
            Task.Factory.StartNew(() =>{
                try
                {
                    var exchangeChannel = new ExchangeChannel.Network.Channel();
                    IPAddress iPAddress = IPAddress.Parse("127.0.0.1");

                    exchangeChannel.ConnectToServer(IPAddress.Parse("89.179.187.119"), 25565);
                    MyExchanger = new Exchanger(exchangeChannel);
                    //MyExchanger.SendHandshakeRequestAsync();
                    IsConnected = true;
                    MessageBox.Show("!!!!");
                }
                catch (Exception e) { IsConnected = false; }
            });
        }
        private float isNumber(string number)
        {
         
            float num = 0;
            number = number.Replace('.', ',');
            if (float.TryParse(number, out num)) return num;
            else return 0;
        }

        private bool isNum(string num)
        {
            float n = 0;
            num = num.Replace('.', ',');
            if (float.TryParse(num, out n)) return true;
            else return false;
        }

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
                    IsWork = false;
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
                    IsWork = false;
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
                    if(IsAutoMode)
                    {
                        if (LengthStep != String.Empty &&
                        Width != String.Empty &&
                        BeginX != String.Empty &&
                        BeginY != String.Empty &&
                        EndX != String.Empty &&
                        EndY != String.Empty)
                        {
                            MessageBox.Show("Got it!");
                            IsWork = true;
                            FlagException = false;
                            if(IsConnected) MyExchanger.SendAutoCutRequest(MyHacsaw.X0, MyHacsaw.Y0, MyHacsaw.XEnd, MyHacsaw.YEnd, MyHacsaw.Width);
                            return;
                        }
                        else FlagException = true;
                    }

                    if (IsHandMode)
                    {
                        if (
                        Width != String.Empty &&
                        BeginX != String.Empty &&
                        BeginY != String.Empty 
                        )
                        {
                            MessageBox.Show("Got it!");
                            IsWork = true;
                            FlagException = false;
                            
                            return;
                        }
                        else FlagException = true;
                    }

                    if (FlagException) MessageBox.Show("Не все поля заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    
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
                    IsWork = false;

                    if (IsConnected) MyHacsaw.XCurrent = MyExchanger.GetBrickParams().X;
                    if (IsConnected) MyHacsaw.YCurrent = MyExchanger.GetBrickParams().Y;
                    MyHacsaw.X0 = MyHacsaw.XCurrent;
                    MyHacsaw.Y0 = MyHacsaw.YCurrent;
                }));
            }
        }
        /// <summary>
        /// Нажатие клавиши вверх на клавиатуре
        /// </summary>
        public ICommand clickButtonUp
        {
            get
            {
                return new delegateCommand(new Action(() =>
                {
                    if (IsHandMode && IsWork && (!FlagException))
                    {
                        MyHacsaw.X0++;
                        BeginX = MyHacsaw.X0.ToString();
                        VisibilityMoveButons = true;

                        //MessageBox.Show("Up!");
                        if (IsConnected) MyExchanger.SendManualCutRequest(EDirection.Up, MyHacsaw.LengthStep, MyHacsaw.Width);
                    }
                }));
            }
        }

        /// <summary>
        /// Нажатие клавиши вниз на клавиатуре
        /// </summary>
        public ICommand clickButtonDown
        {
            get
            {
                return new delegateCommand(new Action(() =>
                {
                    if (IsHandMode && IsWork&&(!FlagException))
                    {
                        MessageBox.Show("Down!");
                        if (IsConnected) MyExchanger.SendManualCutRequest(EDirection.Down, MyHacsaw.LengthStep, MyHacsaw.Width);
                    }
                }));
            }
        }

        /// <summary>
        /// Нажатие клавиши влево на клавиатуре
        /// </summary>
        public ICommand clickButtonLeft
        {
            get
            {
                return new delegateCommand(new Action(() =>
                {
                    if (IsHandMode && IsWork && (!FlagException))
                    {
                        MessageBox.Show("Left!");
                        if (IsConnected) MyExchanger.SendManualCutRequest(EDirection.Left, MyHacsaw.LengthStep, MyHacsaw.Width);
                    }
                }));
            }
        }

        /// <summary>
        /// Нажатие клавиши вправо на клавиатуре
        /// </summary>
        public ICommand clickButtonRight
        {
            get
            {
                return new delegateCommand(new Action(() =>
                {
                    if (IsHandMode && IsWork && (!FlagException))
                    {
                        MessageBox.Show("Right!");
                        if (IsConnected) MyExchanger.SendManualCutRequest(EDirection.Right, MyHacsaw.LengthStep, MyHacsaw.Width);
                    }
                }));
            }
        }

        public ICommand clickBtn_Try
        {
            get
            {
                return new delegateCommand(new Action(() =>
                {
                    
                    //if( Key.Up)
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
        public string BeginX
        {
            get
            {
                if (!isNum(_xBegin) && _xBegin != String.Empty)
                {
                    BeginX = _xBegin.Substring(0, _xBegin.Length - 1);
                }
                else MyHacsaw.X0 = isNumber(_xBegin);
                return _xBegin; }
            set
            {
                _xBegin = value;
                OnPropertyChanged("BeginX");
            } }

        /// <summary>
        /// Строка с начальной координатой Y
        /// </summary>
        public string BeginY
        {
            get
            {
                if (!isNum(_yBegin) && _yBegin != String.Empty)
                {
                    BeginY = _yBegin.Substring(0, _yBegin.Length - 1);
                }
                else MyHacsaw.Y0 = isNumber(_yBegin);
                return _yBegin;
            }
            set
            {
                _yBegin = value;
                OnPropertyChanged("BeginY");
            }
        }
        /// <summary>
        /// Строка с конечной координатой X
        /// </summary>
        public string EndX
        {
            get
            {
                if (!isNum(_xEnd) && _xEnd != String.Empty)
                {
                    EndX = _xEnd.Substring(0, _xEnd.Length - 1);
                }
                else MyHacsaw.XEnd = isNumber(_xEnd);
                return _xEnd; }
            set
            {
                _xEnd = value;
                OnPropertyChanged("EndX");
            }
        }
        /// <summary>
        /// Строка с конечной координатой Y
        /// </summary>
        public string EndY
        {
            get
            {
                if (!isNum(_yEnd) && _yEnd != String.Empty)
                {
                    EndY = _yEnd.Substring(0, _yEnd.Length - 1);
                }
                else MyHacsaw.YEnd = isNumber(_yEnd);
                return _yEnd;
            }
            set
            {
                _yEnd = value;
                OnPropertyChanged("EndY");
            }
        }
        /// <summary>
        /// Строка с шириной рубанка
        /// </summary>
        public string Width
        {
            get
            {
                if (!isNum(_width) && _width != String.Empty)
                {
                    Width = _width.Substring(0, _width.Length - 1);
                }
                else MyHacsaw.Width = isNumber(_width);
                return _width;
            }
            set
            {
                _width = value;
                OnPropertyChanged("Width");
            }
        }
        /// <summary>
        /// Строка с длинной шага
        /// </summary>
        public string 
            LengthStep
        {
            get
            {
                if (!isNum(_lengthStep) && _lengthStep != String.Empty)
                {
                    LengthStep = _lengthStep.Substring(0, _lengthStep.Length - 1);
                }
                else MyHacsaw.LengthStep = isNumber(_lengthStep);
                return _lengthStep;
            }
            set
            {
                _lengthStep = value;
                OnPropertyChanged("LengthStep");
            }
        }
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
        /// <summary>
        /// Флаг определяющий работает сейчас рубанок или нет
        /// </summary>
        public bool IsWork { get => _isWork; set => _isWork = value; }
        /// <summary>
        /// Флаг, отвечающий о наличии ошибок (пустые строки)
        /// </summary>
        public bool FlagException { get => _flagException; set => _flagException = value; }
        public Exchanger MyExchanger { get => _myExchanger; set => _myExchanger = value; }
        /// <summary>
        /// Флаг проверки подключения к серверу
        /// </summary>
        public bool IsConnected { get => isConnected; set => isConnected = value; }
    }
}
