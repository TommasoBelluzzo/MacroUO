#region Using Directives
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
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
        #region Members: Instance
        private ContainerControl m_ContainerControl;
        private IntPtr m_HookHandle;
        #endregion

        #region Members: Static
        private static NativeMethods.HookProcess s_HookProcessDelegate;
        #endregion

        #region Properties: Instance
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public ContainerControl ContainerControl
        {
            get { return m_ContainerControl; }
            set { m_ContainerControl = value; }
        }
        #endregion

        #region Properties: Overrides
        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;

                if (value == null)
                    return;

                IDesignerHost service = value.GetService(typeof(IDesignerHost)) as IDesignerHost;

                if (service == null)
                    return;

                m_ContainerControl = service.RootComponent as ContainerControl;
            }
        }
        #endregion

        #region Constructors
        private Messenger() { }

        public Messenger(IContainer container) : this()
        {
            if (container == null)
                throw new ArgumentNullException("container");

            container.Add(this);
        }
        #endregion

        #region Methods: Events
        private IntPtr HookProcessDialog(Int32 code, IntPtr wParameter, IntPtr lParameter)
        {
            if (code < 0)
                return NativeMethods.NextHook(m_HookHandle, code, wParameter, lParameter);

            IntPtr windowHandle;

            if (!NativeMethods.HookedInitialize(lParameter, out windowHandle))
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

            IntPtr windowHandle;

            if (!NativeMethods.HookedActivate(lParameter, out windowHandle))
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

        #region Methods: Overrides
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
                HookDisable();

            base.Dispose(disposing);
        }
        #endregion

        #region Methods: Instance
        private Boolean DisplayYesNoMessage(MessageBoxIcon icon, String text, MessageBoxDefaultButton defaultButton)
        {
            HookEnable(HookProcessMessageBox);
            return (MessageBox.Show(m_ContainerControl, text, GetIconText(icon), MessageBoxButtons.YesNo, icon, defaultButton, 0) == DialogResult.Yes);
        }

        private void CenterWindow(IntPtr childHandle)
        {
            Rectangle parentArea;

            if (!NativeMethods.GetRectangle(m_ContainerControl.Handle, out parentArea))
                return;

            Rectangle childArea;

            if (!NativeMethods.GetRectangle(childHandle, out childArea))
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
            MessageBox.Show(m_ContainerControl, text, GetIconText(icon), MessageBoxButtons.OK, icon, MessageBoxDefaultButton.Button1, 0);
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

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public Boolean DisplayDialog(CommonDialog dialog)
        {
            if (dialog == null)
                throw new ArgumentNullException("dialog");

            HookEnable(HookProcessDialog);

            DialogResult result = dialog.ShowDialog();
            return (result == DialogResult.OK);
        }

        public Boolean DisplayQuestionImportant(String text)
        {
            return DisplayYesNoMessage(MessageBoxIcon.Warning, text, MessageBoxDefaultButton.Button2);
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public Boolean DisplayQuestionStandard(String text)
        {
            return DisplayYesNoMessage(MessageBoxIcon.Question, text, MessageBoxDefaultButton.Button1);
        }

        public void DisplayError(String text)
        {
            DisplayOkMessage(MessageBoxIcon.Error, text);
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public void DisplayInformation(String text)
        {
            DisplayOkMessage(MessageBoxIcon.Information, text);
        }

        [SuppressMessage("ReSharper","UnusedMember.Global")]
        public void DisplayWarning(String text)
        {
            DisplayOkMessage(MessageBoxIcon.Warning, text);
        }
        #endregion

        #region Methods: Static
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