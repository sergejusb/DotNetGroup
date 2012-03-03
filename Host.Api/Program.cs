namespace Host.Api
{
    using System;
    using System.Threading;

    public class Program
    {
        public static void Main(string[] args)
        {
            using (var server = new ApiServer())
            {
                Console.WriteLine("API is running on {0}", server.BaseUri);
                Thread.Sleep(-1);
            }
        }
    }
}
