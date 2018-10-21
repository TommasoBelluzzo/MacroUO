#region Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MacroUO.Properties;
#endregion

namespace MacroUO
{
    [DataContract(Namespace="")]
    public sealed class Macro
    {
        #region Constants
        public const Int32 MAXIMUM_NAME_LENGTH = 30;
        public const UInt32 MINIMUM_DELAY = 100;
        public const UInt32 MAXIMUM_DELAY = 60000;
        #endregion

        #region Members (Static)
        private static readonly MacroKey[] s_Keys = 
        {
            new MacroKey(Keys.F1),
            new MacroKey(Keys.F2),
            new MacroKey(Keys.F3),
            new MacroKey(Keys.F4),
            new MacroKey(Keys.F5),
            new MacroKey(Keys.F6),
            new MacroKey(Keys.F7),
            new MacroKey(Keys.F8),
            new MacroKey(Keys.F9),
            new MacroKey(Keys.F10),
            new MacroKey(Keys.F11),
            new MacroKey(Keys.F12),
            new MacroKey(Keys.D0, "0"),
            new MacroKey(Keys.D1, "1"),
            new MacroKey(Keys.D2, "2"),
            new MacroKey(Keys.D3, "3"),
            new MacroKey(Keys.D4, "4"),
            new MacroKey(Keys.D5, "5"),
            new MacroKey(Keys.D6, "6"),
            new MacroKey(Keys.D7, "7"),
            new MacroKey(Keys.D8, "8"),
            new MacroKey(Keys.D9, "9"),
            new MacroKey(Keys.A),
            new MacroKey(Keys.B),
            new MacroKey(Keys.C),
            new MacroKey(Keys.D),
            new MacroKey(Keys.E),
            new MacroKey(Keys.F),
            new MacroKey(Keys.G),
            new MacroKey(Keys.H),
            new MacroKey(Keys.I),
            new MacroKey(Keys.J),
            new MacroKey(Keys.K),
            new MacroKey(Keys.L),
            new MacroKey(Keys.M),
            new MacroKey(Keys.N),
            new MacroKey(Keys.O),
            new MacroKey(Keys.P),
            new MacroKey(Keys.Q),
            new MacroKey(Keys.R),
            new MacroKey(Keys.S),
            new MacroKey(Keys.T),
            new MacroKey(Keys.U),
            new MacroKey(Keys.V),
            new MacroKey(Keys.W),
            new MacroKey(Keys.X),
            new MacroKey(Keys.Y),
            new MacroKey(Keys.Z),
            new MacroKey(Keys.Multiply, "NUMPAD *"),
            new MacroKey(Keys.Add, "NUMPAD +"),
            new MacroKey(Keys.Subtract, "NUMPAD -"),
            new MacroKey(Keys.Divide, "NUMPAD /"),
            new MacroKey(Keys.NumPad0, "NUMPAD 0"),
            new MacroKey(Keys.NumPad1, "NUMPAD 1"),
            new MacroKey(Keys.NumPad2, "NUMPAD 2"),
            new MacroKey(Keys.NumPad3, "NUMPAD 3"),
            new MacroKey(Keys.NumPad4, "NUMPAD 4"),
            new MacroKey(Keys.NumPad5, "NUMPAD 5"),
            new MacroKey(Keys.NumPad6, "NUMPAD 6"),
            new MacroKey(Keys.NumPad7, "NUMPAD 7"),
            new MacroKey(Keys.NumPad8, "NUMPAD 8"),
            new MacroKey(Keys.NumPad9, "NUMPAD 9"),
            new MacroKey(Keys.Back, "BACK"),
            new MacroKey(Keys.Delete, "DELETE"),
            new MacroKey(Keys.End, "END"),
            new MacroKey(Keys.Enter, "ENTER"),
            new MacroKey(Keys.Escape, "ESCAPE"),
            new MacroKey(Keys.Home, "HOME"),
            new MacroKey(Keys.Insert, "INSERT"),
            new MacroKey(Keys.PageDown, "PAGE DOWN"),
            new MacroKey(Keys.PageUp, "PAGE UP"),
            new MacroKey(Keys.Pause, "PAUSE"),
            new MacroKey(Keys.Space, "SPACE"),
            new MacroKey(Keys.Tab, "TABULATOR")
        };

