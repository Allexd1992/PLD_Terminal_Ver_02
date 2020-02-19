using HslCommunication;
using HslCommunication.Profinet.Omron;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PLD_Terminal_Ver__02.Model
{
    class MainModel
    {
        OmronFinsNet OmronPLC_01, OmronPLC_03;
        OperateResult PLC_01_connect, PLC_03_connect;
        Thread thread;
        public float TapeStartPos {get; set;}
        public float TapeEndPos {get; set;}
        public float delta {get; set;}
        public float tapeLen {get; set;}
        public string TapeName {get; set;}
        public string TapeNameMon {get; set;}
        public int TargetNum {get; set;}
        public int TapeNum {get; set;}
        public  string TargetName {get; set;}
        public string TargetNameMon {get; set;}
        public int coil01 {get; set;}
        public int coil02 {get; set;}
        public  bool WbOn {get; set;}
        public bool WbOff {get; set;}
        public bool SetWbOff {get; set;}
        public bool SetWbOn {get; set;}
        public bool WbRun {get; set;}
        public float tapeCoord {get; set;}
        public float targetDeg {get; set;}
        private bool wrFlag=false;
        private bool rdPlc=false;
        private bool wrPLC = false;
        private float[] data = new float[35];     
        private string path = "IP_Adress.txt";
        private string PLC_01;
        private string PLC_03;
        private string PC;
        private int runNumb;
        private int runTime;
        public MainModel()
        {

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    PLC_01 = (sr.ReadLine());
                    PLC_03 = (sr.ReadLine());
                    PC = (sr.ReadLine());

                }
                OmronPLC_01 = new OmronFinsNet(PLC_01, 9600);
                OmronPLC_01.SA1 = Convert.ToByte(PC.Split('.')[3]);             
                OmronPLC_01.DA1 = Convert.ToByte(PLC_01.Split('.')[3]);
                PLC_01_connect = OmronPLC_01.ConnectServer();
                OmronPLC_03 = new OmronFinsNet(PLC_03, 9600);
                OmronPLC_03.SA1 = Convert.ToByte(PC.Split('.')[3]);
                OmronPLC_03.DA1 = Convert.ToByte(PLC_03.Split('.')[3]);// объект соединения 
                PLC_03_connect = OmronPLC_03.ConnectServer();
               // new PlcTcp();
            }
            catch
            {
                LogWriter.WriteLog("", "", "IP_Adress file is not found", 0);
            }




            try
            {
               // TargetName = PlcTcp.ReadStringPLC(PLC_01, 15400);
                TargetName = OmronPLC_01.ReadString("D15400", 99).Content;
                TapeName = PlcTcp.ReadStringPLC(PLC_01, 10000);
                TargetNum = PlcTcp.ReadIntPLC(PLC_01, 15500);
                TapeNum = PlcTcp.ReadIntPLC(PLC_01, 11280);
                coil01 = PlcTcp.ReadIntPLC(PLC_01, 11300);
                coil02 = PlcTcp.ReadIntPLC(PLC_01, 11302);
                TapeStartPos = PlcTcp.ReadRealPLC(PLC_01, 11270);
                TapeEndPos = PlcTcp.ReadRealPLC(PLC_01, 11275);
                tapeLen = PlcTcp.ReadRealPLC(PLC_01, 11292);
                delta = PlcTcp.ReadRealPLC(PLC_01, 11250);
                WbOn = PlcTcp.GetBitPLC(PLC_01, 11290, 2);
                WbOff = !PlcTcp.GetBitPLC(PLC_01, 11290, 2);
                SetWbOn = PlcTcp.GetBitPLC(PLC_01, 11290, 7);
                SetWbOff = !PlcTcp.GetBitPLC(PLC_01, 11290, 7);
                tapeLen = PlcTcp.ReadRealPLC(PLC_01, 11292);
            }
            catch
            {
                LogWriter.WriteLog("", "", "PLC is not response", 0);
            }
           
            var timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick1);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
            var timer2 = new DispatcherTimer();
            timer2.Tick += new EventHandler(timer_Tick2);
            timer2.Interval = new TimeSpan(0, 0, 1);
            timer2.Start();
            var timer3 = new DispatcherTimer();
            timer3.Tick += new EventHandler(timer_Tick3);
            timer3.Interval = new TimeSpan(0, 0, 2);
            timer3.Start();
        }
        public   void TapeUp()
        {
            //thread.Interrupt();

                wrPLC = true;
                thread.Abort();
                TapeNum = PlcTcp.ReadIntPLC(PLC_01, 11280);
                if (TapeNum < 10)
                {
                    TapeNum += 1;
                    PlcTcp.WriteIntPLC(PLC_01, 11280, (short)TapeNum);

                    TapeName = PlcTcp.ReadStringPLC(PLC_01, 10000);
                    TapeStartPos = PlcTcp.ReadRealPLC(PLC_01, 11270);
                    TapeEndPos = PlcTcp.ReadRealPLC(PLC_01, 11275);
                    tapeLen = PlcTcp.ReadRealPLC(PLC_01, 11292);
                }
            rdPlc = false;
            wrPLC = false;
           //thread.Resume();
        }
        public void TapeDown()
        {
            wrPLC = true;
            thread.Abort();
            TapeNum = PlcTcp.ReadIntPLC(PLC_01, 11280);
            if (TapeNum > 1)
            {
                TapeNum -= 1;
                PlcTcp.WriteIntPLC(PLC_01, 11280, (short)TapeNum);
                TapeName = PlcTcp.ReadStringPLC(PLC_01, 10000);
                TapeStartPos = PlcTcp.ReadRealPLC(PLC_01, 11270);
                TapeEndPos = PlcTcp.ReadRealPLC(PLC_01, 11275);
                tapeLen = PlcTcp.ReadRealPLC(PLC_01, 11292);
            }
            rdPlc = false;
            wrPLC = false;
        }
        public void TapeSave()
        {
            wrPLC = true;
            thread.Abort();
            PlcTcp.WriteRealPLC(PLC_01, 11270, TapeStartPos);
            PlcTcp.WriteRealPLC(PLC_01, 11275, TapeEndPos);
            PlcTcp.WriteStringPLC(PLC_01, TapeName, 10000);
            PlcTcp.WriteRealPLC(PLC_01, 11250, delta);
            PlcTcp.WriteIntPLC(PLC_01, 11300, (short)coil01);
            PlcTcp.WriteIntPLC(PLC_01, 11302, (short)coil02);
            PlcTcp.SetBitPLC(PLC_01, 11290, 0, true);
            wrPLC = false;
            rdPlc = false;
        }
        public void TargetUp()
        {
            wrPLC = true;
            thread.Abort();
            TargetNum = PlcTcp.ReadIntPLC(PLC_01, 15500);
            if (TargetNum < 3)
            {
                TargetNum += 1;
                PlcTcp.WriteIntPLC(PLC_01, 15500, (short)TargetNum);
                TargetName = PlcTcp.ReadStringPLC(PLC_01, 15400);
               
            }
            wrPLC = false;
            rdPlc = false;
        }
        public void TargetDown()
        {
            wrPLC = true;
            thread.Abort();
            TargetNum = PlcTcp.ReadIntPLC(PLC_01, 15500);
            if (TargetNum > 1)
            {
                TargetNum -= 1;
                PlcTcp.WriteIntPLC(PLC_01, 15500, (short)TargetNum);
                TargetName = PlcTcp.ReadStringPLC(PLC_01, 15400);

            }
            wrPLC = false;
            rdPlc = false;
        }
        public void TargetSave()
        {
            wrPLC = true;
            thread.Abort();
            PlcTcp.WriteStringPLC(PLC_01, TargetName, 15400);
            PlcTcp.SetBitPLC(PLC_01, 15504, 0, true);
            wrPLC = false;
            rdPlc = false;
        }
        private   void timer_Tick1(object sender, EventArgs e)
        {

            //if (thread.ThreadState == ThreadState.Unstarted)
            if (!rdPlc && !wrPLC)
            {
                
                // await Task.Run(() =>
                //{

                thread = new Thread(delegate (){
                    rdPlc = true;
                WbOn = PlcTcp.GetBitPLC(PLC_01, 11290, 2);
                WbOff = !PlcTcp.GetBitPLC(PLC_01, 11290, 2);
                SetWbOn = PlcTcp.GetBitPLC(PLC_01, 11290, 7);
                SetWbOff = !PlcTcp.GetBitPLC(PLC_01, 11290, 7);
                WbRun = PlcTcp.GetBitPLC(PLC_01, 11290, 11);
                TapeNameMon = PlcTcp.ReadStringPLC(PLC_01, 13000);
                TargetNameMon = PlcTcp.ReadStringPLC(PLC_01, 15000);
                tapeCoord = PlcTcp.ReadRealPLC(PLC_01, 11265);
                targetDeg = PlcTcp.ReadRealPLC(PLC_01, 22028);
                data[0] = PlcTcp.ReadRealPLC(PLC_03, 60);//L_Ten
                data[1] = PlcTcp.ReadRealPLC(PLC_03, 62);// R_Ten
                data[2] = PlcTcp.ReadRealPLC(PLC_03, 64);// O2_01
                data[3] = PlcTcp.ReadRealPLC(PLC_03, 66);// O2_02
                data[4] = PlcTcp.ReadRealPLC(PLC_03, 68);//Pressure_V
                data[5] = PlcTcp.ReadRealPLC(PLC_03, 70);//Temp_HSL
                data[6] = PlcTcp.ReadRealPLC(PLC_03, 72);//Temp_HSR
                data[7] = PlcTcp.ReadRealPLC(PLC_03, 74);//Temp_HF
                data[8] = PlcTcp.ReadRealPLC(PLC_03, 76);//Temp_HC
                data[9] = PlcTcp.ReadRealPLC(PLC_03, 78);//Temp_HB
                data[10] = PlcTcp.ReadRealPLC(PLC_03, 80);//Current_HSL
                data[11] = PlcTcp.ReadRealPLC(PLC_03, 82);//Current_HSR
                data[12] = PlcTcp.ReadRealPLC(PLC_03, 84);//Current_HF
                data[13] = PlcTcp.ReadRealPLC(PLC_03, 86);//Current_HC
                data[14] = PlcTcp.ReadRealPLC(PLC_03, 88);//Current_HB
                data[15] = PlcTcp.ReadRealPLC(PLC_03, 90);//Voltage_HSL
                data[16] = PlcTcp.ReadRealPLC(PLC_03, 92);//Voltage_HSR
                data[17] = PlcTcp.ReadRealPLC(PLC_03, 94);//Voltage_HF
                data[18] = PlcTcp.ReadRealPLC(PLC_03, 96);//Voltage_HC
                data[19] = PlcTcp.ReadRealPLC(PLC_03, 98);//Voltage_HB
                data[20] = PlcTcp.ReadRealPLC(PLC_03, 346);//FRG_pa
                data[21] = PlcTcp.ReadRealPLC(PLC_03, 400);//Pressure_pa
                data[22] = PlcTcp.ReadRealPLC(PLC_01, 22012);//M_Len
                data[23] = PlcTcp.ReadRealPLC(PLC_01, 22016);//R_Len
                data[24] = PlcTcp.ReadRealPLC(PLC_01, 22020);//L_Len
                data[25] = targetDeg;//Target deg
                data[26] = PlcTcp.ReadRealPLC(PLC_01, 22024);//Speed
                data[27] = PlcTcp.ReadRealPLC(PLC_01, 22036);//L_ten
                data[28] = PlcTcp.ReadRealPLC(PLC_01, 22032);//R_Ten
                data[29] = PlcTcp.ReadRealPLC(PLC_01, 22044);//L_Cl
                data[30] = PlcTcp.ReadRealPLC(PLC_01, 22040);//R_Cl
                runNumb = PlcTcp.ReadIntPLC(PLC_01, 11304); // Run_Number
                runTime = PlcTcp.ReadIntPLC(PLC_01, 122); //Run_Time
                rdPlc = false;
            });
                //});
               // thread.Start();
                Console.WriteLine(thread.ThreadState);
             
            thread.Start();
                Console.WriteLine(thread.ThreadState);
            }
        }
        private async void timer_Tick2(object sender, EventArgs e)
        {
           
            string Row = data[22] + ";" + data[23] + ";" + data[24] + ";" + TapeNameMon + ";" + tapeCoord + ";" + TargetNameMon + ";" + data[25] + ";" + data[26] + ";" + data[29] + ";" + data[30] + ";" + data[0] + ";" + data[1] + ";" + data[2] + ";" + data[3] + ";" + data[5] + ";" + data[6] + ";" + data[7] + ";" + data[8] + ";" + data[9] + ";" + data[10] + ";" + data[11] + ";" + data[12] + ";" + data[13] + ";" + data[14] + ";" + data[15] + ";" + data[16] + ";" + data[17] + ";" + data[18] + ";" + data[19] + ";" + data[20] + ";" + data[21] + ";" + runNumb + ";" + runTime + ";" + coil01 + ";" + coil02;
            string Head = "Date;Time;Length_m;R_Length_m;L_Length_m;TapeName;Coordinat_m;Target_name;Target_deg;Speed_mh;L_Cl_%;R_Cl_%;L_Ten_N;R_Ten_N;O2_AV01_SCCM;O2_AV02_SCCM;T_HSL_C;T_HSR_C;T_HF_C;T_HC_C;T_HB_C;C_HSL_A;C_HSR_A;C_HF_A;C_HC_A;C_HB_A;V_HSL_V;V_HSR_V;V_HF_V;V_HC_V;V_HB_V;FRG_Pa;CDG_Pa;Run_Number;Run_time;Coil01;Coil02";
            if (WbOn && !wrFlag && !rdPlc )
            {
                wrFlag = true;

                await Task.Run(() =>
                {
                    LogWriter.WriteLog(@"TapeLog\" + TapeNameMon + "_" +"Run_"+ runNumb, Head, Row, 1);
                    LogWriter.WriteLog(@"DayLog\" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year, Head, Row, 1);
                    wrFlag = false;
                });

            }
            else
            {
                await Task.Run(() =>
                {
                    LogWriter.WriteLog(@"DayLog\"+DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year, Head, Row, 1);
                    Console.WriteLine(DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year);
                    wrFlag = false;
                });
            }


        }
        private void timer_Tick3(object sender, EventArgs e)
        {
           

        }
        public  void AutoSettingRun()
        {
            wrPLC = true;
            thread.Abort();
            PlcTcp.SetBitPLC(PLC_01, 11290, 7, true);
            rdPlc = false;
            wrPLC = false;
        }
        public void AutoSettingStop()
        {
            wrPLC = true;
            thread.Abort();
            PlcTcp.SetBitPLC(PLC_01, 11290, 7, false);
            rdPlc = false;
            wrPLC = false;
        }
        public void BaseWrRun()
        {
            wrPLC = true;
            thread.Abort();
            PlcTcp.SetBitPLC(PLC_01, 11290, 4, true);
            PlcTcp.SetBitPLC(PLC_01, 11290, 8, false);
            rdPlc = false;
            wrPLC = false;
        }
        public void BaseWrStop()
        {
            wrPLC = true;
            thread.Abort();
            PlcTcp.SetBitPLC(PLC_01, 11290, 8, true);
            PlcTcp.SetBitPLC(PLC_01, 11290, 4, false);
            rdPlc = false;
            wrPLC = false;
        }
    }
}
