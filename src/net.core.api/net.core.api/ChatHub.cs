using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using System.Text.Json;


namespace net.core.api
{
    public class ChatHub : Hub
    {
        protected IOpenAIService openAIService;
        public ChatHub(IOpenAIService openAIService)
        {
            this.openAIService = openAIService;
        }

        private async Task SendMessage(string message,bool error)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", JsonSerializer.Serialize(new MessagePayload()
            {
                Error = error,
                Message = message
            }));
        }
       
        public async Task SendMessageToCaller(string message)
        {
            try
            {
                var completionResult = this.openAIService.Completions.CreateCompletionAsStream(new CompletionCreateRequest()
                {
                    Prompt = message,
                    MaxTokens = 2000
                }, Models.TextDavinciV3);

                await foreach (var completion in completionResult)
                {
                    if (completion.Successful)
                    {
                        var resp = completion.Choices.FirstOrDefault()?.Text;
                        await SendMessage(resp, false);                       
                    }
                    else
                    {
                        if (completion.Error == null)
                        {
                            await SendMessage("Unknown Error, ending stream", true);
                            throw new Exception("Unknown Error");
                        }
                        await SendMessage($"{completion.Error.Code}: {completion.Error.Message}", true);                                         
                    }
                }
            } catch (Exception ex)
            {
                await SendMessage(ex.Message, true);            
            }
        }
    }

    class MessagePayload
    {
        public string? Message { get; set; }
        public bool Error { get; set; }
       
    }
}
