using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace MultiDialogSample.Dialogs
{
    [Serializable]
    public class HotelDialog : LogDialog
    {
        string hotelLocation;

        public override async Task StartAsync(IDialogContext context)
        {
            LogActivity("StartAsync", context);
            //Retreive UserData
            context.UserData.TryGetValue("destinationKey", out hotelLocation);

            await context.PostAsync("Welcome to HotelDialog");
            if (hotelLocation == null)
            {
                await context.PostAsync("Where do you want to go?");
                context.Wait(AskDuration);
            }
            else
            {
                LogActivity(context,$"hotelLocation;{hotelLocation}");
                await context.PostAsync("How long do you want to stay?");
                context.Wait(BookHotel);
            }
        }

        private async Task AskDuration(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            LogActivity("AskDuration", context);
            var reply = await result;
            hotelLocation = reply.Text;
            LogActivity(context,$"hotelLocation;{hotelLocation}");
            await context.PostAsync($"OK, you want to go {hotelLocation}.How long do you want to stay?");
            context.Wait(BookHotel);
        }

        private async Task BookHotel(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            LogActivity("BookHotel", context);
            var reply = await result;
            LogActivity(context,$"duration;{reply.Text}");
            await context.PostAsync($"Booked room for people in the {hotelLocation} for {reply.Text} days.");
            context.Done<object>(hotelLocation);
        }
    }
}