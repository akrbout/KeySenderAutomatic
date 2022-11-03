using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace KeySender
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.Title = "AutoKeySender";
            int sTimeFirst = 100000;
            int sTimeSecond = 10000;
            Console.Write("Запуск...");

            Thread.Sleep(sTimeFirst);
            string lOperation = "";
            bool statError = false;
            while (statError == false)
            {
                try
                {
                    Console.WriteLine($"[{DateTime.Now}] Попытка прочитать конф. файл...");
                    using (StreamReader sr = new StreamReader("./oper.txt"))
                    {
                        lOperation = sr.ReadLine();
                        if (lOperation == null)
                        {
                            Console.WriteLine($"[{DateTime.Now}] Конф. файл пустой!");
                            throw new System.ArgumentNullException();
                            statError = false;
                        }
                        else
                        {
                            Console.WriteLine($"[{DateTime.Now}] Конф. строка успешно прочитана и записана");
                            statError = true;
                        }
                    }
                }
                catch (System.IO.FileNotFoundException e)
                {
                    Console.WriteLine($"[{DateTime.Now}] Ошибка: Файл не найден. Чтение файла произойдет повторно через 10 секунд.");
                    Thread.Sleep(sTimeSecond);
                }
                catch (System.ArgumentNullException e)
                {
                    Console.WriteLine($"[{DateTime.Now}] Ошибка: Пустая строка. Чтение файла произойдет повторно через 10 секунд.");
                    Thread.Sleep(sTimeSecond);
                }
            }

            Console.WriteLine($"[{DateTime.Now}] Запуск {lOperation}...");
            Clipboard.SetText(lOperation);
            KeyboardSend.KeyDown(Keys.LWin);
            KeyboardSend.KeyDown(Keys.R);
            KeyboardSend.KeyUp(Keys.LWin);
            KeyboardSend.KeyUp(Keys.R);

            //Вставка данных из буффера обмена
            Thread.Sleep(1000);
            KeyboardSend.KeyDown(Keys.ControlKey);
            KeyboardSend.KeyDown(Keys.V);
            KeyboardSend.KeyUp(Keys.ControlKey);
            KeyboardSend.KeyUp(Keys.V);
            KeyboardSend.KeyDown(Keys.Enter);
            KeyboardSend.KeyUp(Keys.Enter);

            //Прожатие горячих клавиш
            Console.WriteLine($"[{DateTime.Now}] Через {sTimeSecond} мс будет нажата комбинация {Keys.ControlKey} + {Keys.Y}");
            Thread.Sleep(sTimeSecond);
            KeyboardSend.KeyDown(Keys.ControlKey);
            KeyboardSend.KeyDown(Keys.Y);
            KeyboardSend.KeyUp(Keys.ControlKey);
            KeyboardSend.KeyUp(Keys.Y);
        }
    }
    static class KeyboardSend
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private const int KEYEVENTF_EXTENDEDKEY = 1;
        private const int KEYEVENTF_KEYUP = 2;

        public static void KeyDown(Keys vKey)
        {
            keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY, 0);
        }

        public static void KeyUp(Keys vKey)
        {
            keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
    }
}
