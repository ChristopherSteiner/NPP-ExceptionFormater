using Kbg.NppPluginNET.PluginInfrastructure;
using System;
using System.Linq;

namespace Kbg.NppPluginNET.ExceptionFormater
{
    public class ExceptionFormatHandler
    {
        private readonly IScintillaGateway scintilla;

        private const string SimpleNewLineIndicator = "     at";
        private const string FullExceptionIndicator = "------------------------------------------------------------    ";
        private const string ExceptionText = "Exception:        ";
        private const string MessageText = "Message    :";
        private const string TypeText = "Type       :";
        private const string HelpLinkText = "HelpLink   :";
        private const string SourceText = "Source     :";
        private const string StackTraceText = "StackTrace :";
        private const string FullExceptionNewLineIndicator = "                   ##";
        private const string RestServiceExceptionIndicator = "ERROR TYPE:";
        private const string RestServiceMessageText = "MESSAGE:";
        private const string RestServiceStackTraceText = "STACKTRACE:";

        public ExceptionFormatHandler(IScintillaGateway scintilla)
        {
            this.scintilla = scintilla;
        }

        public void Format()
        {
            scintilla.BeginUndoAction();
            int textLength = scintilla.GetTextLength();
            string text = scintilla.GetText(textLength);
            text = FormatText(text);
            scintilla.DeleteRange(new Position(0), textLength);
            scintilla.InsertText(new Position(0), text);            
            scintilla.EndUndoAction();
        }

        private string FormatText(string text)
        {
            if (text.StartsWith(FullExceptionIndicator))
            {
                text = text.Replace(FullExceptionIndicator, $"{Environment.NewLine}{FullExceptionIndicator}{Environment.NewLine}");
                text = text.Replace(ExceptionText, $"{ExceptionText.Trim()}{Environment.NewLine}");
                text = text.Replace(MessageText, $"{Environment.NewLine}{MessageText.Trim()}");
                text = text.Replace(TypeText, $"{Environment.NewLine}{TypeText.Trim()}");
                text = text.Replace(HelpLinkText, $"{Environment.NewLine}{HelpLinkText.Trim()}");
                text = text.Replace(SourceText, $"{Environment.NewLine}{SourceText.Trim()}");
                text = text.Replace(StackTraceText, $"{Environment.NewLine}{StackTraceText.Trim()}");
                text = text.Replace(FullExceptionNewLineIndicator, $"{Environment.NewLine}\t{FullExceptionNewLineIndicator.Trim()}");
                text = text.Replace("\t", "");
                text = string.Join(Environment.NewLine, text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Select(x => x.TrimStart()));
            }
            else
            {
                if(text.StartsWith(RestServiceExceptionIndicator))
                {
                    text = text.Replace(RestServiceMessageText, $"{Environment.NewLine}{RestServiceMessageText}");
                    text = text.Replace(RestServiceStackTraceText, $"{Environment.NewLine}{RestServiceStackTraceText}");
                }
                text = text.Replace(SimpleNewLineIndicator, $"{Environment.NewLine}\t{SimpleNewLineIndicator.Trim()}");
            }
            
            return text;
        }
    }
}
