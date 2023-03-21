using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                Error = error ? 1: 0,
                Message = message
            }));
        }
       
        // TODO : check concurrent request from same client, if so cancel current gpt request
        public async Task SendMessageToCaller(string message)
        {
            try
            {


                var completionResult = this.openAIService.ChatCompletion.CreateCompletionAsStream(new ChatCompletionCreateRequest
                {
                    Messages = new List<ChatMessage>
                        {
                            new(StaticValues.ChatMessageRoles.System, message),
        
                        },
                    Model = Models.ChatGpt3_5Turbo,
                    MaxTokens = 150//optional
                });


                await foreach (var completion in completionResult)
                {
                    if (completion.Successful)
                    {
                        var resp = completion.Choices.First().Message.Content;
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

    public class MessagePayload
    {
        [JsonPropertyName("M")]
        public string? Message { get; set; }
        [JsonPropertyName("E")]
        public int Error { get; set; }
       
    }
}
