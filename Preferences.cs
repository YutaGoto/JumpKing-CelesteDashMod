using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace JumpKing_CelesteDashMod
{
    public class Preferences : INotifyPropertyChanged
    {
        private bool _isEnabled = true;

        private  Dictionary<EBinding, int[]> keyBinds = new Dictionary<EBinding, int[]>()
        {
            { EBinding.Dash, new int[] { (int)Keys.E } },
        };

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public Binding[] Bindings
        {
            get
            {
                // ugly ass code
                List<Binding> bindings = new List<Binding>();
                keyBinds.ToList().ForEach((kvp) =>
                {
                    bindings.Add(new Binding(kvp));
                });
                return bindings.ToArray();
            }
            set => keyBinds = value.ToDictionary(x => x.Bind, x => x.Keys);
        }

        [XmlIgnore]
        public Dictionary<EBinding, int[]> KeyBindings
        {
            get => keyBinds;
            set
            {
                keyBinds = value;
                OnPropertyChanged();
            }
        }
        public void ForceUpdate() => OnPropertyChanged();

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public struct Binding
        {
            public Binding(KeyValuePair<EBinding, int[]> kvp) : this(kvp.Key, kvp.Value) { }
            public Binding(EBinding keyName, int[] actualKeys)
            {
                Bind = keyName;
                Keys = actualKeys;
            }

            public EBinding Bind;
            public int[] Keys;
        }
    }
}
