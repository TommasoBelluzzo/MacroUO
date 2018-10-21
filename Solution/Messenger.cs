#region Using Directives
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
#endregion

namespace MacroUO
{
    [ToolboxBitmap(typeof(LinkLabel))]
    [ToolboxItem(true)]
    public sealed class Messenger : Component
    {
        #region Members
        private IntPtr m_HookHandle;
        #endregion

        #region Members (Static)
        private static NativeMethods.HookProcess s_HookProcessDelegate;
        #endregion

        #region Properties
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public ContainerControl ContainerControl { get; set; }

        public override ISite Site
        {
            get => base.Site;
            set
            {
                base.Site = value;

                if (!(value?.GetService(typeof(IDesignerHost)) is IDesignerHost service))
                    return;

                ContainerControl = service.RootComponent as ContainerControl;
            }
        }
        #endregion

        #region Constructors
        private Messenger() { }

        public Messenger(IContainer container) : this()
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Add(this);
        }
        #endregion

        #region Events
        private IntPtr HookProcessDialog(Int32 code, IntPtr wParameter, IntPtr lParameter)
        {
            if (code < 0)
                return NativeMethods.NextHook(m_HookHandle, code, wParameter, lParameter);

            if (!NativeMethods.HookedInitialize(lParameter, out IntPtr windowHandle))
                return NativeMethods.NextHook(m_HookHandle, code, wParameter, lParameter);

            try
            {
                CenterWindow(windowHandle);
            }
            finally
            {
                HookDisable();
            }

            return NativeMethods.NextHook(m_HookHandle, code, wParameter, lParameter);
        }

        private IntPtr HookProcessMessageBox(Int32 code, IntPtr wParameter, IntPtr lParameter)
        {
            if (code < 0)
                return NativeMethods.NextHook(m_HookHandle, code, wParameter, lParameter);

            if (!NativeMethods.HookedActivate(lParameter, out IntPtr windowHandle))
                return NativeMethods.NextHook(m_HookHandle, code, wParameter, lParameter);

            try
            {
                CenterWindow(windowHandle);
            }
            finally
            {
                HookDisable();
            }

            return NativeMethods.NextHook(m_HookHandle, code, wParameter, lParameter);
        }
        #endregion

        #region Methods
        private Boolean DisplayYesNoMessage(MessageBoxIcon icon, String text, MessageBoxDefaultButton defaultButton)
        {
            HookEnable(HookProcessMessageBox);
            return (MessageBox.Show(ContainerControl, text, GetIconText(icon), MessageBoxButtons.YesNo, icon, defaultButton, 0) == DialogResult.Yes);
        }

        private void CenterWindow(IntPtr childHandle)
        {
            if (!NativeMethods.GetRectangle(ContainerControl.Handle, out Rectangle parentArea))
                return;

            if (!NativeMethods.GetRectangle(childHandle, out Rectangle childArea))
                return;

            Rectangle areaScreen = Screen.FromRectangle(parentArea).WorkingArea;

            Int32 widthDifference = parentArea.Width - childArea.Width;
            Int32 x = parentArea.X;

            if (widthDifference > 0)
                x += widthDifference / 2;
            else if (widthDifference < 0)
                x -= Math.Abs(widthDifference) / 2;

            Int32 heightDifference = parentArea.Height - childArea.Height;
            Int32 y = parentArea.Y;

            if (heightDifference > 0)
                y += heightDifference / 2;
            else if (heightDifference < 0)
                y -= Math.Abs(heightDifference) / 2;

            Rectangle areaCentered = new Rectangle(x, y, childArea.Width, childArea.Height);

            if (areaCentered.X < areaScreen.X)
                areaCentered.X = areaScreen.X;
            else if (areaCentered.Right > areaScreen.Right)
                areaCentered.X -= areaCentered.Right - areaScreen.Right;

            if (areaCentered.Y < areaScreen.Y)
                areaCentered.Y = areaScreen.Y;
            else if (areaCentered.Bottom > areaScreen.Bottom)
                areaCentered.Y -= areaCentered.Bottom - areaScreen.Bottom;

            Task.Factory.StartNew(() => NativeMethods.SetPosition(childHandle, areaCentered));
        }

        private void DisplayOkMessage(MessageBoxIcon icon, String text)
        {
            HookEnable(HookProcessMessageBox);
            MessageBox.Show(ContainerControl, text, GetIconText(icon), MessageBoxButtons.OK, icon, MessageBoxDefaultButton.Button1, 0);
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
                HookDisable();

            base.Dispose(disposing);
        }

        private void HookDisable()
        {
            if (m_HookHandle == IntPtr.Zero)
                return;

            NativeMethods.Unhook(m_HookHandle);

            m_HookHandle = IntPtr.Zero;
            s_HookProcessDelegate = null;
        }

        private void HookEnable(NativeMethods.HookProcess hookProcess)
        {
            if (s_HookProcessDelegate != null)
                throw new InvalidOperationException("The process cannot be hooked more than once.");

            s_HookProcessDelegate = hookProcess;
            m_HookHandle = NativeMethods.Hook(s_HookProcessDelegate);
        }

        public Boolean DisplayDialog(CommonDialog dialog)
        {
            if (dialog == null)
                throw new ArgumentNullException(nameof(dialog));

            HookEnable(HookProcessDialog);

            DialogResult result = dialog.ShowDialog();
            return (result == DialogResult.OK);
        }

        public Boolean DisplayQuestionImportant(String text)
        {
            return DisplayYesNoMessage(MessageBoxIcon.Warning, text, MessageBoxDefaultButton.Button2);
        }

        public Boolean DisplayQuestionStandard(String text)
        {
            return DisplayYesNoMessage(MessageBoxIcon.Question, text, MessageBoxDefaultButton.Button1);
        }

        public void DisplayError(String text)
        {
            DisplayOkMessage(MessageBoxIcon.Error, text);
        }

        public void DisplayInformation(String text)
        {
            DisplayOkMessage(MessageBoxIcon.Information, text);
        }

        public void DisplayWarning(String text)
        {
            DisplayOkMessage(MessageBoxIcon.Warning, text);
        }
        #endregion

        #region Methods (Static)
        private static String GetIconText(MessageBoxIcon icon)
        {
            switch (icon)
            {
                case MessageBoxIcon.Asterisk:
                    return "Information";

                case MessageBoxIcon.Error:
                    return "Error";

                case MessageBoxIcon.Exclamation:
                    return "Warning";

                case MessageBoxIcon.Question:
                    return "Question";

                default:
                    return "Message";
            }
        }
        #endregion
    }
}