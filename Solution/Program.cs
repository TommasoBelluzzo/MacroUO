#region Using Directives
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using MacroUO.Properties;
#endregion

namespace MacroUO
{
    public static class Program
    {
        #region Members
        private static Boolean s_ExceptionHandled;
        private static Mutex s_Mutex;
        private static Object s_Lock;
        #endregion

        #region Properties
        public static ApplicationDialog Form { get; private set; }

        public static Icon Icon { get; private set; }

        public static String MacrosFile { get; private set; }

        public static String Version { get; private set; }

        public static UInt32 MutexMessage { get; private set; }
        #endregion

        #region Entry Point
        [STAThread]
        public static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            Application.ThreadException += ThreadException;

            Assembly assembly = Assembly.GetExecutingAssembly();
            String assemblyGuid = ((GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value;
            String mutexName = String.Format(CultureInfo.InvariantCulture, @"Local\{{{0}}}", assemblyGuid);

            MutexMessage = NativeMethods.RegisterMessage(assemblyGuid);
            s_Mutex = new Mutex(true, mutexName, out Boolean mutexCreated);

            if (!mutexCreated)
            {
                NativeMethods.BroadcastMessage(MutexMessage);
                return;
            }
            
            s_Lock = new Object();

            Stream stream = assembly.GetManifestResourceStream("MacroUO.Properties.Application.ico");

            if (stream != null)
                Icon = new Icon(stream);

            MacrosFile = Path.Combine(Application.StartupPath, "Presets.xml");

            Version programVersion = assembly.GetName().Version;
            Version = String.Concat("v", programVersion.Major, ".", programVersion.Minor);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Form = new ApplicationDialog());

            s_Mutex.ReleaseMutex();
        }
        #endregion

        #region Events
        private static void ThreadException(Object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        private static void UnhandledException(Object sender, UnhandledExceptionEventArgs e)
        {
            HandleException((Exception)e.ExceptionObject);
        }
        #endregion

        #region Methods
        private static void HandleException(Exception e)
        {
            lock (s_Lock)
            {
                if (s_ExceptionHandled)
                    return;

                s_ExceptionHandled = true;

                if (e == null)
                    e = new Exception(Resources.ErrorUnhandledException);

                using (ExceptionDialog dialog = new ExceptionDialog(e))
                    dialog.Prompt();

                Application.Exit();
            }
        }
        #endregion
    }
}
