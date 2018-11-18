using System;
using System.Net;
using System.Windows;


using System.Linq;
using System.Windows.Input;



using System.Collections.Generic;
using Windows.UI.Xaml.Data;


namespace SilverlightLFC.common
{
    public class LynxTime : AbstractLFCDataObject,IComparable//一个支持无限制的时间范围，可以涵盖数十亿年以上的单位
    {//关键是年支持了长整型，而且保持计时的精确
        //从使用的角度说，人只和时间分量打交到，内部的计算变量是封装的
        //未来可以进行扩展，自定义平润体系，但是年月日的结构不变
        //分为两段，一段是以年月日为核心，一段是时分秒为核心

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var lt=obj as LynxTime;
            if(lt==null) return false;
            if (getTotleDays() == lt.getTotleDays() && getTotleMS() == lt.getTotleMS()) return true;
            return base.Equals(obj);
        }
        #region Constructure

        public LynxTime()//新生成的对象计算绝对数值
        {
            InitLeapYearStructure();
            InitYearStructure();
            CalculateValue();
        }
        public LynxTime(string s)//从标准化格式的时间字符串获取时间
        {
            InitLeapYearStructure();
            InitYearStructure();
            bool IsBC = false;
            if (s.Contains("(BC)")||s.StartsWith("-"))
            {
                IsBC = true;
            }
            char[] cl = s.Trim().ToCharArray();

            for (int i = 0; i < cl.Length;i++ )
            {
                if (!char.IsDigit(cl[i]))
                {
                    cl[i] = ',';
                }
            }
            string ns=new string(cl);
            string[] sl = ns.Split(',');
            List<string> tsl = new List<string>();
            for (int i = 0; i < sl.Length; i++)
            {
                if (sl[i] == "")
                {

                }
                else
                {
                    tsl.Add(sl[i]);
                }
            }

            if (tsl.Count == 0) { return; }
            Year = Convert.ToInt64(tsl[0]);
            if (IsBC) {Year = -Year;}

            if (tsl.Count > 1)
            {
                Month = Convert.ToInt32(tsl[1]);
            }

            if (tsl.Count > 2)
            {
                Day = Convert.ToInt32(tsl[2]);
            }

            if (tsl.Count > 3)
            {
                Hour = Convert.ToInt32(tsl[3]);
            }

            if (tsl.Count > 4)
            {
                Minute = Convert.ToInt32(tsl[4]);
            }

            if (tsl.Count > 5)
            {
                Second = Convert.ToInt32(tsl[5]);
            }

            if (tsl.Count > 6)
            {
                Millisecond = Convert.ToInt32(tsl[6]);
            }

            CalculateValue();
        }
        public LynxTime(long y, int m, int d)
        {
            InitLeapYearStructure();
            InitYearStructure();
            _xt_db_lyear = y;
            _xt_db_lmonth = m;
            _xt_db_lday = d;
            CalculateValue();
            
        }
        public LynxTime(long y, int m, int d, int h, int min, int sec, int ms)
        {
            InitLeapYearStructure();
            InitYearStructure();
            _xt_db_lyear = y;
            _xt_db_lmonth = m;
            _xt_db_lday = d;
            _xt_db_lhour = h;
            _xt_db_lminute = min;
            _xt_db_lsecond = sec;
            _xt_db_lmillisecond = ms;
            CalculateValue();       
        }
        public LynxTime(DateTime dt)
        {
            InitLeapYearStructure();
            InitYearStructure();
            _xt_db_lyear = dt.Year;
            _xt_db_lmonth = dt.Month;
            _xt_db_lday = dt.Day;
            _xt_db_lhour = dt.Hour;
            _xt_db_lminute = dt.Minute;
            _xt_db_lsecond = dt.Second;
            _xt_db_lmillisecond = dt.Millisecond;
            CalculateValue();
        }
        public LynxTime(LynxTime st)//克隆时间
        {
            InitLeapYearStructure();
            InitYearStructure();
            CalculateTime(st.TotleDays, st.TotleMS);

        }
        public LynxTime(long y, long ms)//精确的直接赋值
        {
            InitLeapYearStructure();
            InitYearStructure();
            CalculateTime(y, ms);
        }
        public LynxTime(double d)
        {
            setTime(d);
        }

        #endregion
        #region TimeSpanProperty//从时间间隔的角度来看和使用该类

        public long SpanYear
        {
            get
            {
                if (Year > 0) return Year - 1;
                return Year;
            }
        }

        public int SpanMonth
        {
            get { return Month - 1; }
        }

        public int SpanDay
        {
            get { return Day - 1; }
        }

        public long SpanHour
        {
            get { return Hour; }
        }

        public long SpanMinute
        {
            get { return Minute; }
        }

        public long SpanSecond
        {
            get { return Second; }
        }

        public long SpanMS
        {
            get { return Millisecond; }
        }

        public TimeSpan LynxTimeSpan
        {
            get
            {
                double td = getDoubleValue();
                return TimeSpan.FromMilliseconds(td);
            }
            set
            {
                setTime(value.TotalMilliseconds);
            }
        }

        public string getSpanString()//按照时间间隔的格式获取字符串
        {
            string s="";
            if (SpanYear != 0) s = SpanYear.ToString()+"y";
            if (SpanMonth > 0) s = s + SpanMonth.ToString()+"m";
            if (SpanDay > 0) s = s + SpanDay.ToString() + "d";
            if (s.Length > 0) s = s + " ";
            s = s + SpanHour.ToString() + ":"+SpanMinute.ToString()+":"+SpanSecond.ToString();
            if (SpanMS > 0) s = s + " " + SpanMS.ToString() + "ms";
            return s;
        }

        

        #endregion
        #region Property
        public string _xt_db_Name = "公历时间";
        public string _xt_db_Memo = "Lynx Time";

        public object Tag { get; set; }//任意关联的对象
        public string Name
        {
            get
            {
                return _xt_db_Name;
            }
            set
            {
                if (_xt_db_Name != value)
                {
                    _xt_db_Name = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Name");
                }
            }
        }
        public string Memo
        {
            get
            {
                return _xt_db_Memo;
            }
            set
            {
                if (_xt_db_Memo != value)
                {
                    _xt_db_Memo = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Memo");
                }
            }
        }

        public long _xt_db_lyear = 1974;
        public int _xt_db_lmonth = 3;
        public int _xt_db_lday = 7;
        public int _xt_db_lhour = 0;
        public int _xt_db_lminute = 0;
        public int _xt_db_lsecond = 0;
        public int _xt_db_lmillisecond = 0;

        public int Millisecond
        {
            get { return _xt_db_lmillisecond; }
            set
            {
                if (_xt_db_lmillisecond != value)
                {
                    _xt_db_lmillisecond = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Millisecond");
                }
            }
        }
        public long Year
        {
            get { return _xt_db_lyear; }
            set
            {
                if (_xt_db_lyear != value)
                {
                    _xt_db_lyear = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Year");
                }
            }
        }
        public int Month
        {
            get { return _xt_db_lmonth; }
            set
            {
                if (_xt_db_lmonth != value)
                {
                    _xt_db_lmonth = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Month");
                }
            }
        }
        public int Day
        {
            get { return _xt_db_lday; }
            set
            {
                if (_xt_db_lday != value)
                {
                    _xt_db_lday = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Day");
                }
            }
        }
        public int Hour
        {
            get { return _xt_db_lhour; }
            set
            {
                if (_xt_db_lhour != value)
                {
                    _xt_db_lhour = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Hour");
                }
            }
        }
        public int Minute
        {
            get { return _xt_db_lminute; }
            set
            {
                if (_xt_db_lminute != value)
                {
                    _xt_db_lminute = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Minute");
                }
            }


        }
        public int Second
        {
            get { return _xt_db_lsecond; }
            set
            {
                if (_xt_db_lsecond != value)
                {
                    _xt_db_lsecond = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Second");
                }
            }
        }

