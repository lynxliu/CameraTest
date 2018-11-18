using System;
using System.Net;
using System.Windows;



using System.Windows.Input;

using System.Text;
using System.Net.NetworkInformation;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Controls;
using Windows.Networking.Sockets;
using System.Threading.Tasks;
using Windows.Web;
using System.Threading;

namespace SilverlightLynxControls
{
    public delegate void CurrentTimeCallBack(DateTime? currentTime);
    public class LynxTimerManager 
    {
        //进行时间管理
        CurrentTimeCallBack _callback;
        public DateTime CurrentUTCTime = DateTime.Now;//代表没有对表
        DateTime startDT;
        string[] whost = { "time-nw.nist.gov", "5time.nist.gov", "time-a.nist.gov", "time-b.nist.gov", "tick.mit.edu", "time.windows.com", "clock.sgi.com" };
        int CurrentHostIndex = 0;
        int port = 13;
        public void getInternetTime(CurrentTimeCallBack cb)
        {
            _callback = cb;
            //CurrentHostIndex = 0;
            //ConnectInternetTime();
            getRecentTime();
        }
        private StreamWebSocket streamWebSocket;

        public async void ConnectInternetTime()
        {
            if (streamWebSocket == null)
            {
                // 记录开始的时间   
                startDT = DateTime.UtcNow;

                streamWebSocket = new StreamWebSocket();

                // Don't disable the Nagle algorithm
                streamWebSocket.Control.NoDelay = false;
                streamWebSocket.Closed += Closed;
                readBuffer = new byte[1000000];
                // Now you can use the StreamWebSocket to call one of the
                // ConnectAsync methods.
                //IWebSocket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建Socket

                try
                {
                    await streamWebSocket.ConnectAsync(new Uri(whost[CurrentHostIndex] + ":" + port));
                    Task receiving = Task.Factory.StartNew(ReceiveData,
                     streamWebSocket.InputStream.AsStreamForRead(), TaskCreationOptions.LongRunning);


                }
                catch (Exception xe)
                {
                    SilverlightLFC.common.Environment ex = SilverlightLFC.common.Environment.getEnvironment();
                    ex.WriteFile("log.txt", xe.Message);

                }
                finally
                {
                }
            }
        }
        byte[] readBuffer;
        private async void ReceiveData(object state)
        {
            int bytesReceived = 0;
            try
            {
                Stream readStream = (Stream)state;

                while (true) // Until closed and ReadAsync fails.
                {
                    int read = await readStream.ReadAsync(readBuffer, 0, readBuffer.Length);
                    bytesReceived += read;

                    StringBuilder sb = new StringBuilder();
                    sb.Append(Encoding.UTF8.GetString(readBuffer, 0, readBuffer.Length));
                    string[] o = sb.ToString().Split(' '); // 打断字符串   
                    TimeSpan k = new TimeSpan();
                    k = (TimeSpan)(DateTime.UtcNow - startDT);// 得到开始到现在所消耗的时间   
                    CurrentUTCTime = Convert.ToDateTime(o[1] + " " + o[2]).Subtract(-k);// 减去中途消耗的时间   
                    if (_callback != null)
                    {
                        _callback(CurrentUTCTime);
                    }

                }
            }
            catch (ObjectDisposedException)
            {
                CurrentHostIndex++;
                if (CurrentHostIndex < whost.Length)
                {

                    ConnectInternetTime();
                }
                else
                {
                    _callback(null);
                }
            }
            catch (Exception ex)
            {
                WebErrorStatus status = WebSocketError.GetStatus(ex.GetBaseException().HResult);
                // Add your specific error-handling code here.
            }
        }


        private void Closed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
            // You can add code to log or display the code and reason
            // for the closure (stored in args.code and args.reason)

            // This is invoked on another thread so use Interlocked 
            // to avoid races with the Start/Stop/Reset methods.
            StreamWebSocket webSocket = Interlocked.Exchange(ref streamWebSocket, null);
            if (webSocket != null)
            {
                webSocket.Dispose();
            }
        }

        private async void getRecentTime()
        {
            var ex = SilverlightLFC.common.Environment.getEnvironment();
            var s=await ex.GetServiceResult("http://webservice.webxml.com.cn/WebServices/WeatherWebService.asmx/getWeatherbyCityName?theCityName=58367", "");
            XmlReader responseReader = XmlReader.Create(s);
            XElement user = XElement.Load(responseReader);
            string[] sl = user.ToString().Split('\n');
            string ts = sl[5];
            string ss = ts.Substring(ts.IndexOf('>') + 1, (ts.LastIndexOf("</") - ts.IndexOf('>') - 1));
            DateTime dt = Convert.ToDateTime(ss);
            if (_callback != null)
            {
                _callback(dt);
            }

        }
     }

    public struct SystemTime
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;

        /// <summary>   
        /// 从System.DateTime转换。   
        /// </summary>   
        /// <param name="time">System.DateTime类型的时间。</param>   
        public void FromDateTime(DateTime time)
        {
            wYear = (ushort)time.Year;
            wMonth = (ushort)time.Month;
            wDayOfWeek = (ushort)time.DayOfWeek;
            wDay = (ushort)time.Day;
            wHour = (ushort)time.Hour;
            wMinute = (ushort)time.Minute;
            wSecond = (ushort)time.Second;
            wMilliseconds = (ushort)time.Millisecond;
        }
        /// <summary>   
        /// 转换为System.DateTime类型。   
        /// </summary>   
        /// <returns></returns>   
        public DateTime ToDateTime()
        {
            return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds);
        }
        /// <summary>   
        /// 静态方法。转换为System.DateTime类型。   
        /// </summary>   
        /// <param name="time">SYSTEMTIME类型的时间。</param>   
        /// <returns></returns>   
        public static DateTime ToDateTime(SystemTime time)
        {
            return time.ToDateTime();
        }
    }
}
