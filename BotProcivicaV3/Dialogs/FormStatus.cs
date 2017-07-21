using Microsoft.Bot.Builder.FormFlow;
using System;

namespace BotProcivicaV3.Dialogs
{
    [Serializable]
    public class FormStatus
    {
        [Prompt(new string[] { "" })]
        public string Checkid { get; set; }
        public static IForm<FormStatus> BuildForm()
        {
            string id = ChatResponse.id;
            string onemoment = ChatResponse.onemoment;
            return new FormBuilder<FormStatus>()
                .Field(nameof(Checkid), prompt: id)
                /***Desactiva el mensaje de espera en consulta de folio***/
                //.Message(onemoment)
                .AddRemainingFields()
                .Build();
        }
        //private static bool StatusEnabled(SuggestionStatus state) => !string.IsNullOrWhiteSpace(state.Checkid);
    }
}