        public string ShowString
        {
            get { return getTimeString(); }
        }

        #endregion
        #region TimeStructure//时间的结构
        public void setStructure(string name, List<int> Structure, List<int> LeapStructure, int day_hour)
        {
            Name = name;
            YearStructure = Structure;
            LeapYearStructure = LeapStructure;
            HourPerDay = day_hour;
        }
        List<int> _YearStructure;
        public List<int> YearStructure//平年时间结构
        {
            get
            {
                if (_YearStructure == null) { InitYearStructure(); }
                return _YearStructure;
            }
            set
            {
                _YearStructure = value;
            }
        }
        List<int> _LeapYearStructure;
        public List<int> LeapYearStructure//闰年时间结构
        {
            get
            {
                if (_LeapYearStructure == null) { InitLeapYearStructure(); }
                return _LeapYearStructure;
            }
            set
            {
                _LeapYearStructure = value;
            }
        }
        public void InitYearStructure()
        {
            _YearStructure=new List<int>();
            _YearStructure.Add(31);
            _YearStructure.Add(28);
            _YearStructure.Add(31);
            _YearStructure.Add(30);
            _YearStructure.Add(31);
            _YearStructure.Add(30);
            _YearStructure.Add(31);
            _YearStructure.Add(31);
            _YearStructure.Add(30);
            _YearStructure.Add(31);
            _YearStructure.Add(30);
            _YearStructure.Add(31);
        }
        public void InitLeapYearStructure()
        {
            _LeapYearStructure = new List<int>();
            _LeapYearStructure.Add(31);
            _LeapYearStructure.Add(29);
            _LeapYearStructure.Add(31);
            _LeapYearStructure.Add(30);
            _LeapYearStructure.Add(31);
            _LeapYearStructure.Add(30);
            _LeapYearStructure.Add(31);
            _LeapYearStructure.Add(31);
            _LeapYearStructure.Add(30);
            _LeapYearStructure.Add(31);
            _LeapYearStructure.Add(30);
            _LeapYearStructure.Add(31);
        }
        public long TrueSecondPerYear = 31556926;//精确的回归年，可以设定

        public int HourPerDay = 24;//一天有多少小时，可以设定
        public int MinutePerHour = 60;
        public int SecondPerMinute = 60;
        public int MsPerSecond = 1000;

        public int DaysPerWeek = 7;

        public long MSecondPerDay { get { return HourPerDay * MinutePerHour * SecondPerMinute * MsPerSecond; } }
        public double TrueDaysPerYear { get { return TrueSecondPerYear / Convert.ToDouble(HourPerDay * MinutePerHour * SecondPerMinute); } }

        //核心的计算值，属于内部封装的
        public long TotleDays = 0;//TimeSpan,距离1/1/1的距离，覆盖所有整数，和Mark比较，比Mark小1（当大于0时候，小于0时候相等）
        public long TotleMS = 0;
        #endregion
        public bool IsLeapYear()
        {
            return IsLeapYear(Year);

        }

        #region Core

        public void setTime(double d)
        {
            TotleDays = Convert.ToInt64(Math.Floor(d / (HourPerDay * MinutePerHour*SecondPerMinute * MsPerSecond)));
            TotleMS = Convert.ToInt64(d - TotleDays * HourPerDay * MinutePerHour * SecondPerMinute * MsPerSecond);
            CalculateTime();
        }

        public double getDoubleValue()
        {
            CalculateValue();
            return TotleDays * HourPerDay * MinutePerHour * SecondPerMinute * MsPerSecond + TotleMS;
        }

        public DateTime getDateTime()
        {
            if (!IsExtendTime())
            {
                return new DateTime(Convert.ToInt32(_xt_db_lyear), _xt_db_lmonth, _xt_db_lday, _xt_db_lhour, _xt_db_lminute, _xt_db_lsecond, _xt_db_lmillisecond);
            }
            else
            {
                if (_xt_db_lyear > 0)
                    return DateTime.MaxValue;
                else
                    return DateTime.MinValue;

            }
        }

        public void CalculateTime()
        {
            CalculateTime(TotleDays, TotleMS);
        }


        public void CalculateTime(long td, long tms)//用日期和毫秒数表示的时间
        {
            long md = tms / 86400000;
            td += md;
            tms = tms % 86400000;
            
            TotleDays = td;
            if (tms < 0) { tms = 86400000 + tms; TotleDays--; }
            TotleMS = tms;
            _xt_db_lyear = getYearByDays(TotleDays);
            long yearDays = getYearTotleDays(_xt_db_lyear);
            if (_xt_db_lyear < 0)
            {
                yearDays = -yearDays;
            }
            _xt_db_lmonth = getMonthByDays(TotleDays - yearDays);
            int x = getMonthTotleDays(_xt_db_lmonth - 1);

            _xt_db_lday = Convert.ToInt32(TotleDays - yearDays - x)+1;

            _xt_db_lhour = Convert.ToInt32(tms / (MsPerSecond * SecondPerMinute*MinutePerHour));
            _xt_db_lminute = Convert.ToInt32(tms % (MsPerSecond * SecondPerMinute * MinutePerHour) / (MsPerSecond * SecondPerMinute));
            _xt_db_lsecond = Convert.ToInt32((tms % (MsPerSecond * SecondPerMinute * MinutePerHour) % (MsPerSecond * SecondPerMinute)) / MsPerSecond);
            _xt_db_lmillisecond = Convert.ToInt32((tms % (MsPerSecond * SecondPerMinute * MinutePerHour) % (MsPerSecond * SecondPerMinute)) % MsPerSecond);
        }

        public void CalculateValue()
        {
            long yd = getYearTotleDays(_xt_db_lyear);
            if (_xt_db_lyear < 0) { yd = -yd; }
            TotleDays = yd + getMonthTotleDays(_xt_db_lmonth - 1) + _xt_db_lday;
            TotleDays--;
            TotleMS = _xt_db_lhour * (MsPerSecond * SecondPerMinute * MinutePerHour) + _xt_db_lminute * (MsPerSecond * SecondPerMinute) + _xt_db_lsecond * MsPerSecond + _xt_db_lmillisecond;
        }
        long getTotleDays()
        {
            return getYearTotleDays(_xt_db_lyear) + getMonthTotleDays(_xt_db_lmonth - 1) + _xt_db_lday;
        }
        long getTotleMS()
        {
            return _xt_db_lhour * (MsPerSecond * SecondPerMinute * MinutePerHour) + _xt_db_lminute * (MsPerSecond * SecondPerMinute) + _xt_db_lsecond * MsPerSecond + _xt_db_lmillisecond;
        }
        #endregion

        public bool IsExtendTime()//判断是不是除了DateTime的时间范围
        {
            if (Year < 1 || Year > 9999) return true;
            return false;
        }

        #region KeyTransfer//关键是整形和分量之间的转换，实际上，所有的处理和计算都是针对整形
        long getYearDistance()//获取时间原点的年数，没有0年，-1和1是挨着的
        {
            if (Year > 0) { return Year - 1; }
            return Year;
        }
        long getYearDistance(long y)//获取时间原点的年数，没有0年，-1和1是挨着的
        {//总是正数
            if (y > 0) { return y - 1; }
            return -y;
        }

