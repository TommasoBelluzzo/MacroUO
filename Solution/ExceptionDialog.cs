#region Using Directives
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using MacroUO.Properties;
#endregion

namespace MacroUO
{
    [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")]
    public sealed class ExceptionDialog : Form
    {
        #region Members
        private readonly Button m_ButtonCopy;
        private readonly Button m_ButtonDetails;
        private readonly Button m_ButtonProceed;
        private readonly Label m_LabelMessage;
        private readonly TextBox m_TextBoxDetails;
        #endregion

        #region Constructors
        public ExceptionDialog(Exception exception)
        {
            m_ButtonCopy = new Button();
            m_ButtonDetails = new Button();
            m_ButtonProceed = new Button();
            m_LabelMessage = new Label();
            m_TextBoxDetails = new TextBox();

            SuspendLayout();

            m_ButtonCopy.Font = new Font("Microsoft Sans Serif", 9.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_ButtonCopy.Location = new Point(86, 58);
            m_ButtonCopy.Name = "ButtonCopy";
            m_ButtonCopy.Size = new Size(88, 25);
            m_ButtonCopy.TabIndex = 3;
            m_ButtonCopy.Text = Resources.TextCopy;
            m_ButtonCopy.UseVisualStyleBackColor = true;
            m_ButtonCopy.Visible = false;
            m_ButtonCopy.Click += ButtonCopyClick;

            m_ButtonDetails.Font = new Font("Microsoft Sans Serif", 9.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_ButtonDetails.Location = new Point(185, 58);
            m_ButtonDetails.Name = "ButtonDetails";
            m_ButtonDetails.Size = new Size(88, 26);
            m_ButtonDetails.TabIndex = 2;
            m_ButtonDetails.Text = Resources.TextDetails;
            m_ButtonDetails.UseVisualStyleBackColor = true;
            m_ButtonDetails.Click += ButtonDetailsClick;

            m_ButtonProceed.Font = new Font("Microsoft Sans Serif", 9.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_ButtonProceed.Location = new Point(284, 58);
            m_ButtonProceed.Name = "ButtonProceed";
            m_ButtonProceed.Size = new Size(88, 26);
            m_ButtonProceed.TabIndex = 1;
            m_ButtonProceed.Text = Resources.TextProceed;
            m_ButtonProceed.UseVisualStyleBackColor = true;
            m_ButtonProceed.Click += ButtonProceedClick;

            m_LabelMessage.Font = new Font("Microsoft Sans Serif", 10.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_LabelMessage.Location = new Point(56, 10);
            m_LabelMessage.Name = "LabelMessage";
            m_LabelMessage.Size = new Size(330, 40);
            m_LabelMessage.Text = Resources.ErrorUnhandledException;

            m_TextBoxDetails.Font = new Font("Courier New", 9.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_TextBoxDetails.Location = new Point(6, 58);
            m_TextBoxDetails.Multiline = true;
            m_TextBoxDetails.Name = "TextBoxDetails";
            m_TextBoxDetails.ReadOnly = true;
            m_TextBoxDetails.ScrollBars = ScrollBars.Both;
            m_TextBoxDetails.Size = new Size(372, 180);
            m_TextBoxDetails.TabIndex = 0;
            m_TextBoxDetails.TabStop = false;
            m_TextBoxDetails.Text = GenerateReport(exception);
            m_TextBoxDetails.Visible = false;
            m_TextBoxDetails.WordWrap = false;
            m_TextBoxDetails.KeyDown += TextBoxKeyDown;

            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.None;
            CausesValidation = false;
            ClientSize = new Size(384, 96);
            ControlBox = false;
            Controls.Add(m_LabelMessage);
            Controls.Add(m_ButtonCopy);
            Controls.Add(m_ButtonDetails);
            Controls.Add(m_ButtonProceed);
            Controls.Add(m_TextBoxDetails);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "ExceptionDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = Resources.TextUnhandledException;

            ResumeLayout(true);
        }
        #endregion

        #region Events
        private void ButtonCopyClick(Object sender, EventArgs e)
        {
            Clipboard.SetText(m_TextBoxDetails.Text);
        }

        private void ButtonDetailsClick(Object sender, EventArgs e)
        {
            SuspendLayout();

            if (m_TextBoxDetails.Visible)
            {
                Height -= 191;

                m_ButtonCopy.Top -= 192;
                m_ButtonCopy.Visible = false;

                m_ButtonDetails.Text = Resources.TextDetails;
                m_ButtonDetails.Top -= 192;

                m_ButtonProceed.Top -= 192;

                m_TextBoxDetails.Visible = false;
            }
            else
            {
                Height += 191;

                m_ButtonCopy.Top += 192;
                m_ButtonCopy.Visible = true;

                m_ButtonDetails.Text = Resources.TextHide;
                m_ButtonDetails.Top += 192;

                m_ButtonProceed.Top += 192;

                m_TextBoxDetails.Visible = true;
            }

            ResumeLayout(true);
        }

        private void ButtonProceedClick(Object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawIcon(SystemIcons.Error, 13, 13);
        }

        private void TextBoxKeyDown(Object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.A) && e.Control)
                m_TextBoxDetails.SelectAll();
        }
        #endregion

        #region Methods
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new DialogResult ShowDialog()
        {
            return DialogResult.None;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new DialogResult ShowDialog(IWin32Window owner)
        {
            return DialogResult.None;
        }

        public void Prompt()
        {
            Form form = Program.Form;

            if ((form == null) || form.IsDisposed)
            {
                base.ShowDialog();
                return;
            }

            StartPosition = FormStartPosition.CenterParent;

            base.ShowDialog(form);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new void Show() { }
        #endregion

        #region Methods (Static)
        private static String GenerateReport(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("[ERROR - EXCEPTION]");
            sb.AppendLine(String.Concat("Type: ", exception.GetType()));
            sb.AppendLine(String.Concat("Message: ", exception.Message));

            if (exception.InnerException != null)
            {
                Exception ie = exception.InnerException;

                while (ie.InnerException != null)
                    ie = ie.InnerException;

                sb.AppendLine();
                sb.AppendLine("[ERROR - INNER EXCEPTION]");
                sb.AppendLine(String.Concat("Type: ", exception.InnerException.GetType()));
                sb.AppendLine(String.Concat("Message: ", exception.InnerException.Message));
            }

            sb.AppendLine();
            sb.AppendLine("[ERROR - STACK TRACE]");

            String[] tokens = exception.StackTrace.SplitAndTrim("\r\n");

            for (Int32 i = 0; i < tokens.Length; ++i)
                sb.AppendLine(tokens[i]);

            sb.AppendLine();
            sb.AppendLine("[INFORMATION - ENVIRONMENT]");
            sb.AppendLine(String.Concat("Culture: ", CultureInfo.CurrentCulture.Name));
            sb.AppendLine(String.Concat("Framework: ", Environment.Version));
            sb.AppendLine(String.Concat("Machine: ", Environment.MachineName));
            sb.AppendLine(String.Concat("OS: ", Environment.OSVersion, " ", (Environment.Is64BitOperatingSystem ? "64-Bit" : "32-Bit")));

            sb.AppendLine();
            sb.AppendLine("[INFORMATION - APPLICATION]");
            sb.AppendLine(String.Concat("64-Bit: ", (Environment.Is64BitProcess ? "Yes" : "No")));
            sb.AppendLine(String.Concat("Location: ", Program.Assembly.Location));
            sb.AppendLine(String.Concat("Name: ", Program.Assembly.FullName));
            sb.AppendLine(String.Concat("Working Set: ", Environment.WorkingSet));

            return sb.ToString().TrimEnd();
        }
        #endregion
    }
}
