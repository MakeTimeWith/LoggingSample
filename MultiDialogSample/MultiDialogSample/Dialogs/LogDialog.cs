using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using System.Diagnostics;

namespace MultiDialogSample.Dialogs
{
    [Serializable]
    public class LogDialog : IDialog<object>
    {
        protected string methodName;
        public virtual async Task StartAsync(IDialogContext context)
        {
            throw new NotImplementedException();
        }

        protected void LogActivity(string methodname, IDialogContext context)
        {
            this.methodName = methodname;
            Trace.TraceInformation($"{context.Activity.From.Id};{GetType().Name};{methodName}");
        }

        protected void LogActivity(IDialogContext context, string message)
        {
            Trace.TraceInformation($"{context.Activity.From.Id};{GetType().Name};{methodName};{message}");
        }

        protected void LogError(IDialogContext context, string message)
        {
            Trace.TraceError($"{context.Activity.From.Id};{GetType().Name};{methodName};{message}");
        }
    }
}