        public void setTime(long y, int m, int d, int h, int min, int s, int ms)
        {
            _xt_db_lyear = y;
            _xt_db_lmonth = m;
            _xt_db_lday = d;
            _xt_db_lhour = h;
            _xt_db_lminute = min;
            _xt_db_lsecond = s;
            _xt_db_lmillisecond = ms;
            CalculateValue();
            //TrueMSValue = getValue();
        }
        public void setTime(DateTime dt)
        {
            _xt_db_lmonth = dt.Month;
            _xt_db_lday = dt.Day;
            _xt_db_lhour = dt.Hour;
            _xt_db_lminute = dt.Minute;
            _xt_db_lsecond = dt.Second;
            _xt_db_lmillisecond = dt.Millisecond;
            _xt_db_lyear = dt.Year;
            CalculateValue();
        }
        public void setTime(LynxTime dt)
        {
            _xt_db_lmonth = dt.Month;
            _xt_db_lday = dt.Day;
            _xt_db_lhour = dt.Hour;
            _xt_db_lminute = dt.Minute;
            _xt_db_lsecond = dt.Second;
            _xt_db_lmillisecond = dt.Millisecond;
            _xt_db_lyear = dt.Year;
            CalculateValue();
        }

        public bool IsFit()//判断绝对值和时间分量代表的数值是否一致
        {
            if (TotleDays == getTotleDays() && TotleMS == getTotleMS()) { return true; }
            return false;
        }

        #endregion

        public string getTimeString()//给出完整字符串
        {
            string s = "";
            if (_xt_db_lyear < 0) { s = Math.Abs(_xt_db_lyear).ToString() + "(BC)"; } else { s = Math.Abs(_xt_db_lyear).ToString(); }
            s = s + "/" + _xt_db_lmonth.ToString("D2");
            s = s + "/" + _xt_db_lday.ToString("D2");
            s = s + "-" + _xt_db_lhour.ToString("D2");
            s = s + ":" + _xt_db_lminute.ToString("D2");
            s = s + "'" + _xt_db_lsecond.ToString("D2") + "''";
            if (_xt_db_lmillisecond != 0)
            {
                s = s + _xt_db_lmillisecond.ToString("D3");
            }
            return s;
        }

        #region getTime//获得时间分量
        
        //public string getYear()
        //{
        //    string s = "";
        //    if (lyear < 0) { s = Math.Abs(lyear).ToString() + "(BC)"; } else { s = Math.Abs(lyear).ToString(); }
        //    return s;
        //}

        public string getQuarter()
        {
            if (_xt_db_lmonth < 4) { return "1st Quarter"; }
            if (_xt_db_lmonth < 7) { return "2nd Quarter"; }
            if (_xt_db_lmonth < 10) { return "3th Quarter"; }
            return "4th Quarter"; 
        }

        public string getWeekDay()
        {
            int x = Convert.ToInt32((TotleDays +1) % 7);

            if (x < 0) { x += 7; }
            if (x == 0) { return "Monday"; }
            if (x == 1) { return "Tuesday"; }
            if (x == 2) { return "Thursday"; }
            if (x == 3) { return "Wednesday"; }
            if (x == 4) { return "Friday"; }
            if (x == 5) { return "Saturday"; }
            if (x == 6) { return "Sunday"; }
            return "Error";
        }

        //public string getMonth()
        //{
        //    return lmonth.ToString();
        //}

        //public string getDay()
        //{
        //    return lday.ToString();
        //}

        //public string getHour()
        //{
        //    return lhour.ToString();
        //}

        //public string getMinute()
        //{
        //    return lminute.ToString();
        //}

        //public string getSecond()
        //{
        //    return lsecond.ToString();
        //}

        //public string getMS()
        //{
        //    return lms.ToString();
        //}


        #endregion

        #region Operation
        
        public void AddTimeSpan(TimeSpan tp)
        {
            long ds=Convert.ToInt64(tp.TotalMilliseconds)/86400000;
            long ms=Convert.ToInt64(tp.TotalMilliseconds)%86400000;
            CalculateTime(TotleDays + ds, TotleMS + ms);
        }

        public void SubTimeSpan(TimeSpan tp)
        {
            long ds = Convert.ToInt64(tp.TotalMilliseconds) / 86400000;
            long ms = Convert.ToInt64(tp.TotalMilliseconds) % 86400000;
            CalculateTime(TotleDays - ds, TotleMS - ms);
        }

        public void SubUnitDimention(TimeDimention td)
        {
            if (td == TimeDimention.BYear)
            {
                Year = Year - 1000000000000;
                CalculateValue();
            }
            if (td == TimeDimention.TYear)
            {
                Year = Year - 1000000000;
                CalculateValue();
            }
            if (td == TimeDimention.MYear)
            {
                Year = Year - 1000000;
                CalculateValue();
            }
            if (td == TimeDimention.KYear)
            {
                Year = Year - 1000;
                CalculateValue();
            }
            if (td == TimeDimention.Centory)
            {
                Year = Year - 100;
                CalculateValue();
            }
            if (td == TimeDimention.Age)
            {
                Year = Year - 10;
                CalculateValue();
            }
            if (td == TimeDimention.Year)
            {
                Year --;
                CalculateValue();
            }
            if (td == TimeDimention.Month)
            {

                if (Month == 1)
                {
                    Year--;
                    Month = YearStructure.Count;
                }
                else
                {
                    Month--;
                }
                CalculateValue();

            }
            if (td == TimeDimention.Week)
            {
                TotleDays=TotleDays-7;
                CalculateTime();
            }
            if (td == TimeDimention.Day)
            {
                TotleDays--;
                CalculateTime();
            }
            if (td == TimeDimention.Hour)
            {
                TotleMS -= (MinutePerHour*SecondPerMinute* MsPerSecond);
                CalculateTime();
            }
            if (td == TimeDimention.Minute)
            {
                TotleMS -= (SecondPerMinute * MsPerSecond);
                CalculateTime();
            }
            if (td == TimeDimention.Second)
            {
                TotleMS -= MsPerSecond;
                CalculateTime();
            }
            if (td == TimeDimention.MS)
            {
                TotleMS--;
                CalculateTime();
            }
        }

        public void AddUnitDimention(TimeDimention td)
        {
            if (td == TimeDimention.BYear)
            {
                Year = Year + 1000000000000;
                CalculateTime();
            }
            if (td == TimeDimention.TYear)
            {
                Year = Year + 1000000000;
                CalculateTime();
            }
            if (td == TimeDimention.MYear)
            {
                Year = Year + 1000000;
                CalculateTime();
            }
            if (td == TimeDimention.KYear)
            {
                Year = Year + 1000;
                CalculateTime();
            }
            if (td == TimeDimention.Centory)
            {
                Year = Year + 100;
                CalculateTime();
            }
            if (td == TimeDimention.Age)
            {
                Year = Year + 10;
                CalculateTime();
            }
            if (td == TimeDimention.Year)
            {
                Year = Year + 1;
                CalculateTime();
            }
            if (td == TimeDimention.Month)
            {

                if (Month == YearStructure.Count)
                {
                    Year++;
                    Month = 1;

                }
                else
                {
                    Month++;
                }
                CalculateTime();
            }
            if (td == TimeDimention.Week)
            {
                TotleDays = TotleDays+7;
                CalculateTime();
            }
            if (td == TimeDimention.Day)
            {
                TotleDays++;
                CalculateTime();
            }
            if (td == TimeDimention.Hour)
            {
                TotleMS += (MinutePerHour * SecondPerMinute * MsPerSecond);
                CalculateTime();
            }
            if (td == TimeDimention.Minute)
            {
                TotleMS += (SecondPerMinute * MsPerSecond);
                CalculateTime();
            }
            if (td == TimeDimention.Second)
            {
                TotleMS += MsPerSecond;
                CalculateTime();
            }
            if (td == TimeDimention.MS)
            {
                TotleMS++;
                CalculateTime();
            }
        }

