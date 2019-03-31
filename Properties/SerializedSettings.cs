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
        
        public List<KeyMapping> LeftKeys {
            get
            {
                using (StringReader sr = new StringReader(Properties.Settings.Default.leftKeys))
                {
                    try
                    {
                        return keyMappingSerializer.Deserialize(sr) as List<KeyMapping>;
                    }
                    catch
                    {
                        return new List<KeyMapping>()
                        {
                            new KeyMapping("Left", (Keys)Properties.Settings.Default.leftLeftKeyCode, Keys.Left, true),
                            new KeyMapping("Right", (Keys)Properties.Settings.Default.leftRightKeyCode, Keys.Right, true),
                            new KeyMapping("Up", (Keys)Properties.Settings.Default.leftForwardKeyCode, Keys.Up, true),
                            new KeyMapping("Down", (Keys)Properties.Settings.Default.leftBackKeyCode, Keys.Down, true),
                            new KeyMapping("Jump", (Keys)Properties.Settings.Default.leftJumpKeyCode, Keys.ControlKey, true),
                            new KeyMapping("Throw", (Keys)Properties.Settings.Default.leftThrowKeyCode, Keys.Delete, true),
                            new KeyMapping("Book", (Keys)Properties.Settings.Default.leftEscapeKeyCode, Keys.Escape, true)
                        };
                    }
                }
            }
            set
            {
                using (StringWriter sw = new StringWriter())
                {
                    keyMappingSerializer.Serialize(sw, value);
                    Properties.Settings.Default.leftKeys = sw.ToString();
                }
            }
        }

        public List<KeyMapping> RightKeys
        {
            get
            {
                using (StringReader sr = new StringReader(Properties.Settings.Default.rightKeys))
                {
                    try
                    {
                        return keyMappingSerializer.Deserialize(sr) as List<KeyMapping>;
                    }
                    catch
                    {
                        return new List<KeyMapping>()
                        {
                            new KeyMapping("Left", (Keys)Properties.Settings.Default.rightLeftKeyCode, Keys.Left, true),
                            new KeyMapping("Right", (Keys)Properties.Settings.Default.rightRightKeyCode, Keys.Right, true),
                            new KeyMapping("Up", (Keys)Properties.Settings.Default.rightForwardKeyCode, Keys.Up, true),
                            new KeyMapping("Down", (Keys)Properties.Settings.Default.rightBackKeyCode, Keys.Down, true),
                            new KeyMapping("Jump", (Keys)Properties.Settings.Default.rightJumpKeyCode, Keys.ControlKey, true),
                            new KeyMapping("Throw", (Keys)Properties.Settings.Default.rightThrowKeyCode, Keys.Delete, true),
                            new KeyMapping("Book", (Keys)Properties.Settings.Default.rightEscapeKeyCode, Keys.Escape, true)
                        };
                    }
                }
            }
            set
            {
                using (StringWriter sw = new StringWriter())
                {
                    keyMappingSerializer.Serialize(sw, value);
                    Properties.Settings.Default.rightKeys = sw.ToString();
                }
            }
        }
    }
}
