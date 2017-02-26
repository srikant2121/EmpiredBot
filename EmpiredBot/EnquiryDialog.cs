using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace EmpiredBot
{
    [Serializable]
    public class EnquiryDialog : IDialog<object>
    {

        protected MojoBillEnquiry mojoBillEnquiry;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public virtual async  Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var message = await activity;
            mojoBillEnquiry = new MojoBillEnquiry();

            if (message.Text.Contains("Mojo Bill"))
            {
                PromptDialog.Text(
                       context: context,
                       resume: MojoEnquiryCustomerNameHandler,
                       prompt: "Type Customer name",
                       retry: "Sorry, I don't understand that."
                   );
            }
            else if (message.Text == "No")
            {

            }
            else
            {
                await context.PostAsync("Hi, I am an empired Bot, type 'Mojo Bill' to start the conversation.");
                context.Wait(MessageReceivedAsync);
            }
        }
        public virtual async Task MojoEnquiryCustomerNameHandler(IDialogContext context, IAwaitable<string> argument)
        {
            var customerName = await argument;
            mojoBillEnquiry.Name = customerName;
            PromptDialog.Text(
                context: context,
                resume: MojoEnquiryYearHandler,
                prompt: "Type Year(e.g 2016)",
                retry: "Sorry, I don't understand that."
            );
        }

         
        public virtual async Task MojoEnquiryYearHandler(IDialogContext context, IAwaitable<string> argument)
        {
            var period = await argument;
            mojoBillEnquiry.Year = period;
            PromptDialog.Text(
                context: context,
                resume: MojoBillResult,
                prompt: "Type month (e.g January)",
                retry: "Sorry, I don't understand that."
            );
        }


        public async Task MojoBillResult(IDialogContext context, IAwaitable<string> argument)
        {
            var period = await argument;
            mojoBillEnquiry.Month = period;
            await context.PostAsync($@"Thank you for your enquiry, your bill for the period below will be emailed to you shortly.
                                    {Environment.NewLine}Mojo Bill Request details:
                                    {Environment.NewLine}Customer Name: {mojoBillEnquiry.Name},
                                    {Environment.NewLine}Year: {mojoBillEnquiry.Year},
                                    {Environment.NewLine}Month: {mojoBillEnquiry.Month}");


            //Get montly bill from CRM 
        }
    }
}