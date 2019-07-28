using System;
using System.Diagnostics;
using System.Linq;

namespace monitor
{
    class Program
    {
        static void Main()
        {
            try
            {
                //Интервал должен быть меньше времени
                Console.WriteLine("Название, Время, Интервал:");
                //Переводим в массив строку
                string[] arr = Console.ReadLine().Split(',').Select(n => Convert.ToString(n)).ToArray();
                //Конвертируем элементы массива
                string name = arr[0];
                int time = Convert.ToInt32(arr[1]);
                int interval = Convert.ToInt32(arr[2]);
                //Создаём экземпляр класса с входными параметрами
                Run process = new Run(name, interval, time);
                //Запускаем метод закрытия выбранного приложения
                process.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Main();
            }
        }
    }

    class Run
    {
        //Объявляем поля для чтения
        readonly string process;
        readonly int minute;
        readonly int interval;

        //Создаём конструктор
        public Run(string process ,int minute, int interval)
        {
            this.process = process;            
            this.minute = minute;
            this.interval = interval;
        }

        //Создаём метод проверки на наличие процесса в системе
        public bool Check()
        {
            DateTime end = DateTime.Now;
            //Прибавляем интервал проверки к текущему времени
            end = end.AddMinutes(interval);

            while (true)
            {
                DateTime start = DateTime.Now;
                //Высчитываем разницу времени
                TimeSpan ch = end - start;

                //Если время вышло
                if (ch.Seconds == 0)
                {
                    //Ищем запущенный процесс в системе
                    var check = Process.GetProcessesByName(process).Any() ? true : false;
                    //И возвращаем результат
                    return check;
                }
                end = end.AddMinutes(interval);
            }
        }

        //Создаём метод закрытия процесса
        public void Close()
        {
            DateTime end = DateTime.Now;
            //Прибавляем время проверки к текущему времени
            end = end.AddMinutes(minute);

            while (true)
            {
                DateTime start = DateTime.Now;
                //Высчитываем разницу времени
                TimeSpan cl = end - start;

                //Если время вышло
                if (cl.Seconds == 0)
                {
                    //И процесс запущен
                    if (Check() == true)
                    {
                        {
                            foreach (var process in Process.GetProcessesByName(process))
                            {
                                //Находим и закрываем процесс
                                process.Kill();
                                Console.WriteLine("{0} был остановлен", process);
                            }
                            break;
                        }
                    }
                    //И процесс не запущен
                    else
                    {
                        Console.WriteLine("{0} не был найден", process);
                        break;
                    }
                }
                //Если процесс не запущен
                if (Check() == false)
                {
                    Console.WriteLine("{0} не был найден", process);
                    break;
                }
            }
            Console.WriteLine("Нажмите Enter для выхода");
            Console.ReadKey();
        }        
    }
}