using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace MultiDialogSample.Dialogs
{
    [Serializable]
    public class FlightDialog : LogDialog
    {
        string destination;
        public override async Task StartAsync(IDialogContext context)
        {
            LogActivity("StartAsync", context);
            await context.PostAsync("Welcome to FlightDialog");
            await context.PostAsync("Where do you want to go?");
            context.Wait(AskOrigin);
        }

        private async Task AskOrigin(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            LogActivity("AskOrigin", context);
            var reply = await result;
            destination = reply.Text;
            LogActivity(context, $"destination;{destination}");
            context.UserData.SetValue("destinationKey", destination);
            await context.PostAsync($"OK you want to go {destination}.Where is the origin place?");
            context.Wait(BookFlight);
        }

        private async Task BookFlight(IDialogContext context,IAwaitable<IMessageActivity> result)
        {
            LogActivity("BookFlight", context);
            var reply = await result;
            LogActivity(context, $"from;{reply.Text}");
            await context.PostAsync($"OK, booked the flight from {reply.Text} to {destination}. Have a nice trip :)");
            context.Done<object>(null);
        }
    }
}