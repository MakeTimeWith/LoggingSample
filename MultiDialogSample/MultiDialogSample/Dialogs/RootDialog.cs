﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace MultiDialogSample.Dialogs
{
    [Serializable]
    public class RootDialog : LogDialog
    {
        private const string FlightMenu = "Book Flight";
        private const string HotelMenu = "Book Hotel";
        private List<string> mainMenuList = new List<string>() {FlightMenu,HotelMenu };
        private string location;

        public override async Task StartAsync(IDialogContext context)
        {
            LogActivity("StartAsync", context);
            await context.PostAsync("Welcome to Root Dialog");
            context.Wait(MessageReceiveAsync);
        }

        private async Task MessageReceiveAsync (IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            LogActivity("MessageReceiveAsync", context);
            var reply = await result;
            LogActivity(context, reply.Text);
            if(reply.Text.ToLower().Contains("help"))
            {
                await context.PostAsync("You can implement help menu here");
            }
            else
            {
                await ShowMainmenu(context);
            }
        }

        private async Task ShowMainmenu(IDialogContext context)
        {
            LogActivity("ShowMainmenu", context);
            //Show menues
            PromptDialog.Choice(context,this.CallDialog,this.mainMenuList,"What do you want to do?");
        }

        private async Task CallDialog(IDialogContext context, IAwaitable<string> result)
        {
            LogActivity("CallDialog", context);
            //This method is resume after user choise menu
            var selectedMenu = await result;
            LogActivity(context, selectedMenu.ToString());
            switch (selectedMenu)
            {
                case FlightMenu:
                    //Call child dialog without data
                    context.Call(new FlightDialog(),ResumeAfterDialog);
                    break;
                case HotelMenu:
                    //Call child dialog with data
                    context.Call(new HotelDialog(), ResumeAfterDialog);
                    break;
            }
        }

        private async Task ResumeAfterDialog(IDialogContext context,IAwaitable<object> result)
        {
            LogActivity("ResumeAfterDialog", context);
            //Resume this method after child Dialog is done.
            var test = await result;
            if(test != null)
            {
                location = test.ToString();
            }
            else
            {
                location = null;
            }
            //await this.ShowMainmenu(context); // If you want to show main menu when the dialog is done, please comment out this line.
            context.Wait(MessageReceiveAsync);
        }
    }
}