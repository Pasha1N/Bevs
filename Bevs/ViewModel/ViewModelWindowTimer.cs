using Bevs.Model;
using Bevs.View;
using Bevs.Wpf.Commands;
using Bevs.Wpf.ViewModel;
using System;
using System.ComponentModel;
using System.Timers;
using System.Windows.Input;

namespace Bevs.ViewModel
{
    public class ViewModelWindowTimer : EventINotifyPropertyChanged
    {
        private ICommand cancel;
        private double responseTime;
        private bool turnOffPC;
        private ICommand setTimer;
        private Timer timer;
        private bool isTimerSet =false;
        private bool enableBtnSetTimer = true;

        public ViewModelWindowTimer()
        {
            cancel = new DelegateCommand(Cancel);
            setTimer = new DelegateCommand(SetTimer);
        }

        public ICommand BtnCancel => cancel;
        public ICommand BtnSetTimer => setTimer;
        public WindowTimer WindowTimer { get; set; }
        public bool IsTimerSet => isTimerSet;
        public bool EnableBtnSetTimer
        {
            get => enableBtnSetTimer;
            set => SetProperty(ref enableBtnSetTimer, value);
        }
        public double ResponseTime
        {
            get => responseTime;
            set => SetProperty(ref responseTime, value);
        }
        public bool TurnOffPC
        {
            get => turnOffPC;
            set => SetProperty(ref turnOffPC, value);
        }

        private void Cancel()
        {
            WindowTimer.Close();
        }

        private void SetTimer()
        {
            timer = new Timer();
            timer.Elapsed += foo;
            timer.Interval = ResponseTime;
                isTimerSet = true;
            WindowTimer.Close();
        }
        private void foo(object sander, EventArgs e)
        {

        }

        public void UpdateDataTimer()
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(ResponseTime)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(TurnOffPC)));
        }
    }
}