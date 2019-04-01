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
                            new KeyMapping("Left", Keys.Left, true),
                            new KeyMapping("Right", Keys.Right, true),
                            new KeyMapping("Forward", Keys.Up, true),
                            new KeyMapping("Backward", Keys.Down, true),
                            new KeyMapping("Jump", Keys.ControlKey, true),
                            new KeyMapping("Throw", Keys.Delete, true),
                            new KeyMapping("Open Book", Keys.Escape, true)
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

        public List<KeyMapping> LeftKeys
        {
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
                            new KeyMapping("Left", (Keys)Properties.Settings.Default.leftLeftKeyCode, true),
                            new KeyMapping("Right", (Keys)Properties.Settings.Default.leftRightKeyCode, true),
                            new KeyMapping("Forward", (Keys)Properties.Settings.Default.leftForwardKeyCode, true),
                            new KeyMapping("Backward", (Keys)Properties.Settings.Default.leftBackKeyCode, true),
                            new KeyMapping("Jump", (Keys)Properties.Settings.Default.leftJumpKeyCode, true),
                            new KeyMapping("Throw", (Keys)Properties.Settings.Default.leftThrowKeyCode, true),
                            new KeyMapping("Open Book", (Keys)Properties.Settings.Default.leftEscapeKeyCode, true)
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
                            new KeyMapping("Left", (Keys)Properties.Settings.Default.rightLeftKeyCode, true),
                            new KeyMapping("Right", (Keys)Properties.Settings.Default.rightRightKeyCode, true),
                            new KeyMapping("Forward", (Keys)Properties.Settings.Default.rightForwardKeyCode, true),
                            new KeyMapping("Backward", (Keys)Properties.Settings.Default.rightBackKeyCode, true),
                            new KeyMapping("Jump", (Keys)Properties.Settings.Default.rightJumpKeyCode, true),
                            new KeyMapping("Throw", (Keys)Properties.Settings.Default.rightThrowKeyCode, true),
                            new KeyMapping("Open Book", (Keys)Properties.Settings.Default.rightEscapeKeyCode, true)
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
