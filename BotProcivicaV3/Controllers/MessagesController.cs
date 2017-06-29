using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
//using BotProcivicaV3.Translator;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using BotProcivicaV3.Translator;
using RestSharp;
using static BotProcivicaV3.Translator.IdentifyLanguage;
using AutoMapper;
//using BotProcivicaV3.Utilities;

namespace BotProcivicaV3
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {

        public string converstationText = "";
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            /*
            #region Set CurrentBaseURL and ChannelAccount
            // Get the base URL that this service is running at
            // This is used to show images
            string CurrentBaseURL =
                    this.Url.Request.RequestUri.AbsoluteUri.Replace(@"api/messages", "");

            // Create an instance of BotData to store data
            BotData objBotData = new BotData();

            // Instantiate a StateClient to save BotData            
            StateClient stateClient = activity.GetStateClient();

            // Use stateClient to get current userData
            BotData userData = await stateClient.BotState.GetUserDataAsync(
                activity.ChannelId, activity.From.Id);

            // Update userData by setting CurrentBaseURL and Recipient
            userData.SetProperty<string>("CurrentBaseURL", CurrentBaseURL);

            // Save changes to userData
            await stateClient.BotState.SetUserDataAsync(
                activity.ChannelId, activity.From.Id, userData);


            #endregion
            
    */
            if (activity.Type == ActivityTypes.Message)
            {
                IMessageActivity mensaje = activity.AsMessageActivity();

                Activity msj = (Activity)mensaje;
                msj.Recipient = msj.Recipient;
                msj.Type = "Message";

                if (activity.Text.Contains("CANCELAR")|| activity.Text.Contains("CANCEL"))
                {
                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    string response1 = ChatResponse.Cancel;
                    string response2 = ChatResponse.Step5Else;
                    Activity reply = activity.CreateReply(response1);
                    Activity reply2 = activity.CreateReply(response2);
                    await connector.Conversations.ReplyToActivityAsync(reply);
                    await connector.Conversations.ReplyToActivityAsync(reply2);
                    //Reinicia la conversación
                    activity.GetStateClient().BotState.DeleteStateForUser(activity.ChannelId, activity.From.Id);

                   
                }
                //else
                //{
                //    if (activity.Text.Contains("Adiós") || activity.Text.Contains("Adios"))
                //    {
                //        ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                //        string response1 = ChatResponse.Goodbye;
                //        //string response2 = ChatResponse.Step5Else;
                //        Activity reply = activity.CreateReply(response1);
                //        //Activity reply2 = activity.CreateReply(response2);
                //        await connector.Conversations.ReplyToActivityAsync(reply);
                //        //await connector.Conversations.ReplyToActivityAsync(reply2);
                //        //Reinicia la conversación
                //        activity.GetStateClient().BotState.DeleteStateForUser(activity.ChannelId, activity.From.Id);
                //    }
                //}
                else
                {
                    activity.Text = TranslatorHandler.DetectAndTranslate(activity);
                    await Conversation.SendAsync(activity, MakeRoot);
                }
            }
            else
            {
                await HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        internal static IDialog<object> MakeRoot()
        {
            try
            {

                return Chain.From(() => new ChatDialog());
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private async Task<Activity> HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                IConversationUpdateActivity conversationupdate = message;
                using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, message))
                {
                    var client = scope.Resolve<IConnectorClient>();
                    if (conversationupdate.MembersAdded.Any())
                    {
                        
                        var reply = message.CreateReply();
                        

                        foreach (var newMember in conversationupdate.MembersAdded)
                        {
                            if (newMember.Id != message.Recipient.Id)
                            {
                                reply.Text = "¡Hola!";

                                await client.Conversations.ReplyToActivityAsync(reply);
                            }
                        }
                    }
                }

            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }

}