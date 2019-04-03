using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using TTMulti.Controls;

namespace TTMulti.Properties
{
    internal class SerializedSettings
    {
        public static SerializedSettings Default { get; } = new SerializedSettings();

        XmlSerializer keyMappingSerializer = new XmlSerializer(typeof(List<KeyMapping>));

        public List<KeyMapping> Bindings
        {
            get
            {
                using (StringReader sr = new StringReader(Properties.Settings.Default.keyBindings))
                {
                    try
                    {
                        return keyMappingSerializer.Deserialize(sr) as List<KeyMapping>;
                    }
                    catch
                    {
                        return new List<KeyMapping>()
                        {
                            new KeyMapping("Forward", Keys.Up, (Keys)Properties.Settings.Default.leftForwardKeyCode, (Keys)Properties.Settings.Default.rightForwardKeyCode, true),
                            new KeyMapping("Left", Keys.Left, (Keys)Properties.Settings.Default.leftLeftKeyCode, (Keys)Properties.Settings.Default.rightLeftKeyCode, true),
                            new KeyMapping("Backward", Keys.Down, (Keys)Properties.Settings.Default.leftBackKeyCode, (Keys)Properties.Settings.Default.rightBackKeyCode, true),
                            new KeyMapping("Right", Keys.Right, (Keys)Properties.Settings.Default.leftRightKeyCode, (Keys)Properties.Settings.Default.rightRightKeyCode, true),
                            new KeyMapping("Jump", Keys.ControlKey, (Keys)Properties.Settings.Default.leftJumpKeyCode, (Keys)Properties.Settings.Default.rightJumpKeyCode, true),
                            new KeyMapping("Throw", Keys.Delete, (Keys)Properties.Settings.Default.leftThrowKeyCode, (Keys)Properties.Settings.Default.rightThrowKeyCode, true),
                            new KeyMapping("Open Book", Keys.Escape, (Keys)Properties.Settings.Default.leftEscapeKeyCode, (Keys)Properties.Settings.Default.rightEscapeKeyCode, true)
                        };
                    }
                }
            }
            set
            {
                using (StringWriter sw = new StringWriter())
                {
                    keyMappingSerializer.Serialize(sw, value);
                    Properties.Settings.Default.keyBindings = sw.ToString();
                }
            }
        }
    }
}
