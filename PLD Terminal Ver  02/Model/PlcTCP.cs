using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmronFinsTCP.Net;

namespace PLD_Terminal_Ver__02.Model
{
    

    class PlcTcp
    {
        public static double GetValueD(string ip, short adr)
        {
            EtherNetPLC PLC = new EtherNetPLC();
            PLC.Link(ip, 9600, 1000);
            short[] buff = new short[2];
            byte[] buff2 = new byte[4];
            byte[] buff3 = new byte[2];
            PLC.ReadWords(PlcMemory.DM, adr, 2, out buff);
            buff3 = BitConverter.GetBytes((Int16)buff[0]);
            buff2[0] = buff3[0];
            buff2[1] = buff3[1];
            buff3 = BitConverter.GetBytes((Int16)buff[1]);
            buff2[2] = buff3[0];
            buff2[3] = buff3[1];
            int var = BitConverter.ToInt32(buff2, 0);
            PLC.Close();
            return Convert.ToDouble(var);
        }
        public static double GetValueR(string ip, short adr)
        {
            EtherNetPLC PLC = new EtherNetPLC();
            PLC.Link(ip, 9600, 1000);
            short buff;
            PLC.ReadWord(PlcMemory.DM, adr, out buff);
            PLC.Close();
            return Convert.ToDouble(buff);
        }
        public static void WriteStringPLC(string ip, string Value, int adr)
        {
            EtherNetPLC PLC = new EtherNetPLC();
            PLC.Link(ip, 9600, 1000);
            char[] buffful = new char[198];
            byte[] buffs = new byte[2];
            short[] buff = new short[100];
            buffful = Value.ToCharArray();
            for (int i = 0; i < (buffful.Length / 2); i++)
            {
                buffs[0] = (byte)buffful[i * 2 + 1];
                buffs[1] = (byte)buffful[i * 2];
                buff[i] = BitConverter.ToInt16(buffs, 0);
                if (buffful.Length % 2 > 0)
                {
                    buffs[0] = 0;
                    buffs[1] = (byte)buffful[i * 2 + 2];
                    buff[i + 1] = BitConverter.ToInt16(buffs, 0);
                }
            }
            PLC.WriteWords(PlcMemory.DM, (short)adr, 99, buff);
            PLC.Close();
        }
        public static string ReadStringPLC(string ip,int adr)
        {
            EtherNetPLC PLC = new EtherNetPLC();
            PLC.Link(ip, 9600, 1000);
            short[] buff = new short[99];
            char[] buffful = new char[198];
            byte[] buffs = new byte[2];
            PLC.ReadWords(PlcMemory.DM, (short)adr, 99, out buff);
            for (int i = 0; i < 99; i++)
            {

                buffs = BitConverter.GetBytes((int)buff[i]);
                buffful[i * 2] = (char)buffs[1];
                buffful[i * 2 + 1] = (char)buffs[0];

            }
            PLC.Close();
            string str = new string(buffful);
            char[] chr = str.ToCharArray();
            string str1 = "";
            foreach (var st in chr)
            {
                if ((int)st > 0)
                {
                    str1 += st;
                }
                if ((int)st == 0)
                    break;
            }
            return str1;

        }
        public static void WriteRealPLC(string ip,int adr, float value)
        {
            EtherNetPLC PLC = new EtherNetPLC();
            PLC.Link(ip, 9600, 1000);
            byte[] BuffB = BitConverter.GetBytes(value);
            short[] ValW = new short[2];
            ValW[0] = BitConverter.ToInt16(BuffB, 0);
            ValW[1] = BitConverter.ToInt16(BuffB, 2);
            PLC.WriteWords(PlcMemory.DM, (short)adr, 2, ValW);
            PLC.Close();
        }
        public static void WriteIntPLC(string ip, int adr, short value)
        {
            EtherNetPLC PLC = new EtherNetPLC();
            PLC.Link(ip, 9600, 1000);

            PLC.WriteWord(PlcMemory.DM, (short)adr, value);
            PLC.Close();
        }
        public static int ReadIntPLC(string ip,int adr)
        {
            EtherNetPLC PLC = new EtherNetPLC();
            PLC.Link(ip, 9600, 1000);
            short Varl;
            PLC.ReadWord(PlcMemory.DM, (short)adr, out Varl);
            PLC.Close();
            return (int)Varl;
        }
        public static float ReadRealPLC(string ip,int adr)
        {
            EtherNetPLC PLC = new EtherNetPLC();
            PLC.Link(ip, 9600, 1000);
            float Var;
            PLC.ReadReal(PlcMemory.DM, (short)adr, out Var);
            PLC.Close();
            return Var;
        }
        public static bool GetBitPLC(string ip, short adr, int numb)
        {
            EtherNetPLC PLC = new EtherNetPLC();
            PLC.Link(ip, 9600, 1000);
            short BuffW;
            PLC.ReadWord(PlcMemory.DM, adr, out BuffW);
            BitArray bitArr = new BitArray(BitConverter.GetBytes(BuffW));
            PLC.Close();
            return bitArr[numb];
        }
        public static void SetBitPLC(string ip, short adr, int numb, bool value)
        {
            EtherNetPLC PLC = new EtherNetPLC();
            PLC.Link(ip, 9600, 1000);
            if (value)
            {
                PLC.SetBitState(PlcMemory.DM, adr + "." + numb, BitState.ON);
            }
            else
            {
                PLC.SetBitState(PlcMemory.DM, adr + "." + numb, BitState.OFF);
            }
            PLC.Close();
        }  
    }
}
