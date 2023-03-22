using System.Text;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using System.Text.Json;
using System.Text.Json.Serialization;
using net.core.business.Caching.Base;
using net.core.business.Services.ChatServiceFolder;
using net.core.business.Services.SendersServiceFolder;
using net.core.data.Base;
using net.core.data.Services.ChatService;
using net.core.data.StoredProcedureModels;

namespace net.core.api
{
    public class ChatHub : Hub
    {
        protected IOpenAIService openAIService;
        private IChatService _chatService;
        private ISenderService _senderService;
        private ICacheService _cacheService;

        private static Guid? UserId;
        private static Guid? ChatGPTId;

        public ChatHub(IOpenAIService openAIService,
            IChatService chatService,
            ISenderService senderService,
            ICacheService cacheService)
        {
            this.openAIService = openAIService;
            _chatService = chatService;
            _senderService = senderService;
            _cacheService = cacheService;
        }


        public override async Task OnConnectedAsync()
        {
            var userIds = await SetUserIds();
            UserId = userIds.Item1;
            ChatGPTId = userIds.Item2;
            
            var prevChats = await _chatService.GetChats(new GetChatsRequest());
            prevChats.Reverse();
            foreach (var prevChat in prevChats)
            {
                await SendInitialMessage(prevChat.Text, prevChat.SenderId == UserId ? "User" : "System");
            }
        }
//InitialMessages

        private async Task SendInitialMessage(string message, string sender)
        {
            await Clients.Caller.SendAsync("InitialMessage", JsonSerializer.Serialize(new InitialMessage()
            {
                Message = message,
                Sender = sender
            }));
        }

        private async Task SendMessage(string message, bool error)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", JsonSerializer.Serialize(new MessagePayload()
            {
                Error = error ? 1 : 0,
                Message = message
            }));
        }


        private Task<(Guid?, Guid?)> SetUserIds()
        {
            return Task.Run(async () =>
            {
                var users = _cacheService.Get<List<SPGetSendersResponse>>(GLOBALS.SENDERS_CACHE_KEY);

                if (users == null || users.Count == 0)
                {
                    users = await _senderService.GetSenders();
                    _cacheService.Set(GLOBALS.SENDERS_CACHE_KEY, users);
                }

                var uId = users.SingleOrDefault(_ => _.Name == GLOBALS.SENDERS_USER)?.Id.Value;
                var chatGptId = users.SingleOrDefault(_ => _.Name == GLOBALS.SENDERS_SYSTEM)?.Id.Value;

                return (uId, chatGptId);
            });
        }

        // TODO : check concurrent request from same client, if so cancel current gpt request
        public async Task SendMessageToCaller(string message)
        {
            try
            {
                // var allIds = SetUserIds();
                var completionResult = this.openAIService.ChatCompletion.CreateCompletionAsStream(
                    new ChatCompletionCreateRequest
                    {
                        Messages = new List<ChatMessage>
                        {
                            new(StaticValues.ChatMessageRoles.System, message),
                        },
                        Model = Models.ChatGpt3_5Turbo,
                        MaxTokens = 150 //optional
                    });

                await _chatService.InsertMessage(new InsertMessageRequest()
                {
                    SenderId = UserId.Value,
                    Text = message
                });

                int nullCount = 0;
                StringBuilder chatGptResponse = new StringBuilder();
                
                await foreach (var completion in completionResult)
                {
                    if (completion.Successful)
                    {
                        var resp = completion.Choices.First().Message.Content;
                        if (String.IsNullOrEmpty(resp))
                        {
                            nullCount += 1;
                            if (nullCount == 2)
                            {
                                await _chatService.InsertMessage(new InsertMessageRequest()
                                {
                                    SenderId = ChatGPTId.Value,
                                    Text = chatGptResponse.ToString()
                                });
                            }
                        }
                        else
                        {
                            chatGptResponse.Append(resp);
                        }
                        
                        await SendMessage(resp, false);
                    }
                    else
                    {
                        if (completion.Error == null)
                        {
                            await SendMessage("Unknown Error, ending stream", true);
                            throw new Exception("Unknown Error");
                        }

                        string errorMessage = $"{completion.Error.Code}: {completion.Error.Message}";
                        await SendMessage(errorMessage, true);

                        await _chatService.InsertMessage(new InsertMessageRequest()
                        {
                            SenderId = ChatGPTId.Value,
                            Text = errorMessage
                        });

                        throw new Exception(errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                await SendMessage(ex.Message, true);
            }
        }
    }

    public class MessagePayload
    {
        [JsonPropertyName("M")] public string? Message { get; set; }
        [JsonPropertyName("E")] public int Error { get; set; }
    }

    public class InitialMessage
    {
        [JsonPropertyName("M")] public string? Message { get; set; }
        [JsonPropertyName("S")] public string? Sender { get; set; }
    }
}