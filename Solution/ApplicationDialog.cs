#region Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using MacroUO.Properties;
using Timer = System.Windows.Forms.Timer;
#endregion

namespace MacroUO
{
    [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")]
    public sealed class ApplicationDialog : Form
    {
        #region Members: Instance
        private Boolean m_Collapsed;
        private Boolean m_PresetsChanged;
        private Decimal m_CounterRuns;
        private Decimal m_CounterTime;
        private Int32 m_PresetsPreviousIndex;
        private MacroCollection m_Presets;
        private MacroRunning m_MacroCurrent;
        private MacroRunning m_MacroLast;
        private readonly Button m_ButtonAdd;
        private readonly Button m_ButtonReload;
        private readonly Button m_ButtonRemove;
        private readonly Button m_ButtonReset;
        private readonly Button m_ButtonResize;
        private readonly Button m_ButtonScan;
        private readonly Button m_ButtonStart;
        private readonly Button m_ButtonStop;
        private readonly CheckBox m_CheckBoxAlt;
        private readonly CheckBox m_CheckBoxCtrl;
        private readonly CheckBox m_CheckBoxShift;
        private readonly ComboBox m_ComboBoxClients;
        private readonly ComboBox m_ComboBoxKeys;
        private readonly ComboBox m_ComboBoxPresets;
        private readonly Container m_Components;
        private readonly GroupBox m_GroupBoxClients;
        private readonly GroupBox m_GroupBoxMacro;
        private readonly GroupBox m_GroupBoxPresets;
        private readonly IntPtr m_KeyboardLayout;
        private readonly Label m_LabelCounterRuns;
        private readonly Label m_LabelCounterTime;
        private readonly Label m_LabelDelay;
        private readonly Label m_LabelKey;
        private readonly Label m_LabelModifiers;
        private readonly Label m_LabelRuns;
        private readonly List<Client> m_Clients;
        private readonly MenuStrip m_MenuStrip;
        private readonly Messenger m_Messenger;
        private readonly NumberFormatInfo m_NumberFormatInfo;
        private readonly NumericUpDown m_NumericUpDownDelay;
        private readonly NumericUpDown m_NumericUpDownRuns;
        private readonly TableLayoutPanel m_TableLayoutPanelClients;
        private readonly TableLayoutPanel m_TableLayoutPanelContainer;
        private readonly TableLayoutPanel m_TableLayoutPanelMacro;
        private readonly TableLayoutPanel m_TableLayoutPanelModifiers;
        private readonly TableLayoutPanel m_TableLayoutPanelPresets;
        private readonly TextBox m_TextBoxCounterRuns;
        private readonly TextBox m_TextBoxCounterTime;
        private readonly Timer m_TimerClient;
        private readonly Timer m_TimerMacro;
        private readonly ToolStripMenuItem m_ToolStripMenuItemOptions;
        private readonly ToolStripMenuItem m_ToolStripMenuItemTopMost;
        private readonly ToolStripMenuItem m_ToolStripMenuItemTransparency;
        #endregion

        #region Members: Static
        private static readonly Key[] s_Keys = 
        {
            new Key(Keys.F1),
            new Key(Keys.F2),
            new Key(Keys.F3),
            new Key(Keys.F4),
            new Key(Keys.F5),
            new Key(Keys.F6),
            new Key(Keys.F7),
            new Key(Keys.F8),
            new Key(Keys.F9),
            new Key(Keys.F10),
            new Key(Keys.F11),
            new Key(Keys.F12),
            new Key(Keys.D0, "0"),
            new Key(Keys.D1, "1"),
            new Key(Keys.D2, "2"),
            new Key(Keys.D3, "3"),
            new Key(Keys.D4, "4"),
            new Key(Keys.D5, "5"),
            new Key(Keys.D6, "6"),
            new Key(Keys.D7, "7"),
            new Key(Keys.D8, "8"),
            new Key(Keys.D9, "9"),
            new Key(Keys.A),
            new Key(Keys.B),
            new Key(Keys.C),
            new Key(Keys.D),
            new Key(Keys.E),
            new Key(Keys.F),
            new Key(Keys.G),
            new Key(Keys.H),
            new Key(Keys.I),
            new Key(Keys.J),
            new Key(Keys.K),
            new Key(Keys.L),
            new Key(Keys.M),
            new Key(Keys.N),
            new Key(Keys.O),
            new Key(Keys.P),
            new Key(Keys.Q),
            new Key(Keys.R),
            new Key(Keys.S),
            new Key(Keys.T),
            new Key(Keys.U),
            new Key(Keys.V),
            new Key(Keys.W),
            new Key(Keys.X),
            new Key(Keys.Y),
            new Key(Keys.Z),
            new Key(Keys.Multiply, "NPAD *"),
            new Key(Keys.Divide, "NPAD /"),
            new Key(Keys.Add, "NPAD +"),
            new Key(Keys.Subtract, "NPAD -"),
            new Key(Keys.NumPad0, "NPAD 0"),
            new Key(Keys.NumPad1, "NPAD 1"),
            new Key(Keys.NumPad2, "NPAD 2"),
            new Key(Keys.NumPad3, "NPAD 3"),
            new Key(Keys.NumPad4, "NPAD 4"),
            new Key(Keys.NumPad5, "NPAD 5"),
            new Key(Keys.NumPad6, "NPAD 6"),
            new Key(Keys.NumPad7, "NPAD 7"),
            new Key(Keys.NumPad8, "NPAD 8"),
            new Key(Keys.NumPad9, "NPAD 9"),
            new Key(Keys.Back, "BACK"),
            new Key(Keys.Delete, "DELETE"),
            new Key(Keys.End, "END"),
            new Key(Keys.Enter, "ENTER"),
            new Key(Keys.Escape, "ESCAPE"),
            new Key(Keys.Home, "HOME"),
            new Key(Keys.Insert, "INSERT"),
            new Key(Keys.PageDown, "PAGE DOWN"),
            new Key(Keys.PageUp, "PAGE UP"),
            new Key(Keys.Pause, "PAUSE"),
            new Key(Keys.Space, "SPACE"),
            new Key(Keys.Tab, "TABULATOR")
        };
        #endregion

        #region Constructors
        [SuppressMessage("ReSharper", "FunctionComplexityOverflow")]
        [SuppressMessage("ReSharper", "LocalizableElement")]
        public ApplicationDialog()
        {
            m_PresetsPreviousIndex = -1;
            m_ButtonAdd = new Button();
            m_ButtonReload = new Button();
            m_ButtonRemove = new Button();
            m_ButtonReset = new Button();
            m_ButtonResize = new Button();
            m_ButtonScan = new Button();
            m_ButtonStart = new Button();
            m_ButtonStop = new Button();
            m_CheckBoxAlt = new CheckBox();
            m_CheckBoxCtrl = new CheckBox();
            m_CheckBoxShift = new CheckBox();
            m_ComboBoxClients = new ComboBox();
            m_ComboBoxKeys = new ComboBox();
            m_ComboBoxPresets = new ComboBox();
            m_Components = new Container();
            m_GroupBoxClients = new GroupBox();
            m_GroupBoxMacro = new GroupBox();
            m_GroupBoxPresets = new GroupBox();
            m_KeyboardLayout = NativeMethods.GetKeyboardLayout();
            m_LabelCounterRuns = new Label();
            m_LabelCounterTime = new Label();
            m_LabelDelay = new Label();
            m_LabelKey = new Label();
            m_LabelModifiers = new Label();
            m_LabelRuns = new Label();
            m_Clients = new List<Client>();
            m_MenuStrip = new MenuStrip();
            m_Messenger = new Messenger(m_Components);
            m_NumberFormatInfo = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            m_NumericUpDownDelay = new NumericUpDown();
            m_NumericUpDownRuns = new NumericUpDown();
            m_TableLayoutPanelClients = new TableLayoutPanel();
            m_TableLayoutPanelContainer = new TableLayoutPanel();
            m_TableLayoutPanelMacro = new TableLayoutPanel();
            m_TableLayoutPanelModifiers = new TableLayoutPanel();
            m_TableLayoutPanelPresets = new TableLayoutPanel();
            m_TextBoxCounterRuns = new TextBox();
            m_TextBoxCounterTime = new TextBox();
            m_TimerClient = new Timer(m_Components);
            m_TimerMacro = new Timer(m_Components);
            m_ToolStripMenuItemOptions = new ToolStripMenuItem();
            m_ToolStripMenuItemTopMost = new ToolStripMenuItem();
            m_ToolStripMenuItemTransparency = new ToolStripMenuItem();

            m_MenuStrip.SuspendLayout();
            m_TableLayoutPanelContainer.SuspendLayout();

            m_GroupBoxClients.SuspendLayout();
            m_TableLayoutPanelClients.SuspendLayout();

            m_GroupBoxMacro.SuspendLayout();
            m_TableLayoutPanelMacro.SuspendLayout();
            m_TableLayoutPanelModifiers.SuspendLayout();
            ((ISupportInitialize)m_NumericUpDownDelay).BeginInit();
            ((ISupportInitialize)m_NumericUpDownRuns).BeginInit();

            m_GroupBoxPresets.SuspendLayout();
            m_TableLayoutPanelPresets.SuspendLayout();

            SuspendLayout();

            m_ButtonAdd.Anchor = AnchorStyles.None;
            m_ButtonAdd.Location = new Point(234, 0);
            m_ButtonAdd.Margin = new Padding(4, 0, 0, 3);
            m_ButtonAdd.Name = "ButtonAdd";
            m_ButtonAdd.Size = new Size(75, 23);
            m_ButtonAdd.TabIndex = 1;
            m_ButtonAdd.Text = Resources.TextAdd;
            m_ButtonAdd.UseVisualStyleBackColor = true;
            m_ButtonAdd.Click += ButtonAddClick;

            m_ButtonReload.Anchor = AnchorStyles.None;
            m_ButtonReload.Location = new Point(390, 0);
            m_ButtonReload.Margin = new Padding(3, 0, 3, 3);
            m_ButtonReload.Name = "ButtonReload";
            m_ButtonReload.Size = new Size(75, 23);
            m_ButtonReload.TabIndex = 3;
            m_ButtonReload.Text = Resources.TextReload;
            m_ButtonReload.UseVisualStyleBackColor = true;
            m_ButtonReload.Click += ButtonReloadClick;

            m_ButtonRemove.Anchor = AnchorStyles.None;
            m_ButtonRemove.Location = new Point(312, 0);
            m_ButtonRemove.Margin = new Padding(3, 0, 0, 3);
            m_ButtonRemove.Name = "ButtonRemove";
            m_ButtonRemove.Size = new Size(75, 23);
            m_ButtonRemove.TabIndex = 2;
            m_ButtonRemove.Text = Resources.TextRemove;
            m_ButtonRemove.UseVisualStyleBackColor = true;
            m_ButtonRemove.Click += ButtonRemoveClick;

            m_ButtonReset.Anchor = AnchorStyles.None;
            m_ButtonReset.Location = new Point(390, 39);
            m_ButtonReset.Margin = new Padding(4, 0, 3, 0);
            m_ButtonReset.Name = "ButtonReset";
            m_ButtonReset.Size = new Size(75, 23);
            m_ButtonReset.TabIndex = 12;
            m_ButtonReset.Text = Resources.TextReset;
            m_ButtonReset.UseVisualStyleBackColor = true;
            m_ButtonReset.Click += ButtonResetClick;

            m_ButtonResize.Font = new Font("Microsoft Sans Serif", 10.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_ButtonResize.Location = new Point(390, 221);
            m_ButtonResize.Name = "ButtonResize";
            m_ButtonResize.Size = new Size(90, 30);
            m_ButtonResize.TabIndex = 4;
            m_ButtonResize.Text = Resources.TextCollapse;
            m_ButtonResize.UseVisualStyleBackColor = true;
            m_ButtonResize.Click += ButtonResizeClick;

            m_ButtonScan.Anchor = AnchorStyles.None;
            m_ButtonScan.Location = new Point(2, 0);
            m_ButtonScan.Margin = new Padding(2, 0, 0, 3);
            m_ButtonScan.Name = "ButtonScan";
            m_ButtonScan.Size = new Size(75, 23);
            m_ButtonScan.TabIndex = 0;
            m_ButtonScan.Text = Resources.TextScan;
            m_ButtonScan.UseVisualStyleBackColor = true;
            m_ButtonScan.Click += ButtonScanClick;

            m_ButtonStart.Font = new Font("Microsoft Sans Serif", 10.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_ButtonStart.Location = new Point(4, 221);
            m_ButtonStart.Name = "ButtonStart";
            m_ButtonStart.Size = new Size(90, 30);
            m_ButtonStart.TabIndex = 2;
            m_ButtonStart.Text = Resources.TextStart;
            m_ButtonStart.UseVisualStyleBackColor = true;
            m_ButtonStart.Click += ButtonStartClick;

            m_ButtonStop.Enabled = false;
            m_ButtonStop.Font = new Font("Microsoft Sans Serif", 10.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_ButtonStop.Location = new Point(97, 221);
            m_ButtonStop.Name = "ButtonStop";
            m_ButtonStop.Size = new Size(90, 30);
            m_ButtonStop.TabIndex = 3;
            m_ButtonStop.Text = Resources.TextStop;
            m_ButtonStop.UseVisualStyleBackColor = true;
            m_ButtonStop.Click += ButtonStopClick;

            m_CheckBoxAlt.Anchor = AnchorStyles.None;
            m_CheckBoxAlt.AutoSize = true;
            m_CheckBoxAlt.Location = new Point(0, 2);
            m_CheckBoxAlt.Margin = new Padding(0, 2, 3, 1);
            m_CheckBoxAlt.Name = "CheckBoxAlt";
            m_CheckBoxAlt.Size = new Size(46, 17);
            m_CheckBoxAlt.TabIndex = 0;
            m_CheckBoxAlt.Text = Resources.TextAlt;
            m_CheckBoxAlt.UseVisualStyleBackColor = true;
            m_CheckBoxAlt.CheckedChanged += CheckBoxAltCheckedChanged;

            m_CheckBoxCtrl.Anchor = AnchorStyles.None;
            m_CheckBoxCtrl.AutoSize = true;
            m_CheckBoxCtrl.Location = new Point(52, 2);
            m_CheckBoxCtrl.Margin = new Padding(3, 2, 3, 1);
            m_CheckBoxCtrl.Name = "CheckBoxCtrl";
            m_CheckBoxCtrl.Size = new Size(54, 17);
            m_CheckBoxCtrl.TabIndex = 1;
            m_CheckBoxCtrl.Text = Resources.TextCtrl;
            m_CheckBoxCtrl.UseVisualStyleBackColor = true;
            m_CheckBoxCtrl.CheckedChanged += CheckBoxCtrlCheckedChanged;
 
            m_CheckBoxShift.Anchor = AnchorStyles.None;
            m_CheckBoxShift.AutoSize = true;
            m_CheckBoxShift.Location = new Point(112, 2);
            m_CheckBoxShift.Margin = new Padding(3, 2, 0, 1);
            m_CheckBoxShift.Name = "CheckBoxShift";
            m_CheckBoxShift.Size = new Size(57, 17);
            m_CheckBoxShift.TabIndex = 2;
            m_CheckBoxShift.Text = Resources.TextShift;
            m_CheckBoxShift.UseVisualStyleBackColor = true;
            m_CheckBoxShift.CheckedChanged += CheckBoxShiftCheckedChanged;

            m_ComboBoxClients.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            m_ComboBoxClients.DropDownStyle = ComboBoxStyle.DropDownList;
            m_ComboBoxClients.FormattingEnabled = true;
            m_ComboBoxClients.Location = new Point(81, 1);
            m_ComboBoxClients.Margin = new Padding(4, 1, 4, 3);
            m_ComboBoxClients.Name = "ComboBoxClients";
            m_ComboBoxClients.Size = new Size(383, 21);
            m_ComboBoxClients.TabIndex = 1;

            m_ComboBoxKeys.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            m_ComboBoxKeys.DropDownStyle = ComboBoxStyle.DropDownList;
            m_ComboBoxKeys.FormattingEnabled = true;
            m_ComboBoxKeys.Location = new Point(53, 1);
            m_ComboBoxKeys.Margin = new Padding(0, 1, 0, 3);
            m_ComboBoxKeys.Name = "ComboBoxKeys";
            m_ComboBoxKeys.Size = new Size(169, 21);
            m_ComboBoxKeys.TabIndex = 1;
            m_ComboBoxKeys.SelectedIndexChanged += ComboBoxKeysSelectedIndexChanged;

            m_ComboBoxPresets.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            m_ComboBoxPresets.DropDownStyle = ComboBoxStyle.DropDownList;
            m_ComboBoxPresets.FormattingEnabled = true;
            m_ComboBoxPresets.Location = new Point(4, 1);
            m_ComboBoxPresets.Margin = new Padding(4, 1, 0, 3);
            m_ComboBoxPresets.Name = "ComboBoxPresets";
            m_ComboBoxPresets.Size = new Size(226, 21);
            m_ComboBoxPresets.TabIndex = 0;
            m_ComboBoxPresets.SelectedIndexChanged += ComboBoxPresetsSelectedIndexChanged;

            m_GroupBoxClients.AutoSize = true;
            m_GroupBoxClients.Controls.Add(m_TableLayoutPanelClients);
            m_GroupBoxClients.Dock = DockStyle.Fill;
            m_GroupBoxClients.Location = new Point(5, 3);
            m_GroupBoxClients.Margin = new Padding(5, 3, 5, 2);
            m_GroupBoxClients.Name = "GroupBoxClients";
            m_GroupBoxClients.Padding = new Padding(3, 2, 3, 3);
            m_GroupBoxClients.Size = new Size(474, 44);
            m_GroupBoxClients.TabIndex = 0;
            m_GroupBoxClients.TabStop = false;
            m_GroupBoxClients.Text = Resources.TextClients;

            m_GroupBoxMacro.AutoSize = true;
            m_GroupBoxMacro.Controls.Add(m_TableLayoutPanelMacro);
            m_GroupBoxMacro.Dock = DockStyle.Fill;
            m_GroupBoxMacro.Location = new Point(5, 50);
            m_GroupBoxMacro.Margin = new Padding(5, 1, 5, 2);
            m_GroupBoxMacro.Name = "GroupBoxMacro";
            m_GroupBoxMacro.Padding = new Padding(3, 2, 3, 3);
            m_GroupBoxMacro.Size = new Size(474, 95);
            m_GroupBoxMacro.TabIndex = 1;
            m_GroupBoxMacro.TabStop = false;
            m_GroupBoxMacro.Text = Resources.TextMacro;

            m_GroupBoxPresets.AutoSize = true;
            m_GroupBoxPresets.Controls.Add(m_TableLayoutPanelPresets);
            m_GroupBoxPresets.Dock = DockStyle.Fill;
            m_GroupBoxPresets.Location = new Point(5, 148);
            m_GroupBoxPresets.Margin = new Padding(5, 1, 5, 2);
            m_GroupBoxPresets.Name = "GroupBoxPresets";
            m_GroupBoxPresets.Padding = new Padding(3, 2, 3, 3);
            m_GroupBoxPresets.Size = new Size(474, 44);
            m_GroupBoxPresets.TabIndex = 2;
            m_GroupBoxPresets.TabStop = false;
            m_GroupBoxPresets.Text = Resources.TextPresetMacros;

            m_LabelCounterRuns.Anchor = AnchorStyles.Right;
            m_LabelCounterRuns.AutoSize = true;
            m_LabelCounterRuns.Location = new Point(236, 31);
            m_LabelCounterRuns.Margin = new Padding(2, 0, 0, 0);
            m_LabelCounterRuns.Name = "LabelCounterRuns";
            m_LabelCounterRuns.Size = new Size(62, 13);
            m_LabelCounterRuns.TabIndex = 8;
            m_LabelCounterRuns.Text = Resources.TextTotalRuns;

            m_LabelCounterTime.Anchor = AnchorStyles.Right;
            m_LabelCounterTime.AutoSize = true;
            m_LabelCounterTime.Location = new Point(224, 57);
            m_LabelCounterTime.Margin = new Padding(2, 0, 0, 0);
            m_LabelCounterTime.Name = "LabelCounterTime";
            m_LabelCounterTime.Size = new Size(74, 13);
            m_LabelCounterTime.TabIndex = 10;
            m_LabelCounterTime.Text = Resources.TextTimeElapsed;

            m_LabelDelay.Anchor = AnchorStyles.Right;
            m_LabelDelay.AutoSize = true;
            m_LabelDelay.Location = new Point(16, 31);
            m_LabelDelay.Margin = new Padding(0);
            m_LabelDelay.Name = "LabelDelay";
            m_LabelDelay.Size = new Size(37, 13);
            m_LabelDelay.TabIndex = 2;
            m_LabelDelay.Text = Resources.TextDelay;

            m_LabelKey.Anchor = AnchorStyles.Right;
            m_LabelKey.AutoSize = true;
            m_LabelKey.Location = new Point(25, 6);
            m_LabelKey.Margin = new Padding(0);
            m_LabelKey.Name = "LabelKey";
            m_LabelKey.Size = new Size(28, 13);
            m_LabelKey.TabIndex = 0;
            m_LabelKey.Text = Resources.TextKey;

            m_LabelModifiers.Anchor = AnchorStyles.Right;
            m_LabelModifiers.AutoSize = true;
            m_LabelModifiers.Location = new Point(1, 57);
            m_LabelModifiers.Margin = new Padding(1, 0, 0, 0);
            m_LabelModifiers.Name = "LabelModifiers";
            m_LabelModifiers.Size = new Size(52, 13);
            m_LabelModifiers.TabIndex = 4;
            m_LabelModifiers.Text = Resources.TextModifiers;

            m_LabelRuns.Anchor = AnchorStyles.Right;
            m_LabelRuns.AutoSize = true;
            m_LabelRuns.Location = new Point(263, 6);
            m_LabelRuns.Margin = new Padding(2, 0, 0, 0);
            m_LabelRuns.Name = "LabelRuns";
            m_LabelRuns.Size = new Size(35, 13);
            m_LabelRuns.TabIndex = 6;
            m_LabelRuns.Text = Resources.TextRuns;

            m_MenuStrip.Items.AddRange(new ToolStripItem[] { m_ToolStripMenuItemOptions });
            m_MenuStrip.Location = new Point(0, 0);
            m_MenuStrip.Name = "MenuStrip";
            m_MenuStrip.RenderMode = ToolStripRenderMode.System;
            m_MenuStrip.RightToLeft = RightToLeft.Yes;
            m_MenuStrip.Size = new Size(484, 24);
            m_MenuStrip.TabIndex = 0;
            m_MenuStrip.Text = "MenuStrip";

            m_Messenger.ContainerControl = this;

            m_NumberFormatInfo.NumberDecimalDigits = 0;
            m_NumberFormatInfo.NumberGroupSeparator = ".";

            m_NumericUpDownDelay.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            m_NumericUpDownDelay.Font = new Font("Microsoft Sans Serif", 8.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_NumericUpDownDelay.Increment = 50m;
            m_NumericUpDownDelay.Location = new Point(53, 27);
            m_NumericUpDownDelay.Margin = new Padding(0, 2, 0, 3);
            m_NumericUpDownDelay.Maximum = 60000m;
            m_NumericUpDownDelay.Minimum = 100m;
            m_NumericUpDownDelay.Name = "NumericUpDownDelay";
            m_NumericUpDownDelay.Size = new Size(169, 21);
            m_NumericUpDownDelay.TabIndex = 3;
            m_NumericUpDownDelay.Value = 100m;
            m_NumericUpDownDelay.ValueChanged += NumericUpDownDelayValueChanged;

            m_NumericUpDownRuns.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            m_NumericUpDownRuns.Location = new Point(298, 2);
            m_NumericUpDownRuns.Margin = new Padding(0, 2, 4, 3);
            m_NumericUpDownRuns.Maximum = 1000000m;
            m_NumericUpDownRuns.Name = "NumericUpDownRuns";
            m_NumericUpDownRuns.Size = new Size(166, 20);
            m_NumericUpDownRuns.TabIndex = 7;

            m_TableLayoutPanelClients.AutoSize = true;
            m_TableLayoutPanelClients.ColumnCount = 2;
            m_TableLayoutPanelClients.ColumnStyles.Add(new ColumnStyle());
            m_TableLayoutPanelClients.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
            m_TableLayoutPanelClients.Controls.Add(m_ButtonScan, 0, 0);
            m_TableLayoutPanelClients.Controls.Add(m_ComboBoxClients, 1, 0);
            m_TableLayoutPanelClients.Dock = DockStyle.Fill;
            m_TableLayoutPanelClients.Location = new Point(3, 15);
            m_TableLayoutPanelClients.Margin = new Padding(0);
            m_TableLayoutPanelClients.Name = "TableLayoutPanelClients";
            m_TableLayoutPanelClients.RowCount = 1;
            m_TableLayoutPanelClients.RowStyles.Add(new RowStyle());
            m_TableLayoutPanelClients.Size = new Size(468, 26);
            m_TableLayoutPanelClients.TabIndex = 0;

            m_TableLayoutPanelContainer.AutoSize = true;
            m_TableLayoutPanelContainer.ColumnCount = 1;
            m_TableLayoutPanelContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
            m_TableLayoutPanelContainer.Controls.Add(m_GroupBoxClients, 0, 0);
            m_TableLayoutPanelContainer.Controls.Add(m_GroupBoxMacro, 0, 1);
            m_TableLayoutPanelContainer.Controls.Add(m_GroupBoxPresets, 0, 2);
            m_TableLayoutPanelContainer.Dock = DockStyle.Top;
            m_TableLayoutPanelContainer.Location = new Point(0, 24);
            m_TableLayoutPanelContainer.Name = "TableLayoutPanelContainer";
            m_TableLayoutPanelContainer.RowCount = 3;
            m_TableLayoutPanelContainer.RowStyles.Add(new RowStyle());
            m_TableLayoutPanelContainer.RowStyles.Add(new RowStyle());
            m_TableLayoutPanelContainer.RowStyles.Add(new RowStyle());
            m_TableLayoutPanelContainer.Size = new Size(484, 194);
            m_TableLayoutPanelContainer.TabIndex = 1;

            m_TableLayoutPanelMacro.AutoSize = true;
            m_TableLayoutPanelMacro.ColumnCount = 5;
            m_TableLayoutPanelMacro.ColumnStyles.Add(new ColumnStyle());
            m_TableLayoutPanelMacro.ColumnStyles.Add(new ColumnStyle());
            m_TableLayoutPanelMacro.ColumnStyles.Add(new ColumnStyle());
            m_TableLayoutPanelMacro.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
            m_TableLayoutPanelMacro.ColumnStyles.Add(new ColumnStyle());
            m_TableLayoutPanelMacro.Controls.Add(m_LabelKey, 0, 0);
            m_TableLayoutPanelMacro.Controls.Add(m_ComboBoxKeys, 1, 0);
            m_TableLayoutPanelMacro.Controls.Add(m_LabelDelay, 0, 1);
            m_TableLayoutPanelMacro.Controls.Add(m_NumericUpDownDelay, 1, 1);
            m_TableLayoutPanelMacro.Controls.Add(m_LabelModifiers, 0, 2);
            m_TableLayoutPanelMacro.Controls.Add(m_TableLayoutPanelModifiers, 1, 2);
            m_TableLayoutPanelMacro.Controls.Add(m_LabelRuns, 2, 0);
            m_TableLayoutPanelMacro.Controls.Add(m_NumericUpDownRuns, 3, 0);
            m_TableLayoutPanelMacro.Controls.Add(m_LabelCounterRuns, 2, 1);
            m_TableLayoutPanelMacro.Controls.Add(m_TextBoxCounterRuns, 3, 1);
            m_TableLayoutPanelMacro.Controls.Add(m_LabelCounterTime, 2, 2);
            m_TableLayoutPanelMacro.Controls.Add(m_TextBoxCounterTime, 3, 2);
            m_TableLayoutPanelMacro.Controls.Add(m_ButtonReset, 4, 1);
            m_TableLayoutPanelMacro.Dock = DockStyle.Fill;
            m_TableLayoutPanelMacro.Location = new Point(3, 15);
            m_TableLayoutPanelMacro.Margin = new Padding(0);
            m_TableLayoutPanelMacro.Name = "TableLayoutPanelMacro";
            m_TableLayoutPanelMacro.RowCount = 3;
            m_TableLayoutPanelMacro.RowStyles.Add(new RowStyle());
            m_TableLayoutPanelMacro.RowStyles.Add(new RowStyle());
            m_TableLayoutPanelMacro.RowStyles.Add(new RowStyle());
            m_TableLayoutPanelMacro.SetColumnSpan(m_NumericUpDownRuns, 2);
            m_TableLayoutPanelMacro.SetRowSpan(m_ButtonReset, 2);
            m_TableLayoutPanelMacro.Size = new Size(468, 77);
            m_TableLayoutPanelMacro.TabIndex = 0;

            m_TableLayoutPanelModifiers.Anchor = AnchorStyles.None;
            m_TableLayoutPanelModifiers.AutoSize = true;
            m_TableLayoutPanelModifiers.ColumnCount = 3;
            m_TableLayoutPanelModifiers.ColumnStyles.Add(new ColumnStyle());
            m_TableLayoutPanelModifiers.ColumnStyles.Add(new ColumnStyle());
            m_TableLayoutPanelModifiers.ColumnStyles.Add(new ColumnStyle());
            m_TableLayoutPanelModifiers.Controls.Add(m_CheckBoxAlt, 0, 0);
            m_TableLayoutPanelModifiers.Controls.Add(m_CheckBoxCtrl, 1, 0);
            m_TableLayoutPanelModifiers.Controls.Add(m_CheckBoxShift, 2, 0);
            m_TableLayoutPanelModifiers.Location = new Point(53, 54);
            m_TableLayoutPanelModifiers.Margin = new Padding(0);
            m_TableLayoutPanelModifiers.Name = "TableLayoutPanelModifiers";
            m_TableLayoutPanelModifiers.RowCount = 1;
            m_TableLayoutPanelModifiers.RowStyles.Add(new RowStyle());
            m_TableLayoutPanelModifiers.Size = new Size(169, 20);
            m_TableLayoutPanelModifiers.TabIndex = 5;

            m_TableLayoutPanelPresets.AutoSize = true;
            m_TableLayoutPanelPresets.ColumnCount = 4;
            m_TableLayoutPanelPresets.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
            m_TableLayoutPanelPresets.ColumnStyles.Add(new ColumnStyle());
            m_TableLayoutPanelPresets.ColumnStyles.Add(new ColumnStyle());
            m_TableLayoutPanelPresets.ColumnStyles.Add(new ColumnStyle());
            m_TableLayoutPanelPresets.Controls.Add(m_ComboBoxPresets, 0, 0);
            m_TableLayoutPanelPresets.Controls.Add(m_ButtonAdd, 1, 0);
            m_TableLayoutPanelPresets.Controls.Add(m_ButtonRemove, 2, 0);
            m_TableLayoutPanelPresets.Controls.Add(m_ButtonReload, 3, 0);
            m_TableLayoutPanelPresets.Dock = DockStyle.Fill;
            m_TableLayoutPanelPresets.Location = new Point(3, 15);
            m_TableLayoutPanelPresets.Margin = new Padding(0);
            m_TableLayoutPanelPresets.Name = "TableLayoutPanelPresets";
            m_TableLayoutPanelPresets.RowCount = 1;
            m_TableLayoutPanelPresets.RowStyles.Add(new RowStyle());
            m_TableLayoutPanelPresets.Size = new Size(468, 26);
            m_TableLayoutPanelPresets.TabIndex = 0;

            m_TextBoxCounterRuns.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            m_TextBoxCounterRuns.Font = new Font("Microsoft Sans Serif", 8.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_TextBoxCounterRuns.Location = new Point(298, 27);
            m_TextBoxCounterRuns.Margin = new Padding(0, 2, 0, 3);
            m_TextBoxCounterRuns.Name = "TextBoxCounterRuns";
            m_TextBoxCounterRuns.ReadOnly = true;
            m_TextBoxCounterRuns.Size = new Size(88, 21);
            m_TextBoxCounterRuns.TabIndex = 9;
            m_TextBoxCounterRuns.Text = "0";

            m_TextBoxCounterTime.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            m_TextBoxCounterTime.Font = new Font("Microsoft Sans Serif", 8.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_TextBoxCounterTime.Location = new Point(298, 53);
            m_TextBoxCounterTime.Margin = new Padding(0, 2, 0, 4);
            m_TextBoxCounterTime.Name = "TextBoxCounterTime";
            m_TextBoxCounterTime.ReadOnly = true;
            m_TextBoxCounterTime.Size = new Size(88, 20);
            m_TextBoxCounterTime.TabIndex = 11;
            m_TextBoxCounterTime.Text = "0s";

            m_TimerClient.Interval = 10;
            m_TimerClient.Tick += TimerClientTick;

            m_TimerMacro.Tick += TimerMacroTick;

            m_ToolStripMenuItemOptions.DropDownItems.AddRange(new ToolStripItem[] { m_ToolStripMenuItemTopMost, m_ToolStripMenuItemTransparency });
            m_ToolStripMenuItemOptions.Name = "ToolStripMenuItemOptions";
            m_ToolStripMenuItemOptions.Size = new Size(61, 20);
            m_ToolStripMenuItemOptions.Text = Resources.TextOptions;

            m_ToolStripMenuItemTopMost.CheckOnClick = true;
            m_ToolStripMenuItemTopMost.Name = "ToolStripMenuItemTopMost";
            m_ToolStripMenuItemTopMost.Size = new Size(145, 22);
            m_ToolStripMenuItemTopMost.Text = Resources.TextTopMost;
            m_ToolStripMenuItemTopMost.CheckedChanged += ToolStripMenuItemTopMostCheckedChanged;

            m_ToolStripMenuItemTransparency.CheckOnClick = true;
            m_ToolStripMenuItemTransparency.Name = "ToolStripMenuItemTransparency";
            m_ToolStripMenuItemTransparency.Size = new Size(145, 22);
            m_ToolStripMenuItemTransparency.Text = Resources.TextTransparency;
            m_ToolStripMenuItemTransparency.CheckedChanged += ToolStripMenuItemTransparencyChanged;

            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.Disable;
            ClientSize = new Size(484, 255);
            Controls.Add(m_TableLayoutPanelContainer);
            Controls.Add(m_ButtonStart);
            Controls.Add(m_ButtonStop);
            Controls.Add(m_ButtonResize);
            Controls.Add(m_MenuStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = Program.Icon;
            MainMenuStrip = m_MenuStrip;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ApplicationDialog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = String.Concat(Resources.TextTitle, " ", Program.Version);

            m_MenuStrip.ResumeLayout(true);
            m_TableLayoutPanelContainer.ResumeLayout(true);

            m_GroupBoxClients.ResumeLayout(true);
            m_TableLayoutPanelClients.ResumeLayout(true);

            m_GroupBoxMacro.ResumeLayout(true);
            m_TableLayoutPanelMacro.ResumeLayout(true);
            m_TableLayoutPanelModifiers.ResumeLayout(true);
            ((ISupportInitialize)m_NumericUpDownDelay).EndInit();
            ((ISupportInitialize)m_NumericUpDownRuns).EndInit();

            m_GroupBoxPresets.ResumeLayout(true);
            m_TableLayoutPanelPresets.ResumeLayout(true);

            ResumeLayout(true);
        }
        #endregion

        #region Methods: Events
        [SuppressMessage("ReSharper", "AccessToModifiedClosure")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        private void ButtonAddClick(Object sender, EventArgs e)
        {
            ActiveControl = null;
            m_ButtonAdd.Enabled = false;

            String presetName = null;
            UInt32 presetNumber = 1;

            while (presetNumber < UInt32.MaxValue)
            {
                presetName = String.Concat("Preset ", presetNumber);

                if (m_Presets.Any(x => x.Name == presetName))
                    ++presetNumber;
                else
                    break;
            }

            m_Presets.Add(new Macro
            {
                ModifierAlt = m_CheckBoxAlt.Checked,
                ModifierCtrl = m_CheckBoxCtrl.Checked,
                ModifierShift = m_CheckBoxShift.Checked,
                Key = m_ComboBoxKeys.SelectedItem.ToString(),
                Name = presetName,
                Delay = (UInt32)m_NumericUpDownDelay.Value
            });
            m_PresetsChanged = true;

            m_ComboBoxPresets.BeginUpdate();

            if (m_Presets.Count == 1)
            {
                m_ComboBoxPresets.Items.Clear();
                m_ComboBoxPresets.Items.AddRange(new Object[] { Resources.TextPresetsUnselected, presetName });
            }
            else
                m_ComboBoxPresets.Items.Add(presetName);

            m_ComboBoxPresets.SelectedIndex = m_ComboBoxPresets.Items.Count - 1;
            m_ComboBoxPresets.EndUpdate();

            ActiveControl = m_ButtonRemove;
        }

        private void ButtonReloadClick(Object sender, EventArgs e)
        {
            if (!m_PresetsChanged || m_Messenger.DisplayQuestionImportant(Resources.QuestionPresetsModified))
                PresetsLoad();
        }

        private void ButtonRemoveClick(Object sender, EventArgs e)
        {
            ActiveControl = null;
            m_ButtonRemove.Enabled = false;

            Int32 selectedIndex = m_ComboBoxPresets.SelectedIndex;
            Int32 selectedIndexMinus = selectedIndex - 1;

            m_Presets.RemoveAt(selectedIndexMinus);
            m_PresetsChanged = true;

            if (m_Presets.Count == 0)
            {
                m_ComboBoxPresets.BeginUpdate();
                m_ComboBoxPresets.Items.Clear();
                m_ComboBoxPresets.Items.Add(Resources.TextPresetsEmpty);
                m_ComboBoxPresets.SelectedIndex = 0;
                m_ComboBoxPresets.EndUpdate();

                m_ComboBoxPresets.Enabled = false;

                m_ButtonAdd.Enabled = true;
                ActiveControl = m_ButtonAdd;
            }
            else
            {
                m_ComboBoxPresets.BeginUpdate();
                m_ComboBoxPresets.Items.RemoveAt(selectedIndex);
                m_ComboBoxPresets.SelectedIndex = (selectedIndexMinus == 0) ? selectedIndex : selectedIndexMinus;
                m_ComboBoxPresets.EndUpdate();

                m_ButtonRemove.Enabled = true;
                ActiveControl = m_ButtonRemove;
            }
        }

        private void ButtonResetClick(Object sender, EventArgs e)
        {
            CountersReset();
        }

        private void ButtonResizeClick(Object sender, EventArgs e)
        {
            m_ButtonResize.Enabled = false;

            SuspendLayout();

            if (m_Collapsed)
            {
                Size size = ClientSize;
                ClientSize = new Size(484, 255);

                Int32 x = Location.X - (ClientSize.Width - size.Width);
                Int32 y = Location.Y - (ClientSize.Height - size.Height);
                SetLocation(x, y);

                m_ButtonStart.Location = new Point(4, 221);
                m_ButtonStop.Location = new Point(97, 221);

                m_ButtonResize.Location = new Point(390, 221);
                m_ButtonResize.Text = Resources.TextCollapse;

                m_TableLayoutPanelContainer.Visible = true;

                m_Collapsed = false;
            }
            else
            {
                m_TableLayoutPanelContainer.Visible = false;

                m_ButtonStart.Location = new Point(4, 28);
                m_ButtonStop.Location = new Point(97, 28);

                m_ButtonResize.Location = new Point(195, 28);
                m_ButtonResize.Text = Resources.TextExpand;

                Size size = ClientSize;
                ClientSize = new Size(289, 62);

                Int32 x = Location.X + (size.Width - ClientSize.Width);
                Int32 y = Location.Y + (size.Height - ClientSize.Height);
                SetLocation(x, y);

                m_Collapsed = true;
            }

            ResumeLayout(true);

            m_ButtonResize.Enabled = true;
            ActiveControl = m_ButtonResize;
        }

        private void ButtonScanClick(Object sender, EventArgs e)
        {
            ClientsScan();
        }

        private void ButtonStartClick(Object sender, EventArgs e)
        {
            MacroStart();
        }

        private void ButtonStopClick(Object sender, EventArgs e)
        {
            MacroStop();
        }

        private void CheckBoxAltCheckedChanged(Object sender, EventArgs e)
        {
            if (!m_CheckBoxAlt.Focused)
                return;

            Int32 selectedIndex = m_ComboBoxPresets.SelectedIndex;

            if (selectedIndex < 1)
                return;

            m_Presets[(selectedIndex - 1)].ModifierAlt = m_CheckBoxAlt.Checked;
            m_PresetsChanged = true;
        }

        private void CheckBoxCtrlCheckedChanged(Object sender, EventArgs e)
        {
            if (!m_CheckBoxCtrl.Focused)
                return;

            Int32 selectedIndex = m_ComboBoxPresets.SelectedIndex;

            if (selectedIndex < 1)
                return;

            m_Presets[(selectedIndex - 1)].ModifierCtrl = m_CheckBoxCtrl.Checked;
            m_PresetsChanged = true;
        }

        private void CheckBoxShiftCheckedChanged(Object sender, EventArgs e)
        {
            if (!m_CheckBoxShift.Focused)
                return;

            Int32 selectedIndex = m_ComboBoxPresets.SelectedIndex;

            if (selectedIndex < 1)
                return;

            m_Presets[(selectedIndex - 1)].ModifierShift = m_CheckBoxShift.Checked;
            m_PresetsChanged = true;
        }

        private void ComboBoxKeysSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!m_ComboBoxKeys.Focused)
                return;

            Int32 selectedIndex = m_ComboBoxPresets.SelectedIndex;

            if (selectedIndex < 1)
                return;

            m_Presets[(selectedIndex - 1)].Key = m_ComboBoxKeys.SelectedItem.ToString();
            m_PresetsChanged = true;
        }

        [SuppressMessage("ReSharper", "InvertIf")]
        private void ComboBoxPresetsSelectedIndexChanged(Object sender, EventArgs e)
        {
            Int32 selectedIndex = m_ComboBoxPresets.SelectedIndex;

            if (selectedIndex == m_PresetsPreviousIndex)
                return;

            if (selectedIndex == 0)
            {
                m_ButtonAdd.Enabled = true;
                m_ButtonRemove.Enabled = false;

                m_PresetsPreviousIndex = 0;
                return;
            }

            Macro macro = m_Presets[(selectedIndex - 1)];
            String macroKey = macro.Key;
            Int32 macroKeyIndex = -1;

            for (Int32 i = 0; i < s_Keys.Length; ++i)
            {
                if (s_Keys[i].Name == macroKey)
                {
                    macroKeyIndex = i;
                    break;
                }
            }

            if (macroKeyIndex == -1)
            {
                m_Messenger.DisplayError(String.Format(Resources.ErrorPresetsKey, macro.Name));
                m_ComboBoxPresets.SelectedIndex = m_PresetsPreviousIndex;
            }
            else
            {
                m_ButtonAdd.Enabled = false;
                m_ButtonRemove.Enabled = true;

                m_ComboBoxKeys.SelectedIndex = macroKeyIndex;
                m_NumericUpDownDelay.Value = macro.Delay;
                m_CheckBoxAlt.Checked = macro.ModifierAlt;
                m_CheckBoxCtrl.Checked = macro.ModifierCtrl;
                m_CheckBoxShift.Checked = macro.ModifierShift;

                m_PresetsPreviousIndex = selectedIndex;
            }
        }

        private void NumericUpDownDelayValueChanged(Object sender, EventArgs e)
        {
            if (!m_NumericUpDownDelay.Focused)
                return;

            Int32 selectedIndex = m_ComboBoxPresets.SelectedIndex;

            if (selectedIndex < 1)
                return;

            m_Presets[(selectedIndex - 1)].Delay = (UInt32)m_NumericUpDownDelay.Value;
            m_PresetsChanged = true;
        }

        private void TimerClientTick(Object sender, EventArgs e)
        {
            if (NativeMethods.IsValidWindow(m_MacroCurrent.WindowHandle))
                return;

            MacroStop();

            if (m_Clients.Count == 1)
            {
                m_Clients.Clear();

                m_ComboBoxClients.BeginUpdate();
                m_ComboBoxClients.Items.Clear();
                m_ComboBoxClients.Items.Add(Resources.TextClientsEmpty);
                m_ComboBoxClients.SelectedIndex = 0;
                m_ComboBoxClients.EndUpdate();

                m_ComboBoxClients.Enabled = false;

                ActiveControl = m_ButtonScan;

                m_Messenger.DisplayError(Resources.ErrorClientNone);
            }
            else
            {
                Int32 selectedIndex = m_ComboBoxClients.SelectedIndex;

                m_Clients.RemoveAt(selectedIndex);

                m_ComboBoxClients.BeginUpdate();
                m_ComboBoxClients.Items.RemoveAt(selectedIndex);
                m_ComboBoxClients.EndUpdate();

                ActiveControl = m_ButtonStart;

                m_Messenger.DisplayError(Resources.ErrorClientSome);
            }
        }

        private void TimerMacroTick(Object sender, EventArgs e)
        {
            IntPtr windowHandle = m_MacroCurrent.WindowHandle;
            UInt32 keyCode = m_MacroCurrent.KeyCode;

            if (m_MacroCurrent.Modifiers > 0)
            {
                UInt32 windowThreadIdClient = m_MacroCurrent.WindowThreadId;
                UInt32 windowThreadIdThis = NativeMethods.GetThreadId();

                if (NativeMethods.ThreadInputAttach(windowThreadIdThis, windowThreadIdClient))
                {
                    Byte[] keyboardState = NativeMethods.GetKeyboardState();

                    if (m_CheckBoxAlt.Checked)
                        keyboardState[18] |= 0x80;

                    if (m_CheckBoxCtrl.Checked)
                        keyboardState[17] |= 0x80;

                    if (m_CheckBoxShift.Checked)
                        keyboardState[16] |= 0x80;

                    NativeMethods.KeyboardState(keyboardState);
                    NativeMethods.SendKeyDown(windowHandle, m_KeyboardLayout, keyCode);

                    if (m_CheckBoxAlt.Checked)
                        keyboardState[18] &= 0x7F;

                    if (m_CheckBoxCtrl.Checked)
                        keyboardState[17] &= 0x7F;

                    if (m_CheckBoxShift.Checked)
                        keyboardState[16] &= 0x7F;

                    NativeMethods.KeyboardState(keyboardState);
                    NativeMethods.ThreadInputDetach(windowThreadIdThis, windowThreadIdClient);

                    if (CountersIncrease())
                        MacroStop();
                }
                else
                {
                    MacroStop();
                    m_Messenger.DisplayError(Resources.ErrorClientInput);
                }
            }
            else
            {
                NativeMethods.SendKeyDown(windowHandle, m_KeyboardLayout, keyCode);

                if (CountersIncrease())
                    MacroStop();
            }
        }

        private void ToolStripMenuItemTopMostCheckedChanged(Object sender, EventArgs e)
        {
            TopMost = Settings.Default.TopMost = m_ToolStripMenuItemTopMost.Checked;
        }

        private void ToolStripMenuItemTransparencyChanged(Object sender, EventArgs e)
        {
            if (m_ToolStripMenuItemTransparency.Checked)
            {
                Settings.Default.Transparent = true;
                Opacity = 0.60d;
            }
            else
            {
                Settings.Default.Transparent = false;
                Opacity = 1.0d;
            }
        }
        #endregion

        #region Methods: Instance
        private Boolean CountersIncrease()
        {
            ++m_CounterRuns;
            m_TextBoxCounterRuns.Text = m_CounterRuns.ToString("N", m_NumberFormatInfo);

            m_CounterTime += m_NumericUpDownDelay.Value;
            m_TextBoxCounterTime.Text = m_CounterTime.ToStringTime();

            if (m_MacroCurrent.Runs == -1m)
                return false;

            return (--m_MacroCurrent.Runs == 0m);
        }

        private Boolean EnumerateWindow(IntPtr windowHandle, IntPtr lParameter)
        {
            String windowClass = NativeMethods.GetWindowClass(windowHandle);

            if (!windowClass.Contains("Ultima Online"))
                return true;

            String windowText = NativeMethods.GetWindowText(windowHandle);

            if (!windowText.Contains("Ultima Online"))
                return true;

            UInt32 windowThreadId = NativeMethods.GetWindowThreadId(windowHandle);

            m_Clients.Add(new Client(windowText, windowHandle, windowThreadId));

            return true;
        }

        private void ClientsDisable()
        {
            ActiveControl = null;

            m_ButtonStart.Enabled = false;
            m_TableLayoutPanelClients.Enabled = false;

            m_Clients.Clear();

            m_ComboBoxClients.BeginUpdate();
            m_ComboBoxClients.Items.Clear();
            m_ComboBoxClients.Items.Add(Resources.TextClientsEmpty);
            m_ComboBoxClients.SelectedIndex = 0;
            m_ComboBoxClients.EndUpdate();

            Refresh();
        }

        private void ClientsEnable()
        {
            Boolean clientsFound = (m_Clients.Count > 0);

            m_ComboBoxClients.Enabled = clientsFound;
            m_TableLayoutPanelClients.Enabled = true;
            m_ButtonStart.Enabled = clientsFound;

            ActiveControl = m_ButtonScan;
        }

        private void ClientsScan()
        {
            ClientsDisable();

            NativeMethods.EnumerateWindows(EnumerateWindow);

            Int32 clientsCount = m_Clients.Count;

            if (clientsCount > 0)
            {
                Object[] names = new Object[clientsCount];

                for (Int32 i = 0; i < clientsCount; ++i)
                {
                    Client client = m_Clients[i];
                    names[i] = String.Concat(client.Name, " (", client.WindowHandle, ")");
                }

                m_ComboBoxClients.BeginUpdate();
                m_ComboBoxClients.Items.Clear();
                m_ComboBoxClients.Items.AddRange(names);
                m_ComboBoxClients.SelectedIndex = 0;
                m_ComboBoxClients.EndUpdate();
            }

            Thread.Sleep(250);

            ClientsEnable();
        }

        [SuppressMessage("ReSharper", "LocalizableElement")]
        private void CountersReset()
        {
            m_CounterRuns = 0m;
            m_TextBoxCounterRuns.Text = "0";

            m_CounterTime = 0m;
            m_TextBoxCounterTime.Text = "0s";
        }

        private void MacroDisable()
        {
            m_ButtonStart.Enabled = false;

            m_TableLayoutPanelClients.Enabled = false;
            m_TableLayoutPanelPresets.Enabled = false;

            m_LabelKey.Enabled = false;
            m_ComboBoxKeys.Enabled = false;

            m_LabelDelay.Enabled = false;
            m_NumericUpDownDelay.Enabled = false;

            m_LabelModifiers.Enabled = false;
            m_CheckBoxAlt.Enabled = false;
            m_CheckBoxCtrl.Enabled = false;
            m_CheckBoxShift.Enabled = false;

            if (m_NumericUpDownRuns.Value == 0m)
            {
                m_LabelRuns.Enabled = false;
                m_NumericUpDownRuns.Enabled = false;
            }
            else
                m_NumericUpDownRuns.ReadOnly = true;

            m_ButtonStop.Enabled = true;
            ActiveControl = m_ButtonStop;
        }

        private void MacroEnable()
        {
            m_ButtonStop.Enabled = false;

            m_TableLayoutPanelClients.Enabled = true;
            m_TableLayoutPanelPresets.Enabled = true;

            m_LabelKey.Enabled = true;
            m_ComboBoxKeys.Enabled = true;

            m_LabelDelay.Enabled = true;
            m_NumericUpDownDelay.Enabled = true;

            m_LabelModifiers.Enabled = true;
            m_CheckBoxAlt.Enabled = true;
            m_CheckBoxCtrl.Enabled = true;
            m_CheckBoxShift.Enabled = true;

            m_LabelRuns.Enabled = true;
            m_NumericUpDownRuns.ReadOnly = false;
            m_NumericUpDownRuns.Enabled = true;

            m_ButtonStart.Enabled = true;
            ActiveControl = m_ButtonStart;
        }

        private void MacroStart()
        {
            MacroDisable();

            Client client = m_Clients[m_ComboBoxClients.SelectedIndex];
            UInt32 keyCode = s_Keys[m_ComboBoxKeys.SelectedIndex].Code;
            UInt32 modifiers = (UInt32)((m_CheckBoxAlt.Checked ? Keys.Alt : Keys.None) | (m_CheckBoxCtrl.Checked ? Keys.Control : Keys.None) | (m_CheckBoxShift.Checked ? Keys.Shift : Keys.None));
            Decimal runs = (m_NumericUpDownRuns.Value == 0m) ? -1m : m_NumericUpDownRuns.Value;

            m_MacroCurrent = new MacroRunning(client.WindowHandle, client.WindowThreadId, keyCode, modifiers, runs);

            if ((m_MacroLast != null) && (m_MacroLast != m_MacroCurrent))
                CountersReset();

            m_MacroLast = m_MacroCurrent;

            m_TimerMacro.Interval = (Int32)m_NumericUpDownDelay.Value;

            m_TimerClient.Start();
            m_TimerMacro.Start();
        }

        private void MacroStop()
        {
            m_TimerClient.Stop();
            m_TimerMacro.Stop();

            MacroEnable();
        }

        private void PresetsDisable()
        {
            ActiveControl = null;

            m_ButtonStart.Enabled = false;
            m_TableLayoutPanelMacro.Enabled = false;
            m_TableLayoutPanelPresets.Enabled = false;

            m_ComboBoxKeys.SelectedIndex = 0;
            m_NumericUpDownDelay.Value = 100m;
            m_CheckBoxAlt.Checked = false;
            m_CheckBoxCtrl.Checked = false;
            m_CheckBoxShift.Checked = false;

            Refresh();
        }

        private void PresetsEnable()
        {
            Boolean presetsExist = (m_Presets.Count > 0);

            m_ComboBoxPresets.Enabled = presetsExist;

            m_TableLayoutPanelMacro.Enabled = true;
            m_TableLayoutPanelPresets.Enabled = true;
            m_ButtonStart.Enabled = (m_Clients.Count > 0);

            ActiveControl = m_ButtonReload;
        }

        private void PresetsLoad()
        {
            PresetsDisable();

            Boolean caughtError = false;
            Boolean firstLoad = (m_Presets == null);

            FileStream stream = null;

            try
            {
                stream = new FileStream(Program.MacrosFile, FileMode.Open);

                DataContractSerializer dcs = new DataContractSerializer(typeof(MacroCollection));
                XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(stream, (new XmlDictionaryReaderQuotas()));

                m_Presets = (MacroCollection)dcs.ReadObject(reader);
            }
            catch
            {
                m_Presets = new MacroCollection();
                caughtError = true;
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }

            Int32 presetsCount = m_Presets.Count;
            Object[] names;

            if (presetsCount == 0)
                names = new Object[] { Resources.TextPresetsEmpty };
            else
            {
                Int32 namesLength = presetsCount + 1;

                names = new Object[namesLength];
                names[0] = Resources.TextPresetsUnselected;

                for (Int32 i = 1; i < namesLength; ++i)
                    names[i] = m_Presets[(i - 1)].Name;
            }

            m_ComboBoxPresets.BeginUpdate();
            m_ComboBoxPresets.Items.Clear();
            m_ComboBoxPresets.Items.AddRange(names);
            m_ComboBoxPresets.SelectedIndex = 0;
            m_ComboBoxPresets.EndUpdate();

            if (caughtError)
                m_Messenger.DisplayError(Resources.ErrorPresetsFile);
            else if (!firstLoad)
                m_Messenger.DisplayInformation(Resources.SuccessPresetsReload);

            PresetsEnable();
        }

        private void SetLocation(Int32 x, Int32 y)
        {
            if (x < 0)
                x = 0;
            else
            {
                Int32 screenRight = Screen.FromControl(this).WorkingArea.Right;

                if ((x + Size.Width) > screenRight)
                    x = screenRight - Size.Width;
            }

            if (y < 0)
                y = 0;
            else
            {
                Int32 screenBottom = Screen.FromControl(this).WorkingArea.Bottom;

                if ((y + Size.Height) > screenBottom)
                    y = screenBottom - Size.Height;
            }

            Location = new Point(x, y);
        }
        #endregion

        #region Methods: Overrides
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && (m_Components != null))
                m_Components.Dispose();

            base.Dispose(disposing);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (m_PresetsChanged)
            {
                try
                {
                    DataContractSerializer dcs = new DataContractSerializer(typeof(MacroCollection));
                    XmlWriterSettings xws = new XmlWriterSettings { Indent = true, IndentChars = "\t" };

                    using (XmlWriter writer = XmlWriter.Create(Program.MacrosFile, xws))
                    {
                        writer.WriteStartDocument(true);
                        dcs.WriteObject(writer, m_Presets);
                    }
                }
                catch
                {
                    m_Messenger.DisplayError(Resources.ErrorPresetsSave);
                }
            }

            Settings.Default.Save();

            base.OnFormClosing(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            Int32 keysLength = s_Keys.Length;
            Object[] items = new Object[keysLength];

            for (Int32 i = 0; i < keysLength; ++i)
                items[i] = s_Keys[i].Name;

            m_ComboBoxKeys.BeginUpdate();
            m_ComboBoxKeys.Items.AddRange(items);
            m_ComboBoxKeys.EndUpdate();

            Boolean topMost = Settings.Default.TopMost;
            Boolean transparency = Settings.Default.Transparent;

            m_ToolStripMenuItemTopMost.Checked = topMost;
            m_ToolStripMenuItemTransparency.Checked = transparency;

            ClientsScan();
            PresetsLoad();

            ActiveControl = m_ButtonStart.Enabled ? m_ButtonStart : m_ButtonScan;
            Opacity = transparency ? 0.6d : 1.0d;
            TopMost = topMost;

            base.OnLoad(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Program.MutexMessage)
                NativeMethods.Restore(this);

            base.WndProc(ref m);
        }
        #endregion

        #region Nesting: Structures
        private struct Client
        {
            #region Members: Instance
            private readonly IntPtr m_WindowHandle;
            private readonly String m_Name;
            private readonly UInt32 m_WindowThreadId;
            #endregion

            #region Properties: Instance
            public IntPtr WindowHandle
            {
                get { return m_WindowHandle; }
            }

            public String Name
            {
                get { return m_Name; }
            }

            public UInt32 WindowThreadId
            {
                get { return m_WindowThreadId; }
            }
            #endregion

            #region Constructors
            public Client(String name, IntPtr windowHandle, UInt32 windowThreadID)
            {
                m_WindowHandle = windowHandle;
                m_Name = name;
                m_WindowThreadId = windowThreadID;
            }
            #endregion
        }

        private struct Key
        {
            #region Members: Instance
            private readonly String m_Name;
            private readonly UInt32 m_Code;
            #endregion

            #region Properties: Instance
            public String Name
            {
                get { return m_Name; }
            }

            public UInt32 Code
            {
                get { return m_Code; }
            }
            #endregion

            #region Constructors
            [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
            public Key(Keys key)
            {
                m_Name = Enum.GetName(typeof(Keys), key).ToUpper();
                m_Code = (UInt32)key;
            }

            public Key(Keys key, String name)
            {
                m_Name = name;
                m_Code = (UInt32)key;
            }
            #endregion
        }
        #endregion
    }
}