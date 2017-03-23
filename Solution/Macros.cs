#region Using Directives
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
#endregion

namespace MacroUO
{
    [CollectionDataContract(ItemName="Macro", Name="Macros", Namespace="")]
    public sealed class MacroCollection : List<Macro> { }

    [DataContract(Namespace="")]
    public sealed class Macro
    {
        #region Members: Instance
        private Boolean m_ModifierAlt;
        private Boolean m_ModifierCtrl;
        private Boolean m_ModifierShift;
        private String m_Key;
        private String m_Name;
        private UInt32 m_Delay;
        #endregion

        #region Members: Properties
        [DataMember(IsRequired=true, Name="ModifierALT", Order=2)]
        public Boolean ModifierAlt
        {
            get { return m_ModifierAlt; }
            set { m_ModifierAlt = value; }
        }

        [DataMember(IsRequired=true, Name="ModifierCTRL", Order=3)]
        public Boolean ModifierCtrl
        {
            get { return m_ModifierCtrl; }
            set { m_ModifierCtrl = value; }
        }

        [DataMember(IsRequired=true, Name="ModifierSHIFT", Order=4)]
        public Boolean ModifierShift
        {
            get { return m_ModifierShift; }
            set { m_ModifierShift = value; }
        }

        [DataMember(IsRequired=true, Name="Key", Order=1)]
        public String Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }

        [DataMember(IsRequired=true, Name="Name", Order=0)]
        public String Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        [DataMember(IsRequired=true, Name="Delay", Order=5)]
        public UInt32 Delay
        {
            get { return m_Delay; }
            set { m_Delay = value; }
        }
        #endregion
    }

    public sealed class MacroRunning : IEquatable<MacroRunning>
    {
        #region Instances
        private Decimal m_Runs;
        private readonly IntPtr m_WindowHandle;
        private readonly UInt32 m_Key;
        private readonly UInt32 m_Modifiers;
        private readonly UInt32 m_WindowThreadID;
        #endregion

        #region Properties
        public Decimal Runs
        {
            get { return m_Runs; }
            set { m_Runs = value; }
        }

        public IntPtr WindowHandle
        {
            get { return m_WindowHandle; }
        }

        public UInt32 KeyCode
        {
            get { return m_Key; }
        }

        public UInt32 Modifiers
        {
            get { return m_Modifiers; }
        }

        public UInt32 WindowThreadId
        {
            get { return m_WindowThreadID; }
        }
        #endregion

        #region Constructors
        public MacroRunning(IntPtr windowHandle, UInt32 windowThreadId, UInt32 keyCode, UInt32 modifiers, Decimal runs)
        {
            m_Runs = runs;
            m_WindowHandle = windowHandle;
            m_Key = keyCode;
            m_Modifiers = modifiers;
            m_WindowThreadID = windowThreadId;
        }
        #endregion

        #region Methods: IEquatable
        public Boolean Equals(MacroRunning other)
        {
            if (ReferenceEquals(null, other))
                return false;

            return ((m_WindowHandle == other.WindowHandle) && (m_Key == other.KeyCode) && (m_Modifiers == other.Modifiers));
        }
        #endregion

        #region Methods: Operators
        public static Boolean operator ==(MacroRunning left, MacroRunning right)
        {
            return (ReferenceEquals(null, left) ? ReferenceEquals(null, right) : left.Equals(right));
        }

        public static Boolean operator !=(MacroRunning left, MacroRunning right)
        {
            return (ReferenceEquals(null, left) ? !ReferenceEquals(null, right) : !left.Equals(right));
        }
        #endregion

        #region Methods: Overrides
        public override Boolean Equals(Object obj)
        {
            return Equals(obj as MacroRunning);
        }

        public override Int32 GetHashCode()
        {
            unchecked
            {
                Int32 hash = (Int32)2166136261;
                hash = (hash * 16777619) ^ m_WindowHandle.GetHashCode();
                hash = (hash * 16777619) ^ m_Key.GetHashCode();
                hash = (hash * 16777619) ^ m_Modifiers.GetHashCode();

                return hash;
            }
        }
        #endregion
    }
}