        public void AddTimeSpan(long num, TimeDimention td)
        {
            if (num < 0)
            {
                SubTimeSpan(-num, td);
                return;
            }
            for (int i = 0; i < num; i++)
            {
                AddUnitDimention(td);
            }
        }

        public void SubTimeSpan(long num, TimeDimention td)
        {
            if (num < 0)
            {
                AddTimeSpan(-num, td);
                return;
            }
            for (int i = 0; i < num; i++)
            {
                SubUnitDimention(td);
            }
        }

        public LynxTime getNextTime(TimeDimention td)
        {
            //LynxTime lt = new LynxTime();
            if (td == TimeDimention.BYear)
            {
                long y = (_xt_db_lyear / 1000000000000) * 1000000000000 + 1000000000000;
                return new LynxTime(y, 1, 1, 0, 0, 0, 0);
            }
            if (td == TimeDimention.TYear)
            {
                long y = (_xt_db_lyear / 1000000000) * 1000000000 + 1000000000;
                return new LynxTime(y, 1, 1, 0, 0, 0, 0);
            }
            if (td == TimeDimention.MYear)
            {
                long y = (_xt_db_lyear / 1000000) * 1000000 + 1000000;
                return new LynxTime(y, 1, 1, 0, 0, 0, 0);
            }
            if (td == TimeDimention.KYear)
            {
                long y = (_xt_db_lyear / 1000) * 1000 + 1000;
                return new LynxTime(y, 1, 1, 0, 0, 0, 0);
            }
            if (td == TimeDimention.Centory)
            {
                long y = (_xt_db_lyear / 100) * 100 + 100;
                return new LynxTime(y, 1, 1, 0, 0, 0, 0);
            }
            if (td == TimeDimention.Age)
            {
                long y = (_xt_db_lyear / 10) * 10 + 10;
                return new LynxTime(y, 1, 1, 0, 0, 0, 0);
            }
            if (td == TimeDimention.Year)
            {
                return new LynxTime(_xt_db_lyear++, 1, 1, 0, 0, 0, 0);
            }
            if (td == TimeDimention.Month)
            {
                int m = _xt_db_lmonth + 1;
                if (m == 13)
                {
                    return new LynxTime(_xt_db_lyear++, 1, 1, 0, 0, 0, 0);
                }
                return new LynxTime(_xt_db_lyear, m, 1, 0, 0, 0, 0);
            }
            if (td == TimeDimention.Week)
            {
                LynxTime t = new LynxTime(this);
                if (TotleMS > 0)
                    t.TotleDays = (7-t.TotleDays % 7) + t.TotleDays;
                t.TotleMS = 0;
                t.CalculateTime();
                return t;
            }
            if (td == TimeDimention.Day)
            {
                LynxTime t=new LynxTime(this);
                if(TotleMS>0)
                    t.TotleDays++;
                t.TotleMS = 0;
                t.CalculateTime();
                return t;
            }
            if (td == TimeDimention.Hour)
            {
                LynxTime t = new LynxTime(this);
                if (TotleMS > _xt_db_lhour * (MinutePerHour * SecondPerMinute * MsPerSecond))
                    t._xt_db_lhour++;

                t._xt_db_lminute = 0;
                t._xt_db_lsecond = 0;
                t._xt_db_lmillisecond = 0;
                if (t._xt_db_lhour == HourPerDay) { t._xt_db_lhour = 0; t.TotleDays++; t.TotleMS = 0; }
                t.CalculateValue();
                t.CalculateTime();
                return t;
            }
            if (td == TimeDimention.Minute)
            {
                LynxTime t = new LynxTime(this);

                t._xt_db_lminute++;
                t._xt_db_lsecond = 0;
                t._xt_db_lmillisecond = 0;

                t.CalculateValue();
                t.CalculateTime();
                return t;
            }
            if (td == TimeDimention.Second)
            {
                LynxTime t = new LynxTime(this);

                t._xt_db_lsecond++;
                t._xt_db_lmillisecond = 0;

                t.CalculateValue();
                t.CalculateTime();
                return t;
            }
            if (td == TimeDimention.MS)
            {
                LynxTime t = new LynxTime(this);

                t._xt_db_lmillisecond++;

                t.CalculateValue();
                t.CalculateTime();
                return t;
            }
            return null;
        }

        public LynxTime getPastTime(TimeDimention td)
        {
            //LynxTime lt = new LynxTime();
            if (td == TimeDimention.BYear)
            {
                long y = (_xt_db_lyear / 1000000000000) * 1000000000000;
                return new LynxTime(y, 1, 1, 0, 0, 0, 0);
            }
            if (td == TimeDimention.TYear)
            {
                long y = (_xt_db_lyear / 1000000000) * 1000000000;
                return new LynxTime(y, 1, 1, 0, 0, 0, 0);
            }
            if (td == TimeDimention.MYear)
            {
                long y = (_xt_db_lyear / 1000000) * 1000000;
                return new LynxTime(y, 1, 1, 0, 0, 0, 0);
            }
            if (td == TimeDimention.KYear)
            {
                long y = (_xt_db_lyear / 1000) * 1000;
                return new LynxTime(y, 1, 1, 0, 0, 0, 0);
            }
            if (td == TimeDimention.Centory)
            {
                long y = (_xt_db_lyear / 100) * 100;
                return new LynxTime(y, 1, 1, 0, 0, 0, 0);
            }
            if (td == TimeDimention.Age)
            {
                long y = (_xt_db_lyear / 10) * 10;
                return new LynxTime(y, 1, 1, 0, 0, 0, 0);
            }
            if (td == TimeDimention.Year)
            {
                return new LynxTime(_xt_db_lyear, 1, 1, 0, 0, 0, 0);
            }
            if (td == TimeDimention.Month)
            {
                return new LynxTime(_xt_db_lyear, _xt_db_lmonth, 1, 0, 0, 0, 0);

            }
            if (td == TimeDimention.Week)
            {
                LynxTime t = new LynxTime(this);
                if (TotleMS > 0)
                    t.TotleDays =  t.TotleDays - (t.TotleDays % 7);
                t.TotleMS = 0;
                t.CalculateTime();
                return t;
            }
            if (td == TimeDimention.Day)
            {
                return new LynxTime(_xt_db_lyear, _xt_db_lmonth, _xt_db_lday, 0, 0, 0, 0);
            }
            if (td == TimeDimention.Hour)
            {
                return new LynxTime(_xt_db_lyear, _xt_db_lmonth, _xt_db_lday, _xt_db_lhour, 0, 0, 0);
            }
            if (td == TimeDimention.Minute)
            {
                return new LynxTime(_xt_db_lyear, _xt_db_lmonth, _xt_db_lday, _xt_db_lhour, _xt_db_lminute, 0, 0);
            }
            if (td == TimeDimention.Second)
            {
                return new LynxTime(_xt_db_lyear, _xt_db_lmonth, _xt_db_lday, _xt_db_lhour, _xt_db_lminute, _xt_db_lsecond, 0);
            }
            if (td == TimeDimention.MS)
            {
                return new LynxTime(_xt_db_lyear, _xt_db_lmonth, _xt_db_lday, _xt_db_lhour, _xt_db_lminute, _xt_db_lsecond, _xt_db_lmillisecond--);

            }
            return null;
        }
        #endregion

