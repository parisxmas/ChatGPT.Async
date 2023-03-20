using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using System.Text.RegularExpressions;

namespace net.core.api
{
    public class ChatHub : Hub
    {
        protected IOpenAIService openAIService;
        public ChatHub(IOpenAIService openAIService)
        {
            this.openAIService = openAIService;
        }
        public async Task SendMessageToCaller(string message)
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
                    
                    await Clients.Caller.SendAsync("ReceiveMessage", resp);

                }
            }            
        }
    }

    class MessagePayload
    {
        public string? Message { get; set; }
        public bool Error { get; set; }
       
    }
}