        private static readonly Regex s_RegexName = new Regex(@"^[0-9a-z]+( ?[0-9a-z]+)*$", (RegexOptions.Compiled | RegexOptions.IgnoreCase));
        #endregion

        #region Properties
        [DataMember(IsRequired=true, Name="ModifierALT", Order=2)]
        public Boolean ModifierAlt { get; set; }

        [DataMember(IsRequired=true, Name="ModifierCTRL", Order=3)]
        public Boolean ModifierCtrl { get; set; }

        [DataMember(IsRequired=true, Name="ModifierSHIFT", Order=4)]
        public Boolean ModifierShift { get; set; }

        public MacroKey Key { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(IsRequired=true, Name="Key", Order=1)]
        public String KeyProxy
        {
            get => Key.Name;
            set
            {
                for (Int32 i = 0; i < s_Keys.Length; ++i)
                {
                    MacroKey current = s_Keys[i];

                    if (current.Name == value)
                    {
                        Key = current;
                        return;
                    }
                }

                Key = new MacroKey(Keys.None, value);
            }
        }

        [DataMember(IsRequired=true, Name="Name", Order=0)]
        public String Name { get; set; }

        [DataMember(IsRequired=true, Name="Delay", Order=5)]
        public UInt32 Delay { get; set; }
        #endregion

        #region Methods (Static)
        public static Boolean ValidateName(String name)
        {
            return s_RegexName.IsMatch(name);
        }

        public static Object[] GetKeys()
        {
            Int32 keysLength = s_Keys.Length;
            Object[] keys = new Object[keysLength];

            for (Int32 i = 0; i < keysLength; ++i)
                keys[i] = s_Keys[i];

            return keys;
        }
        #endregion
    }

    [CollectionDataContract(ItemName="Macro", Name="Macros", Namespace="")]
    public sealed class MacroCollection : List<Macro>
    {
        #region Methods
        public String Validate()
        {
            for (Int32 i = 0; i < Count; ++i)
            {
                Macro preset = this[i];
                String presetName = preset.Name;

                if (String.IsNullOrWhiteSpace(presetName))
                    return String.Format(CultureInfo.CurrentCulture, Resources.ErrorPresetsFileNameUnspecified, (i + 1).ToOrdinal());

                if (!Macro.ValidateName(presetName))
                    return String.Format(CultureInfo.CurrentCulture, Resources.ErrorPresetsFileNameInvalid, presetName, (i + 1).ToOrdinal());

                if (presetName.Length > Macro.MAXIMUM_NAME_LENGTH)
                    return String.Format(CultureInfo.CurrentCulture, Resources.ErrorPresetsFileNameLength, presetName, (i + 1).ToOrdinal(), Macro.MAXIMUM_NAME_LENGTH);

                for (Int32 j = 0; j < i; ++j)
                {
                    if (this[j].Name == presetName)
                        return String.Format(CultureInfo.CurrentCulture, Resources.ErrorPresetsFileNameDuplicate, presetName, (j + 1).ToOrdinal());
                }

                if (preset.Key.Code == 0)
                    return String.Format(CultureInfo.CurrentCulture, Resources.ErrorPresetsFileKey, preset.Key, presetName);

                UInt32 presetDelay = preset.Delay;

                if ((presetDelay < Macro.MINIMUM_DELAY) || (presetDelay > Macro.MAXIMUM_DELAY))
                    return String.Format(CultureInfo.CurrentCulture, Resources.ErrorPresetsFileDelay, presetDelay, presetName, Macro.MINIMUM_DELAY, Macro.MAXIMUM_DELAY);
            }

            return null;
        }

