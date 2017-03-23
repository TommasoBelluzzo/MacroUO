#region Using Directives
using System;
using System.Diagnostics.CodeAnalysis;
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
        #region Members: Static
        private static ApplicationDialog s_Form;
        private static Icon s_Icon;
        private static Mutex s_Mutex;
        private static String s_MacrosFile;
        private static String s_Version;
        private static UInt32 s_MutexMessage;
        #endregion

        #region Properties: Static
        public static ApplicationDialog Form
        {
            get { return s_Form; }
        }

        public static Icon Icon
        {
            get { return s_Icon; }
        }

        public static String MacrosFile
        {
            get { return s_MacrosFile; }
        }

        public static String Version
        {
            get { return s_Version; }
        }

        public static UInt32 MutexMessage
        {
            get { return s_MutexMessage; }
        }
        #endregion

        #region Methods: Entry Point
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        [STAThread]
        public static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            Application.ThreadException += ThreadException;

            Assembly assembly = Assembly.GetExecutingAssembly();
            String assemblyGuid = ((GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value;
            String mutexName = String.Format(CultureInfo.InvariantCulture, @"Local\{{{0}}}", assemblyGuid);

            Boolean mutexCreated;

            s_MutexMessage = NativeMethods.RegisterMessage(assemblyGuid);
            s_Mutex = new Mutex(true, mutexName, out mutexCreated);

            if (!mutexCreated)
            {
                NativeMethods.BroadcastMessage(s_MutexMessage);
                return;
            }

            Version programVersion = assembly.GetName().Version;

            s_Icon = new Icon(assembly.GetManifestResourceStream("MacroUO.Properties.Application.ico"));
            s_MacrosFile = Path.Combine(Application.StartupPath, "Macros.xml");
            s_Version = String.Concat("v", programVersion.Major, ".", programVersion.Minor);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(s_Form = new ApplicationDialog());

            s_Mutex.ReleaseMutex();
        }
        #endregion

        #region Methods: Events
        private static void ThreadException(Object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        private static void UnhandledException(Object sender, UnhandledExceptionEventArgs e)
        {
            HandleException((Exception)e.ExceptionObject);
        }
        #endregion

        #region Methods: Static
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        private static void HandleException(Exception e)
        {
            if (e == null)
                e = new ApplicationException(Resources.TextUnknownException);

            using (ExceptionDialog dialog = new ExceptionDialog(e))
                dialog.Prompt();

            Application.Exit();
        }
        #endregion
    }
}