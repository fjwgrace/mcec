﻿//-------------------------------------------------------------------
// Copyright © 2019 Kindel Systems, LLC
// http://www.kindel.com
// charlie@kindel.com
// 
// Published under the MIT License.
// Source control on SourceForge 
//    http://sourceforge.net/projects/mcecontroller/
//-------------------------------------------------------------------

using System;
using System.Globalization;
using System.Xml.Serialization;
using WindowsInput;
using WindowsInput.Native;

namespace MCEControl {
    /// <summary>
    /// Simulates mouse movements.
    /// </summary>
    class MouseCommands : Command {
        private String _action;
        private String[] _parameters;
        public const string CmdPrefix = "mouse:";

        public static MouseCommands[] Commands = new MouseCommands[] {
            new MouseCommands{ Key = $"{CmdPrefix }lbc" },
            new MouseCommands{ Key = $"{CmdPrefix }lbc" },
            new MouseCommands{ Key = $"{CmdPrefix }lbdc" },
            new MouseCommands{ Key = $"{CmdPrefix }lbd" },
            new MouseCommands{ Key = $"{CmdPrefix }lbu" },
            new MouseCommands{ Key = $"{CmdPrefix }rbc" },
            new MouseCommands{ Key = $"{CmdPrefix }rbdc" },
            new MouseCommands{ Key = $"{CmdPrefix }rbd" },
            new MouseCommands{ Key = $"{CmdPrefix }rbu" },
            new MouseCommands{ Key = $"{CmdPrefix }xbc" },
            new MouseCommands{ Key = $"{CmdPrefix }xbcd" },
            new MouseCommands{ Key = $"{CmdPrefix }xbd" },
            new MouseCommands{ Key = $"{CmdPrefix }xbu" },
            new MouseCommands{ Key = $"{CmdPrefix }mm" },
            new MouseCommands{ Key = $"{CmdPrefix }mt" },
            new MouseCommands{ Key = $"{CmdPrefix }hs" },
            new MouseCommands{ Key = $"{CmdPrefix }vs" },
        };
        public MouseCommands() { }

        public MouseCommands(String cmd) {
            Key = CmdPrefix;
            _parameters = cmd.Substring(CmdPrefix.Length, cmd.Length - CmdPrefix.Length).Split(',');
            if (_parameters.Length > 0)
                _action = _parameters[0];
        }

        public override string ToString() {
            return $"Cmd=\"{Key}\"";
        }

        public override void Execute(Reply reply) {
            var sim = new InputSimulator();
            // Format is "mouse:<action>[,<parameters>]
            switch (_action) {
                case "lbc": sim.Mouse.LeftButtonClick(); break;
                case "lbdc": sim.Mouse.LeftButtonDoubleClick(); break;
                case "lbd": sim.Mouse.LeftButtonDown(); break;
                case "lbu": sim.Mouse.LeftButtonUp(); break;

                case "rbc": sim.Mouse.RightButtonClick(); break;
                case "rbdc": sim.Mouse.RightButtonDoubleClick(); break;
                case "rbd": sim.Mouse.RightButtonDown(); break;
                case "rbu": sim.Mouse.RightButtonUp(); break;

                // "mouse:xbc,3" - Mouse button 3 click
                case "xbc": sim.Mouse.XButtonClick(GetIntOrZero(_parameters, 1)); break;
                case "xbdc": sim.Mouse.XButtonDoubleClick(GetIntOrZero(_parameters, 1)); break;
                case "xbd": sim.Mouse.XButtonDown(GetIntOrZero(_parameters, 1)); break;
                case "xbu": sim.Mouse.XButtonUp(GetIntOrZero(_parameters, 1)); break;

                // "mouse:mb,15,20" - Move mouse 15 in X direction, and 20 in Y direction
                case "mm": sim.Mouse.MoveMouseBy(GetIntOrZero(_parameters, 1), GetIntOrZero(_parameters, 2)); break;

                // "mouse:mt,812,562" - Move mouse to (812,562) on the screen
                case "mt": sim.Mouse.MoveMouseTo(GetIntOrZero(_parameters, 1), GetIntOrZero(_parameters, 2)); break;

                // "mouse:mtv,812,562" - Move mouse to (812,562) on the virtual desktop screen
                case "mtv": sim.Mouse.MoveMouseToPositionOnVirtualDesktop(GetIntOrZero(_parameters, 1), GetIntOrZero(_parameters, 2)); break;

                case "hs": sim.Mouse.HorizontalScroll(GetIntOrZero(_parameters, 1)); break;
                case "vs": sim.Mouse.VerticalScroll(GetIntOrZero(_parameters, 1)); break;
            }
        }

        private int GetIntOrZero(String[] s, int index) {
            int val = 0;
            if (index < s.Length) {
                if (!int.TryParse(s[index], out val))
                    return 0;
            }
            return val;
        }
    }
}