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
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using WindowsInput;
using WindowsInput.Native;

namespace MCEControl {
    /// <summary>
    /// Supports sending raw text.
    /// </summary>
    public class CharsCommand : Command {
        public const string CmdPrefix = "chars:";
        private string chars;
        [XmlAttribute("Chars")] public string Chars { get => chars; set => chars = value; }

        public static List<CharsCommand> Commands = new List<CharsCommand>();
        static CharsCommand() {
            Commands.Add(new CharsCommand { Key = $"{CmdPrefix}" });
        }

        public CharsCommand() { }

        public override string ToString() {
            return $"Cmd=\"{Key}\"";
        }

        public override void Execute(string args, Reply reply) {
            String text = Regex.Unescape(args);
            // if command came in as a literal "chars:foo" command use args
            // otherwise, use the Chars property
            if (string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(chars))
                text = chars;

            Logger.Instance.Log4.Info($"Cmd: Sending {text.Length} chars: {text}");
            new InputSimulator().Keyboard.TextEntry(text);
        }
    }
}