        public string getBottomFormatStr(TimeDimention Ceiling)
        {
            if (Ceiling == TimeDimention.MS) return "''" + _xt_db_lmillisecond.ToString();
            if (Ceiling == TimeDimention.Second) return _xt_db_lsecond.ToString() + "''" + _xt_db_lmillisecond.ToString();
            if (Ceiling == TimeDimention.Minute) return _xt_db_lhour.ToString() + ":" + _xt_db_lminute.ToString() + "'" + _xt_db_lsecond.ToString() + "''";

            if (Ceiling == TimeDimention.Hour) return _xt_db_lhour.ToString() + ":" + _xt_db_lminute.ToString() + "'";
            if (Ceiling == TimeDimention.Day) return _xt_db_lmonth.ToString() + "/" + _xt_db_lday.ToString() + "-" + _xt_db_lhour.ToString();
            if (Ceiling == TimeDimention.Month) return _xt_db_lmonth.ToString() + "/" + _xt_db_lday.ToString();
            if (Ceiling == TimeDimention.Year) return _xt_db_lyear.ToString() + "/" + _xt_db_lmonth.ToString() + "/" + _xt_db_lday.ToString();
            return _xt_db_lyear.ToString();
        }
        public string getTopFormatStr(TimeDimention Ceiling)
        {
            if (Ceiling == TimeDimention.MS) return _xt_db_lsecond.ToString() + "''" + _xt_db_lmillisecond.ToString();
            if (Ceiling == TimeDimention.Second) return _xt_db_lminute.ToString() + "'" + _xt_db_lsecond.ToString() + "''";
            if (Ceiling == TimeDimention.Minute) return _xt_db_lhour.ToString() + ":" + _xt_db_lminute.ToString() + "'";

            if (Ceiling == TimeDimention.Hour) return _xt_db_lday.ToString() + "-" + _xt_db_lhour.ToString();
            if (Ceiling == TimeDimention.Day) return _xt_db_lmonth.ToString() + "/" + _xt_db_lday.ToString();
            if (Ceiling == TimeDimention.Month) return _xt_db_lyear.ToString() + "/" + _xt_db_lmonth.ToString();
            //if (Ceiling == TimeDimention.Year) return lyear.ToString() + "/" + lmonth.ToString() + "/" + lday.ToString();
            return _xt_db_lyear.ToString();
        }
        public string getFallingFormatStr(TimeDimention Ceiling)//获取跨越整边界的时间的格式化字符串
        {
            if (Ceiling == TimeDimention.MS) return _xt_db_lmillisecond.ToString();
            if (Ceiling == TimeDimention.Second) return _xt_db_lyear.ToString() + "/" + _xt_db_lmonth.ToString() + "/" + _xt_db_lday.ToString() + "-" + _xt_db_lhour.ToString() + ":" + _xt_db_lminute.ToString() + "'" + _xt_db_lsecond.ToString() + "''";
            if (Ceiling == TimeDimention.Minute) return _xt_db_lyear.ToString() + "/" + _xt_db_lmonth.ToString() + "/" + _xt_db_lday.ToString() + "-" + _xt_db_lhour.ToString() + ":" + _xt_db_lminute.ToString() + "'";

            if (Ceiling == TimeDimention.Hour) return _xt_db_lyear.ToString() + "/" + _xt_db_lmonth.ToString() + "/" + _xt_db_lday.ToString() + "-" + _xt_db_lhour.ToString();
            if (Ceiling == TimeDimention.Day) return _xt_db_lyear.ToString() + "/" + _xt_db_lmonth.ToString() + "/" + _xt_db_lday.ToString();
            if (Ceiling == TimeDimention.Month) return _xt_db_lyear.ToString() + "/" + _xt_db_lmonth.ToString();
            return _xt_db_lyear.ToString();
        }

        public static List<LynxTime> getUnitList(LynxTime FromTime, LynxTime ToTime,int Num)
        {//把两个时间间隔进行等分，输出等分的时间序列
            List<LynxTime> tl = new List<LynxTime>();

            double step = (ToTime - FromTime).getDoubleValue() / Num;

            for (int i = 0; i < Num; i++)
            {
                LynxTime t = new LynxTime(FromTime.TotleDays, Convert.ToInt64(FromTime.TotleMS + (step * i)));
                //t.setTime();
                tl.Add(t);
            }
            tl.Add(ToTime);
            return tl;
        }

        public static List<LynxTime> getCoverUnitList(LynxTime FromTime, LynxTime ToTime)
        {
            TimeDimention td = getMaxCrossUnit(FromTime, ToTime);
            List<LynxTime> tl=new List<LynxTime>();
            LynxTime bt = FromTime.getPastTime(td);

            LynxTime step = bt;
            for (step = bt; step < ToTime; step = step + td)
            {
                tl.Add(step);
            }
            tl.Add(step);
            return tl;
        }

        public LynxTime getNearestLtnxTime(TimeDimention td)//获取距离某个时间最近的整点时间
        {
            LynxTime pt = getPastTime(td);
            LynxTime ft = getNextTime(td);
            double p = getDoubleValue() - pt.getDoubleValue();
            double f = ft.getDoubleValue() - getDoubleValue();
            if (p < f)
            {
                return pt;
            }
            return ft;
        }

        public static List<LynxTime> getCoverUnitList(LynxTime FromTime, LynxTime ToTime, int Num)//两个时间点之间，符合时间间隔的点的集合
        {
            TimeDimention td = getMaxCrossUnit(FromTime, ToTime);
            List<LynxTime> tl = new List<LynxTime>();

            double l = ToTime.getDoubleValue() - FromTime.getDoubleValue();
            l = l / Num;

            LynxTime bt = FromTime.getPastTime(td);

            for (int i = 0; i < Num; i++)
            {
                LynxTime t = new LynxTime();
                t.setTime(l*i+bt.getDoubleValue());
                t=t.getNearestLtnxTime(td);
                if (!IsContains(tl, t))
                {
                    tl.Add(t);
                }
            }
            LynxTime lt = ToTime.getNextTime(td);
            if (!IsContains(tl, lt))
            {
                tl.Add(lt);
            }

            return tl;
        }

        public static List<LynxTime> getCoverUnitList(LynxTime FromTime, LynxTime ToTime,TimeDimention CurrentDimention, int Num)//两个时间点之间，在某个时间级别上面，符合时间间隔的点的集合
        {
            List<LynxTime> tl = new List<LynxTime>();
            LynxTime bt = FromTime.getPastTime(CurrentDimention);
            long u = getUnitNum(FromTime, ToTime, CurrentDimention);
            long stepUnit = Convert.ToInt64(u / Convert.ToDouble(Num));
            for (int i = 0; i < Num; i++)
            {
                LynxTime t = new LynxTime(bt);
                t.AddTimeSpan(i * stepUnit, CurrentDimention);
                if (!IsContains(tl, t))
                {
                    tl.Add(t);
                }
            }
            LynxTime lt = ToTime.getNextTime(CurrentDimention);
            if (!IsContains(tl, lt))
            {
                tl.Add(lt);
            }

            return tl;
        }

