using nesne;

namespace nesne
{
    internal static class Program
    {

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string kullaniciAdi = "Betül";
            string avatarYolu = @"C:\Users\betul\OneDrive\Desktop\ödevler\nesne\nesne\Resources\pngwing.com (3).png";
            string yetki = "admin";
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new ekran(kullaniciAdi, avatarYolu, yetki));
        }
    }
} 