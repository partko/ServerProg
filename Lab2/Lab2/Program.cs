using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab2
{
    internal class Program
    {
        static Boolean alive = true;
        private const string UriPrefix = "http://127.0.0.1:8080/";


        static void Main(string[] args)
        {
            Console.WriteLine($"Main start in tread: {Thread.CurrentThread.ManagedThreadId}");
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(UriPrefix);
            listener.Start();


            var listenTask = ProcessAsync(listener);
            while (alive)
            {
                Console.WriteLine("Цикл alive в Main");
                var cmd = Console.ReadLine();
                if (cmd.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    alive = false;
                }
            }
            Console.WriteLine("Цикл alive в Main завершился");

            Console.WriteLine($"Main end in tread: {Thread.CurrentThread.ManagedThreadId}");
            listenTask.Wait(2000);
            //Console.WriteLine($"Main end in tread: {Thread.CurrentThread.ManagedThreadId}");
        }

        static async Task ProcessAsync(HttpListener listener)
        {
            Console.WriteLine($"ProcessAsync start in tread: {Thread.CurrentThread.ManagedThreadId}");
            while (alive)
            {
                Console.WriteLine($"ProcessAsync tread: {Thread.CurrentThread.ManagedThreadId}");
                HttpListenerContext context = await listener.GetContextAsync();

                if (alive) PerformAsync(context);
                else return;
            }
        }

        static async Task PerformAsync(HttpListenerContext context)
        {
            //string filePath = context.Request.Url.GetComponents(UriComponents.Path, UriFormat.UriEscaped);
            var filename = context.Request.Url.AbsolutePath;
            Console.WriteLine($"Запрос: {filename} от {context.Request.RemoteEndPoint.Address}");
            var filePath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, filename.TrimStart('/'));
            Console.WriteLine(filePath);

            Console.WriteLine(context.Request.Url);
            Console.WriteLine(context.Request.RemoteEndPoint);
            if (!File.Exists(filePath))
            {
                HttpListenerResponse response = context.Response;
                response.StatusCode = 404;
                Console.WriteLine(response.StatusCode);
                response.OutputStream.Close();
            }
            else
            {
                HttpListenerResponse response = context.Response;
                System.IO.Stream input = File.OpenRead(filePath);
                byte[] buffer = new byte[1024];
                int bytesRead = await input.ReadAsync(buffer, 0, buffer.Length);
                while (bytesRead > 0)
                {
                    //Console.WriteLine($"Перед WriteAsync: {Thread.CurrentThread.ManagedThreadId}");
                    await response.OutputStream.WriteAsync(buffer, 0, bytesRead);
                    //Console.WriteLine($"После WriteAsync: {Thread.CurrentThread.ManagedThreadId}");
                    //Console.WriteLine($"Перед readasynс: {Thread.CurrentThread.ManagedThreadId}");
                    bytesRead = await input.ReadAsync(buffer, 0, buffer.Length);
                    //Console.WriteLine($"После readasync: {Thread.CurrentThread.ManagedThreadId}");
                }
                response.StatusCode = 200;
                Console.WriteLine(response.StatusCode);
                input.Close();
                response.OutputStream.Close();
                //await Task.Delay(1000);
                Console.WriteLine($"Выполнено в потоке {Thread.CurrentThread.ManagedThreadId}");
            }
        }
    }
}