        public static long getUnitNum(LynxTime bt, LynxTime et, TimeDimention td)//获得指定的单位下时间单元数
        {
            long d=0,m=0;
            if (td == TimeDimention.BYear)
            {
                d = et.TotleDays - bt.TotleDays;
                return Convert.ToInt64(d / bt.TrueDaysPerYear / 1000000000000);
            }
            if (td == TimeDimention.TYear)
            {
                d = et.TotleDays - bt.TotleDays;
                return Convert.ToInt64(d / bt.TrueDaysPerYear / 1000000000);
            }
            if (td == TimeDimention.MYear)
            {
                d = et.TotleDays - bt.TotleDays;
                return Convert.ToInt64(d / bt.TrueDaysPerYear / 1000000);
            }
            if (td == TimeDimention.KYear)
            {
                d = et.TotleDays - bt.TotleDays;
                return Convert.ToInt64(d / bt.TrueDaysPerYear / 1000);
            }
            if (td == TimeDimention.Centory)
            {
                d = et.TotleDays - bt.TotleDays;
                return Convert.ToInt64(d / bt.TrueDaysPerYear / 100);
            }
            if (td == TimeDimention.Age)
            {
                d = et.TotleDays - bt.TotleDays;
                return Convert.ToInt64(d / bt.TrueDaysPerYear / 10);
            }
            if (td == TimeDimention.Year)
            {
                d = et.TotleDays - bt.TotleDays;
                return Convert.ToInt64(d / bt.TrueDaysPerYear);
            }
            if (td == TimeDimention.Month)
            {
                d = et.TotleDays - bt.TotleDays;
                return d / bt.YearStructure[0];
            }
            if (td == TimeDimention.Day)
            {
                d = et.TotleDays - bt.TotleDays;
                return d;
            }
            if (td == TimeDimention.Hour)
            {
                d = et.TotleDays - bt.TotleDays;
                m = et.TotleMS - bt.TotleMS;

                return d * bt.HourPerDay + m / (bt.MinutePerHour * bt.SecondPerMinute * bt.MsPerSecond);
            }
            if (td == TimeDimention.Minute)
            {
                d = et.TotleDays - bt.TotleDays;
                m = et.TotleMS - bt.TotleMS;

                return d * bt.HourPerDay * bt.MinutePerHour + m / (bt.SecondPerMinute * bt.MsPerSecond);
            }
            if (td == TimeDimention.Second)
            {
                d = et.TotleDays - bt.TotleDays;
                m = et.TotleMS - bt.TotleMS;

                return d * bt.HourPerDay * (bt.MinutePerHour * bt.SecondPerMinute) + m / (bt.MsPerSecond);
            }
            if (td == TimeDimention.MS)
            {
                d = et.TotleDays - bt.TotleDays;
                m = et.TotleMS - bt.TotleMS;

                return d * bt.HourPerDay * (bt.MinutePerHour * bt.SecondPerMinute * bt.MsPerSecond) + m;
            }
            return Convert.ToInt32(d);
        }
        static bool IsContains(List<LynxTime> tl, LynxTime t)
        {
            foreach (LynxTime ct in tl)
            {
                if (ct == t) { return true; }
            }
            return false;
        }

        public static TimeDimention getMaxCrossUnit(LynxTime FromTime, LynxTime ToTime)//获得两个时间里面跨越的最大的单位
        {
            long dy = Math.Abs(FromTime.TotleDays - ToTime.TotleDays);
            if (dy / 1000000000000 >= FromTime.TrueDaysPerYear) { return TimeDimention.BYear; }
            if (dy / 1000000000 >= FromTime.TrueDaysPerYear) { return TimeDimention.TYear; }
            if (dy / 1000000 >= FromTime.TrueDaysPerYear) { return TimeDimention.MYear; }
            if (dy / 1000 >= FromTime.TrueDaysPerYear) { return TimeDimention.KYear; }
            if (dy / 100 >= FromTime.TrueDaysPerYear) { return TimeDimention.Centory; }
            if (dy / 10 >= FromTime.TrueDaysPerYear) { return TimeDimention.Age; }
            if (dy >= FromTime.TrueDaysPerYear) { return TimeDimention.Year; }
            if (dy >= FromTime.YearStructure[0]) { return TimeDimention.Month; }


            if (dy >= 1) { return TimeDimention.Day; }

            long ds = Math.Abs(FromTime.TotleMS - ToTime.TotleMS);
            if (ds > (FromTime.MinutePerHour * FromTime.SecondPerMinute * FromTime.MsPerSecond)) { return TimeDimention.Hour; }
            if (ds > (FromTime.SecondPerMinute * FromTime.MsPerSecond)) { return TimeDimention.Minute; }
            if (ds > FromTime.MsPerSecond) { return TimeDimention.Second; }

            return TimeDimention.MS; 
        }

        public static LynxTime AddTimeSpan(LynxTime source,int num, TimeDimention td)
        {
            LynxTime l = new LynxTime(source);
            l.AddTimeSpan(num, td);
            return l;
        }

        public static string getShortExpressStr(LynxTime lt)
        {
            if (lt.Millisecond != 0)
            {
                return lt.Year.ToString() + "/" + lt.Month.ToString() + "/" + lt.Day.ToString() + "-" + lt.Hour.ToString() + ":" + lt.Minute.ToString() + "'" + lt.Second.ToString() + "''" + lt.Millisecond.ToString();
            }
            if (lt.Second != 0)
            {
                return lt.Year.ToString() + "/" + lt.Month.ToString() + "/" + lt.Day.ToString() + "-" + lt.Hour.ToString() + ":" + lt.Minute.ToString() + "'" + lt.Second.ToString() + "''" ;
            }
            if (lt.Minute != 0)
            {
                return lt.Year.ToString() + "/" + lt.Month.ToString() + "/" + lt.Day.ToString() + "-" + lt.Hour.ToString() + ":" + lt.Minute.ToString() + "'";
            }
            if (lt.Hour != 0)
            {
                return lt.Year.ToString() + "/" + lt.Month.ToString() + "/" + lt.Day.ToString() + "-" + lt.Hour.ToString();
            }
            if (lt.Day != 1)
            {
                return lt.Year.ToString() + "/" + lt.Month.ToString() + "/" + lt.Day.ToString();
            }
            if (lt.Month != 1)
            {
                return lt.Year.ToString() + "/" + lt.Month.ToString() ;
            }
            return lt.Year.ToString();
        }

        public static LynxTime getUnitSpan(double l, TimeDimention td)//获得单位时间间隔
        {
            LynxTime lt = new LynxTime();
            if (td == TimeDimention.MS) 
            {
                lt.CalculateTime(0,1);
            }
            if (td == TimeDimention.Second)
            {
                lt.CalculateTime(0,lt.MsPerSecond);
            }
            if (td == TimeDimention.Minute)
            {
                lt.CalculateTime(0, (lt.SecondPerMinute * lt.MsPerSecond));
            }
            if (td == TimeDimention.Hour)
            {
                lt.CalculateTime(0, (lt.MinutePerHour*lt.SecondPerMinute*lt.MsPerSecond));
            }
            if (td == TimeDimention.Day)
            {
                lt.CalculateTime(1, 0);
            }
            if (td == TimeDimention.Week)
            {
                lt.CalculateTime(7, 0);
            }
            if (td == TimeDimention.Month)
            {
                lt.CalculateTime(30, 0);
            }
            if (td == TimeDimention.Year)
            {
                lt.CalculateTime(365, 0);
            }
            if (td == TimeDimention.Age)
            {
                lt.CalculateTime(3652, 0);
            }
            if (td == TimeDimention.Centory)
            {
                lt.CalculateTime(Convert.ToInt64(lt.TrueDaysPerYear * 100), 0);
            }
            if (td == TimeDimention.KYear)
            {
                lt.CalculateTime(Convert.ToInt64(lt.TrueDaysPerYear * 1000), 0);
            }
            if (td == TimeDimention.MYear)
            {
                lt.CalculateTime(Convert.ToInt64(lt.TrueDaysPerYear * 1000000), 0);
            }
            if (td == TimeDimention.TYear)
            {
                lt.CalculateTime(Convert.ToInt64(lt.TrueDaysPerYear * 1000000000), 0);
            }
            if (td == TimeDimention.BYear)
            {
                lt.CalculateTime(Convert.ToInt64(lt.TrueDaysPerYear * 1000000000000), 0);
            }
            return lt;
        }

