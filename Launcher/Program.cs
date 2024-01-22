namespace Launcher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * This feature was removed due to lack of time
             */
            while(true)
            {
                if(System.Diagnostics.Process.GetProcessesByName("Survival").Length == 0)
                   System.Diagnostics.Process.Start("Survival.exe");
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
