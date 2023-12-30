using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = DownloadPicture();
            changeImg(path);
            //Console.ReadLine();
            //File.Delete(path);
        }

        public static string DownloadPicture(int timeOut = 10000)
        {
            System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

            string picUrl = "https://bghuan.cn/api/bing.php";
            string savePath = Directory.GetCurrentDirectory() + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".jpg";
            string value = String.Empty;
            WebResponse response = null;
            Stream stream = null;
            try
            {
                WebClient mywebclient = new WebClient();
                mywebclient.DownloadFile(picUrl, savePath);
                value = savePath;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (stream != null) stream.Close();
                if (response != null) response.Close();
            }
            return value;
        }

        public class WinAPI
        {
            [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
            public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        }

        private static void changeImg(string bmpPath)
        {
            int nResult;
            if (File.Exists(bmpPath))
            {

                nResult = WinAPI.SystemParametersInfo(20, 1, bmpPath, 0x1 | 0x2); //更换壁纸
                if (nResult == 0)
                {
                    Console.WriteLine("err");
                }
                else
                {
                    RegistryKey hk = Registry.CurrentUser;
                    RegistryKey run = hk.CreateSubKey(@"Control Panel\Desktop\");
                    run.SetValue("Wallpaper", bmpPath);  //将新图片路径写入注册表
                    Console.WriteLine("success");
                }
            }
            else
            {
                Console.WriteLine("not exist");
            }
        }
    }
}