        public static TimeDimention getLowTimeDim(TimeDimention td)
        {
            if (td == TimeDimention.MYear) { return TimeDimention.KYear; }
            if (td == TimeDimention.KYear) { return TimeDimention.Centory; }
            if (td == TimeDimention.Centory) { return TimeDimention.Age; }
            if (td == TimeDimention.Age) { return TimeDimention.Year; }
            if (td == TimeDimention.Year) { return TimeDimention.Month; }
            if (td == TimeDimention.Month) { return TimeDimention.Day; }
            if (td == TimeDimention.Day) { return TimeDimention.Hour; }
            if (td == TimeDimention.Hour) { return TimeDimention.Minute; }
            if (td == TimeDimention.Minute) { return TimeDimention.Second; }
            if (td == TimeDimention.Second) { return TimeDimention.MS; }
            return TimeDimention.TYear;
        }
        public static TimeDimention getHighTimeDim(TimeDimention td)
        {
            if (td == TimeDimention.KYear) { return TimeDimention.MYear; }
            if (td == TimeDimention.Centory) { return TimeDimention.KYear; }
            if (td == TimeDimention.Age) { return TimeDimention.Centory; }
            if (td == TimeDimention.Year) { return TimeDimention.Age; }
            if (td == TimeDimention.Month) { return TimeDimention.Year; }
            if (td == TimeDimention.Day) { return TimeDimention.Month; }
            if (td == TimeDimention.Hour) { return TimeDimention.Day; }
            if (td == TimeDimention.Minute) { return TimeDimention.Hour; }
            if (td == TimeDimention.Second) { return TimeDimention.Minute; }
            if (td == TimeDimention.MS) { return TimeDimention.Second; }
            return TimeDimention.TYear;
        }

        #region Operator Overload Methods
        public static bool operator ==(LynxTime l, LynxTime r)
        {
            if (!(l is object) && !(r is object)) { return true; }
            if (!(l is object) || !(r is object)) { return false; }
            //if (r == null) { return false; }
            l.CalculateValue();
            r.CalculateValue();
            return (l.TotleDays == r.TotleDays) && (l.TotleMS == r.TotleMS);

        }
        public static bool operator !=(LynxTime l, LynxTime r)
        {

            if (!(l is object) && !(r is object)) { return false; }
            if ((l is object) && !(r is object)) { return true; }
            if (!!(l is object) && (r is object)) { return true; }
            if (l == r) { return false; }
            return true;
        }
        public static bool operator >(LynxTime l, LynxTime r)
        {
            l.CalculateValue();
            r.CalculateValue();
            if (l.TotleDays > r.TotleDays) { return true; }
            else
            {
                if ((l.TotleDays == r.TotleDays) && (l.TotleMS > r.TotleMS)) { return true; }
            }
            return false;
        }
        public static bool operator >=(LynxTime l, LynxTime r)
        {
            l.CalculateValue();
            r.CalculateValue();
            if (l.TotleDays > r.TotleDays) { return true; }
            else
            {
                if ((l.TotleDays == r.TotleDays) && (l.TotleMS >= r.TotleMS)) { return true; }
            }
            return false;
        }
        public static bool operator <(LynxTime l, LynxTime r)
        {
            l.CalculateValue();
            r.CalculateValue();
            if (l.TotleDays < r.TotleDays) { return true; }
            else
            {
                if ((l.TotleDays == r.TotleDays) && (l.TotleMS < r.TotleMS)) { return true; }
            }
            return false;
        }
        public static bool operator <=(LynxTime l, LynxTime r)
        {
            l.CalculateValue();
            r.CalculateValue();
            if (l.TotleDays < r.TotleDays) { return true; }
            else
            {
                if ((l.TotleDays == r.TotleDays) && (l.TotleMS <= r.TotleMS)) { return true; }
            }
            return false;
        }
        public static LynxTime operator +(LynxTime l, LynxTime r)
        {
            l.CalculateValue();
            r.CalculateValue();
            long y = l.TotleDays + r.TotleDays;
            long ms = l.TotleMS + r.TotleMS;
            LynxTime dt = new LynxTime(y,ms);
            return dt;
        }
        public static LynxTime operator -(LynxTime l, LynxTime r)
        {
            l.CalculateValue();
            r.CalculateValue();
            long y = l.TotleDays - r.TotleDays;
            long ms = l.TotleMS - r.TotleMS;
            LynxTime dt = new LynxTime(y,ms);
           
            return dt;
        }
        public static LynxTime operator -(LynxTime l, long ms)
        {
            l.CalculateValue();
            LynxTime dt = new LynxTime(l);
            dt.TotleMS -= ms;
            dt.CalculateTime();
            return dt;
        }
        public static LynxTime operator +(LynxTime l, long ms)
        {
            l.CalculateValue();
            LynxTime dt = new LynxTime(l);
            dt.TotleMS += ms;
            dt.CalculateTime();
            return dt;
        }
        public static LynxTime operator -(LynxTime l, TimeDimention d)
        {
            return LynxTime.AddTimeSpan(l,-1,d);
        }
        public static LynxTime operator +(LynxTime l, TimeDimention d)
        {
            return LynxTime.AddTimeSpan(l, 1, d);
        }
        public static LynxTime operator /(LynxTime l, double d)
        {
            l.CalculateValue();
            LynxTime t = new LynxTime(l);
            long y = Convert.ToInt64(l.TotleDays/d);
            long ms = Convert.ToInt64(l.TotleMS/d);
            t.CalculateTime(y, ms);
            return t;
        }
        public static LynxTime operator *(LynxTime l, double d)
        {
            l.CalculateValue();
            LynxTime t = new LynxTime(l);
            long y = Convert.ToInt64(l.TotleDays * d);
            long ms = Convert.ToInt64(l.TotleMS * d);
            t.CalculateTime(y, ms);
            return t;
        }
        #endregion

        public long getLeapYearNum(long y)
        {
            long bx = 0;
            if (y < 0) { y = -y - 1; bx = 1; }
            long na = y / 4;
            long ns = y / 100;
            long na1 = y / 400;
            long ns1 = y / 3200;
            long na2 = y / 172800;
            
            
            long tl = na - ns + na1-ns1+na2+bx;

            return tl;

        }

        public long getYearTotleDays(long y)//获得某个年份的包含的所有日期
        {
            long by=y;
            if (y > 0) { by = y - 1; }
            long n = getLeapYearNum(by);
            
            return getYearDistance(y) * 365+n;
        }

        public long getYearTotleSecond(long TargetYear)
        {
            long TotleDays=0;
            if (TargetYear > 0)
            {
                for (long i = 1; i < TargetYear; i++)
                {
                    if (IsLeapYear(i))
                    {
                        TotleDays += 366;
                    }
                    else
                    {
                        TotleDays += 365;
                    }
                }
            }
            if (TargetYear < 0)
            {
                for (long i = TargetYear; i < 0; i++)
                {
                    if (IsLeapYear(i))
                    {
                        TotleDays += LeapYearStructure.Sum();
                    }
                    else
                    {
                        TotleDays += YearStructure.Sum();
                    }
                }
            }
            return TotleDays * HourPerDay * MinutePerHour * SecondPerMinute;
        }

        public long getYear(long Seconds)
        {
            if (Second > 0)
            {
                double b = Convert.ToDouble(Seconds) / TrueSecondPerYear;
                long by = Convert.ToInt64(Math.Floor(b));
                    long ys = getYearTotleSecond(by+1);
                    if (ys > Seconds) { return by; }
                    else { return by + 1; }

            }
            if (Second < 0)
            {
                double b = Convert.ToDouble(Seconds) / TrueSecondPerYear;
                long by = Convert.ToInt64(Math.Floor(b));
                long ys = getYearTotleSecond(by + 1);
                if (ys < Seconds) { return by; }
                else { return by - 1; }
            }
            return 0;
        }

        public long getMonthSecond(int TargetMonthIndex, bool IsLeapYear)
        {
            if (IsLeapYear)
            {
                return LeapYearStructure[TargetMonthIndex] * MinutePerHour * SecondPerMinute * HourPerDay;
            }
            return YearStructure[TargetMonthIndex] * MinutePerHour * SecondPerMinute * HourPerDay;

        }