        public String[] GetNames(params Int32[] exclusions)
        {
            List<String> names;

            if (exclusions == null)
            {
                names = new List<String>(Count);

                for (Int32 i = 0; i < Count; ++i)
                    names.Add(this[i].Name);
            }
            else
            {
                names = new List<String>(Count - exclusions.Length);

                for (Int32 i = 0; i < Count; ++i)
                {
                    if (exclusions.Contains(i))
                        continue;

                    names.Add(this[i].Name);
                }
            }

            return names.ToArray();
        }
        #endregion
    }

    public sealed class MacroRunning : IEquatable<MacroRunning>
    {
        #region Properties
        public Decimal Runs { get; set; }

        public IntPtr WindowHandle { get; }

        public UInt32 KeyCode { get; }

        public UInt32 Modifiers { get; }

        public UInt32 WindowThreadId { get; }
        #endregion

        #region Constructors
        public MacroRunning(IntPtr windowHandle, UInt32 windowThreadId, UInt32 keyCode, UInt32 modifiers, Decimal runs)
        {
            Runs = runs;
            WindowHandle = windowHandle;
            KeyCode = keyCode;
            Modifiers = modifiers;
            WindowThreadId = windowThreadId;
        }
        #endregion

        #region Operators
        public static Boolean operator ==(MacroRunning left, MacroRunning right)
        {
            return left?.Equals(right) ?? (right is null);
        }

        public static Boolean operator !=(MacroRunning left, MacroRunning right)
        {
            return !left?.Equals(right) ?? !(right is null);
        }
        #endregion

        #region Methods
        public Boolean Equals(MacroRunning other)
        {
            if (other is null)
                return false;

            return ((WindowHandle == other.WindowHandle) && (KeyCode == other.KeyCode) && (Modifiers == other.Modifiers));
        }

        public override Boolean Equals(Object obj)
        {
            return Equals(obj as MacroRunning);
        }

        public override Int32 GetHashCode()
        {
            unchecked
            {
                Int32 hash = (Int32)2166136261;
                hash = (hash * 16777619) ^ WindowHandle.GetHashCode();
                hash = (hash * 16777619) ^ KeyCode.GetHashCode();
                hash = (hash * 16777619) ^ Modifiers.GetHashCode();

                return hash;
            }
        }
        #endregion
    }

    public struct MacroKey : IEquatable<MacroKey>
    {
        #region Properties
        public String Name { get; }

        public UInt32 Code { get; }
        #endregion

        #region Constructors
        public MacroKey(Keys key)
        {
            String name = Enum.GetName(typeof(Keys), key);

            Name = ((name != null) ? name.ToUpper(CultureInfo.CurrentCulture) : String.Empty);
            Code = (UInt32)key;
        }

        public MacroKey(Keys key, String name)
        {
            Name = name;
            Code = (UInt32)key;
        }
        #endregion

        #region Operators
        public static Boolean operator ==(MacroKey left, MacroKey right)
        {
            return left.Equals(right);
        }

        public static Boolean operator !=(MacroKey left, MacroKey right)
        {
            return !left.Equals(right);
        }
        #endregion

        #region Methods
        public Boolean Equals(MacroKey other)
        {
            return ((Name == other.Name) && (Code == other.Code));
        }

        public override Boolean Equals(Object obj)
        {
            return ((obj is MacroKey key) && Equals(key));
        }

        public override Int32 GetHashCode()
        {
            unchecked
            {
                Int32 hash = (Int32)2166136261;
                hash = (hash * 16777619) ^ Name.GetHashCode();
                hash = (hash * 16777619) ^ Code.GetHashCode();

                return hash;
            }
        }

        public override String ToString()
        {
            return Name;
        }
        #endregion
    }
}