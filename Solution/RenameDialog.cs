#region Using Directives
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MacroUO.Properties;
#endregion

namespace MacroUO
{
    [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")]
    public sealed class RenameDialog : Form
    {
        #region Members
        private readonly Button m_ButtonCancel;
        private readonly Button m_ButtonOk;
        private readonly Label m_LabelError;
        private readonly String[] m_ExistingNames;
        private readonly TextBox m_TextBox;
        #endregion

        #region Properties: Instance
        public String NewName { get; private set; }
        #endregion

        #region Constructors
        public RenameDialog(String presetName, String[] existingNames)
        {
            m_ButtonCancel = new Button();
            m_ButtonOk = new Button();
            m_LabelError = new Label();
            m_ExistingNames = existingNames;
            m_TextBox = new TextBox();

            SuspendLayout();

            m_ButtonCancel.DialogResult = DialogResult.Cancel;
            m_ButtonCancel.Location = new Point(137, 43);
            m_ButtonCancel.Name = "ButtonCancel";
            m_ButtonCancel.Size = new Size(75, 23);
            m_ButtonCancel.TabIndex = 3;
            m_ButtonCancel.Text = Resources.TextCancel;
            m_ButtonCancel.UseVisualStyleBackColor = true;

            m_ButtonOk.Location = new Point(24, 43);
            m_ButtonOk.Name = "ButtonOk";
            m_ButtonOk.Size = new Size(75, 23);
            m_ButtonOk.TabIndex = 2;
            m_ButtonOk.Text = Resources.TextOk;
            m_ButtonOk.UseVisualStyleBackColor = true;
            m_ButtonOk.Click += ButtonOkClick;

            m_LabelError.ForeColor = Color.DarkGreen;
            m_LabelError.Location = new Point(5, 25);
            m_LabelError.Name = "LabelError";
            m_LabelError.Size = new Size(226, 15);
            m_LabelError.TabIndex = 1;
            m_LabelError.Text = Resources.ValidationNameSuccess;
            m_LabelError.TextAlign = ContentAlignment.MiddleCenter;

            m_TextBox.Location = new Point(5, 5);
            m_TextBox.MaxLength = Macro.MAXIMUM_NAME_LENGTH;
            m_TextBox.Name = "TextBox";
            m_TextBox.Size = new Size(226, 20);
            m_TextBox.TabIndex = 0;
            m_TextBox.Text = presetName;
            m_TextBox.TextChanged += TextBoxTextChanged;

            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = m_ButtonCancel;
            CausesValidation = false;
            ClientSize = new Size(236, 70);
            Controls.Add(m_TextBox);
            Controls.Add(m_LabelError);
            Controls.Add(m_ButtonOk);
            Controls.Add(m_ButtonCancel);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RenameDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = Resources.TextRenamePreset;
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        #region Events
        private void ButtonOkClick(Object sender, EventArgs e)
        {
            NewName = m_TextBox.Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void TextBoxTextChanged(Object sender, EventArgs e)
        {
            String name = m_TextBox.Text;

            if ((name.Length == 0) || !Macro.ValidateName(name))
            {
                m_LabelError.ForeColor = Color.DarkRed;
                m_LabelError.Text = Resources.ValidationNameErrorInvalid;
                m_ButtonOk.Enabled = false;

                return;
            }

            if (m_ExistingNames.Any(x => x == name))
            {
                m_LabelError.ForeColor = Color.DarkRed;
                m_LabelError.Text = Resources.ValidationNameErrorDuplicate;
                m_ButtonOk.Enabled = false;

                return;
            }

            m_LabelError.ForeColor = Color.DarkGreen;
            m_LabelError.Text = Resources.ValidationNameSuccess;
            m_ButtonOk.Enabled = true;
        }
        #endregion

        #region Methods
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new DialogResult ShowDialog()
        {
            return DialogResult.None;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new void Show() { }

        protected override void OnLoad(EventArgs e)
        {
            ActiveControl = m_TextBox;
            m_TextBox.SelectAll();

            base.OnLoad(e);
        }
        #endregion
    }
}
