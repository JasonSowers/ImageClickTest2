using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot_Application12.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var response = activity.CreateReply();

            IMessageActivity reply = context.MakeMessage();
            var url = $"https://stackoverflow.com/questions/tagged/botframework";
            // Create the images to show
            CardAction tapAction = new CardAction(ActionTypes.OpenUrl, value: url, title: url);
            List<CardImage> cardImages = new List<CardImage>() { new CardImage() { Url = @"someImageUrl", Tap = tapAction } };

            // Add the attachemnt and send the reply message
            reply.Attachments = new List<Attachment>() { new HeroCard() { Images = cardImages }.ToAttachment() };
            await context.PostAsync(reply);


            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            response.Text = $"You sent {activity.Text} which was {length} characters {response.ChannelData}";
            
            // return our reply to the user
            await context.PostAsync(response);

            context.Wait(MessageReceivedAsync);
        }
    }


    
}