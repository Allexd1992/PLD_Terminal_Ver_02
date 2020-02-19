using PLD_Terminal_Ver__02.Model;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace PLD_Terminal_Ver__02.ViewModel
{
    class MainViewModel: INotifyPropertyChanged
    {
        MainModel model;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop="")
        {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #region Tags
        private string tapeName;
        public string  sTapeName
        {
            get { return tapeName; }
            set { if (tapeName != value) { tapeName = value; OnPropertyChanged();GC.Collect(); } }
        }

        private string targetName;
        public string sTargetName
        {
            get { return targetName; }
            set { if (targetName != value) { targetName = value; OnPropertyChanged(); GC.Collect(); } }
        }

        private string tapeStartPos;
        public string sTapeStartPos
        {
            get { return tapeStartPos; }
            set { if (tapeStartPos != value) { tapeStartPos = value; OnPropertyChanged(); GC.Collect(); } }
        }

        private string tapeEndPos;
        public string sTapeEndPos
        {
            get { return tapeEndPos; }
            set { if (tapeEndPos != value) { tapeEndPos = value; OnPropertyChanged(); GC.Collect(); } }
        }

        private string delta;
        public string sDelta
        {
            get { return delta; }
            set { if (delta != value) { delta = value; OnPropertyChanged(); GC.Collect(); } }
        }

        private string lehgth;
        public string slehgth
        {
            get { return lehgth; }
            set { if (lehgth != value) { lehgth = value; OnPropertyChanged(); GC.Collect(); } }
        }

        private string coil01;
        public string sCoil01
        {
            get { return coil01; }
            set { if (coil01 != value) { coil01 = value; OnPropertyChanged(); GC.Collect(); } }
        }

        private string coil02;
        public string sCoil02
        {
            get { return coil02; }
            set { if (coil02 != value) { coil02 = value; OnPropertyChanged(); GC.Collect(); } }
        }

        private string tapeNum;
        public string sTapeNum
        {
            get { return tapeNum; }
            set { if (tapeNum != value) { tapeNum = value; OnPropertyChanged(); GC.Collect(); } }
        }

        private string targetNum;
        public string sTargetNum
        {
            get { return tapeNum; }
            set { if (targetNum != value) { targetNum = value; OnPropertyChanged(); GC.Collect(); } }
        }

        private string targetNameMon;
        public string sTargetNameMon
        {
            get { return targetNameMon; }
            set { if (targetNameMon != value) { targetNameMon = value; OnPropertyChanged(); GC.Collect(); } }
        }

        private string tapeNameMon;
        public string sTapeNameMon
        {
            get { return tapeNameMon; }
            set { if (tapeNameMon != value) { tapeNameMon = value; OnPropertyChanged(); GC.Collect(); } }
        }

        private string targetDeg;
        public string sTargetDeg
        {
            get { return targetDeg; }
            set { if (targetDeg != value) { targetDeg = value; OnPropertyChanged(); GC.Collect(); } }
        }

        private string tapeCoor;
        public string sTapeCoor
        {
            get { return tapeCoor; }
            set { if (tapeCoor != value) { tapeCoor = value; OnPropertyChanged(); GC.Collect(); } }
        }


        private string wrDbOn;
        public string sWrDbOn
        {
            get { return wrDbOn; }
            set { if (wrDbOn != value) { wrDbOn = value; OnPropertyChanged(); GC.Collect(); } }
           
        }
        private string wrDbOff;
        public string sWrDbOff
        {
            get { return wrDbOff; }
            set { if (wrDbOff != value) { wrDbOff = value; OnPropertyChanged(); GC.Collect(); } }
        }
        private string autoWrOn;
        public string sAutoWrOn
        {
            get { return autoWrOn; }
            set { if (autoWrOn != value) { autoWrOn = value; OnPropertyChanged(); GC.Collect(); } }
         
        }
        private string autoWrOff;
        public string sAutoWrOff
        {
            get { return autoWrOff; }
            set { if (autoWrOff != value) { autoWrOff = value; OnPropertyChanged(); GC.Collect(); } }
        }

        private string wrDbRun;
        public string sWrDbRun
        {
            get { return wrDbRun; }
            set { if (wrDbRun != value) { wrDbRun = value; OnPropertyChanged(); GC.Collect(); } }
        }
        #endregion
        #region Buttons
        public ICommand SaveTape
        {
            get
            {
                return new DelegateCommand(() =>
                {
                   
                        model.TapeName = sTapeName;
                        model.TapeStartPos = (float)Convert.ToDouble(sTapeStartPos);
                        model.TapeEndPos = (float)Convert.ToDouble(sTapeEndPos);
                        model.coil01 = Convert.ToInt32(sCoil01);
                        model.coil02 = Convert.ToInt32(sCoil02);
                        model.delta = (float)Convert.ToDouble(sDelta);
                        model.TapeSave();
                   

                });
            }
        }
        public ICommand SaveTarget
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    model.TargetName = sTargetName;
                });
            }
        }
        public ICommand TapeUp
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    model.TapeUp();
                    RefreshSet();

                });
            }
        }
        public ICommand TapeDown
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    model.TapeDown();
                    RefreshSet();
                });
            }
        }
        public ICommand TargetDown
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    model.TargetDown();
                    RefreshSet();
                });
            }
        }
        public ICommand TargetUp
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    model.TargetUp();
                    RefreshSet();
                });
            }
        }

        public ICommand RunWB
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    model.BaseWrRun();
                });
            }
        }
        public ICommand StopWB
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    model.BaseWrStop();
                });
            }
        }


        public ICommand OnAutoWB
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    model.AutoSettingRun();
                });
            }
        }
        public ICommand OffAutoWB
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    model.AutoSettingStop();
                });
            }
        }
        #endregion

        private void RefreshSet()
        {
            sTapeNum = model.TapeNum.ToString();
            sTargetNum = model.TargetNum.ToString();
            sTapeName = model.TapeName;
            sTargetName = model.TargetName;
            slehgth = String.Format("{0:0.0}", model.tapeLen);
            sTapeStartPos = String.Format("{0:0.0}", model.TapeStartPos);
            sTapeEndPos = String.Format("{0:0.0}", model.TapeEndPos);
            sDelta = String.Format("{0:0}", model.delta);
            sCoil01 = String.Format("{0:0}", model.coil01);
            sCoil02 = String.Format("{0:0}", model.coil02);
        }
        private void RefreshCV()
        {
            sTapeNameMon = model.TapeNameMon;
            sTargetNameMon = model.TargetNameMon;
            sTargetDeg = String.Format("{0:0.00}", model.targetDeg);
            sTapeCoor = String.Format("{0:0.00}", model.tapeCoord);
            sWrDbOn = model.WbOn ? "True" : "False";
            sWrDbOff = !model.WbOn ? "True" : "False";
            sAutoWrOn = model.SetWbOn ? "True" : "False";
            sAutoWrOff = !model.SetWbOn ? "True" : "False";
            sWrDbRun = model.WbRun ? "True" : "False";
        }
        public MainViewModel()
        {
            model = new MainModel();
            RefreshSet();
            var timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            RefreshCV();

        }
    }
}