        public long getTotleMonthSecond(int CurrentMonth, bool IsLeapYear)
        {
            if (CurrentMonth == 0) { return 0; }
            if (CurrentMonth < 0 || CurrentMonth > YearStructure.Count) { return -1; }//输入必须合法

            long totles = 0;
            for (int i = 0; i < CurrentMonth; i++)
            {
                totles += getMonthSecond(i, IsLeapYear);
            }
            return totles;

        }

        int getMonthTotleDays(int m)
        {
            if (m < 1 || m > YearStructure.Count) { return 0; }
            List<int> yearStructure;
            if (IsLeapYear())
            {
                yearStructure = LeapYearStructure;
            }
            else
            {
                yearStructure = YearStructure;
            }
            int x = 0;
            for (int i = 0; i < m; i++)
            {
                x += yearStructure[i];
            }
            return x;
        }

        int getMonthByDays(long ds)//仅仅是年内的日期
        {
            if(ds>0)
                ds = ds + 1;
            if (ds > YearStructure.Sum()&&!IsLeapYear())
            {
                return 0;
            }
            if (ds > LeapYearStructure.Sum() && IsLeapYear()) { return 0; }
            if (ds < 0) { return 0; }

            List<int> yearStructure;
            if (IsLeapYear())
            {
                yearStructure = LeapYearStructure;
            }
            else
            {
                yearStructure = YearStructure;
            }
            int x=0;
            for (int i = 0; i < YearStructure.Count; i++)
            {
                x += yearStructure[i];
                if (ds <= x) { return i+1; }
            }

            //x = yearStructure[0];
            //if (ds <= x) { return 1; }
            //x += yearStructure[1];
            //if (ds <= x) { return 2; }
            //x += yearStructure[2];
            //if (ds <= x) { return 3; }
            //x += yearStructure[3];
            //if (ds <= x) { return 4; }
            //x += yearStructure[4];
            //if (ds <= x) { return 5; }
            //x += yearStructure[5];
            //if (ds <= x) { return 6; }
            //x += yearStructure[6];
            //if (ds <= x) { return 7; }
            //x += yearStructure[7];
            //if (ds <= x) { return 8; }
            //x += yearStructure[8];
            //if (ds <= x) { return 9; }
            //x += yearStructure[9];
            //if (ds <= x) { return 10; }
            //x += yearStructure[10];
            //if (ds <= x) { return 11; }
            //x += yearStructure[11];
            //if (ds <= x) { return 12; }
            return 0;
        }

        long getYearByDays(long yds)//从日子到年份
        {
            if(yds>=0)
                yds = yds + 1;//转换为Mark状态
            long TrueYearNum = Convert.ToInt64(Math.Floor(yds / TrueDaysPerYear));

            long MinYearNum = TrueYearNum - 1;
            long MaxYearNum = TrueYearNum + 1;

            if (yds  > 0)
            {
                long MinYearDays = getYearTotleDays(MinYearNum + 1);
                long MaxYearDays = getYearTotleDays(MaxYearNum + 1);
                long TrueYearDays = getYearTotleDays(TrueYearNum + 1);

                if (yds <= TrueYearDays) { return MinYearNum + 1; }
                if (yds <= MaxYearDays) { return TrueYearNum + 1; }


            }
            else
            {
                long MinYearDays = getYearTotleDays(MinYearNum);
                long MaxYearDays = getYearTotleDays(MaxYearNum);
                long TrueYearDays = getYearTotleDays(TrueYearNum);

                if (yds  <= TrueYearDays) { return TrueYearNum; }
                if (yds  <= MaxYearDays) { return MaxYearNum; }
            }
            return 0;
        }

        public static bool IsLeapYear(long y)
        {
            if (y < 0)
            {
                if (Math.Abs(y % 100) == 0)
                {
                    if (Math.Abs(y % 172800) == 1) { return true; }
                    if (Math.Abs(y % 3200) == 1) { return false; }
                    if (Math.Abs(y % 400) == 1) { return true; }
                }
                else
                {
                    if (Math.Abs(y % 4) == 1) { return true; }
                }
            }
            if (y >= 0)
            {
                if ((y % 400 == 0) || (y % 100 != 0) && (y % 4 == 0)) return true;
            }
            return false;//超过范围不判断平润
        }

        public static List<int> getYearStructure(long y)//获得某年的日期结构
        {
            LynxTime lt = new LynxTime();
            if (IsLeapYear(y))
            {
                return lt.LeapYearStructure;
            }
            return lt.YearStructure;
        }

        public static int getMonthMaxDay(long y ,int m)//获取某个年月的最大天数
        {

            List<int> ys = getYearStructure(y);
            return ys[m - 1];
        }

        public static string getWeekDay(long y, int m, int d)
        {
            LynxTime lt = new LynxTime(y, m, d);
            return lt.getWeekDay();
        }

        public TimeSpan getTimeSpan()
        {
            if ((TotleDays > int.MinValue)&&(TotleDays < int.MaxValue))
            {
                TimeSpan ts = new TimeSpan(Convert.ToInt32(TotleDays), Hour, Minute, Second, Millisecond);
            }
            if (TotleDays > 0)
            {
                return TimeSpan.MaxValue;

            }
            return TimeSpan.MinValue;
        }

        public void LoadFromTimeSpan(TimeSpan TSpan)
        {
            TotleDays = TSpan.Days;
            TotleMS = Convert.ToInt64(TSpan.TotalMilliseconds - (TSpan.Days * HourPerDay * MinutePerHour * SecondPerMinute * MsPerSecond));
            CalculateTime();

        }

        public override string ToString()
        {
            return getTimeString();
        }

        #region ID
        public string _xt_db_ID = null;
        override public String ObjectID
        {
            get
            {
                return this._xt_db_ID;
            }
            set
            {
                _xt_db_ID = value;
                DataFlag = DataOperation.Update;
            }
        }
        override public void ClearObjectID()
        {
            this._xt_db_ID = "";
        }
        #endregion

        public int CompareTo(object obj)
        {
            LynxTime o = obj as LynxTime;
            if (TotleDays + TotleMS / DaysMS > o.TotleDays + o.TotleMS / DaysMS) { return 1; }
            else
            {
                if (TotleDays + TotleMS / DaysMS == o.TotleDays + o.TotleMS / DaysMS)
                {
                    if (TotleMS > o.TotleMS) { return 1; }
                    if (TotleMS == o.TotleMS) { return 0; }
                    return -1;
                }
                return -1;
            }
        }
        public long DaysMS { get { return MsPerSecond*SecondPerMinute*MinutePerHour * HourPerDay; } }

        static LynxTime _MaxValue;
        public static LynxTime MaxValue//最大值
        {
            get
            {
                if (_MaxValue == null)
                {
                    _MaxValue = new LynxTime(long.MaxValue, long.MaxValue);
                }
                return _MaxValue;
            }
        }
        static LynxTime _MinValue;
        public static LynxTime MinValue//最小值
        {
            get
            {
                if (_MinValue == null)
                {
                    _MinValue = new LynxTime(long.MinValue, long.MinValue);
                }
                return _MinValue;
            }
        }
    }

    public enum TimeDimention//时间分辨率刻度
    {
        MS, Second, Minute, Hour, Day, Week, Month, Year, Age, Centory, KYear, MYear, TYear, BYear
    }

    public class LynxTimeToDoubleValueConverter : IValueConverter//LynxTime和double的转换
    {

        public object Convert(object value, Type targetType, object parameter, String culture)
        {
            if (value is LynxTime)
            {
                return ((LynxTime)value).getDoubleValue();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, String culture)
        {
            if (value is double)
            {
                return new LynxTime(((double)value));
            }
            return null;
        }

    }
}
