using HC.Common;
using HC.Common.Options;
using HC.Model;
using HC.Patient.Model.Chat;
using HC.Patient.Model.Common;
using HC.Patient.Model.Message;
using Microsoft.Extensions.Logging;
using HC.Patient.Service.IServices.Chats;
using HC.Patient.Service.IServices.Message;
using HC.Patient.Service.Token.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace HC.Patient.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;        
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;
        private readonly JwtIssuerOptions _jwtOptions;

        public ChatHub(IChatService chatService, IMessageService messageService, ITokenService tokenService, ILoggerFactory loggerFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _chatService = chatService;
            _messageService = messageService;
            _tokenService = tokenService;

            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
            _logger = loggerFactory.CreateLogger<ChatHub>();
        }

        /// <summary>
        /// this method is used for connect the user with hub and save the connectionid into database
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Connect(int userId)
        {
            try
            {
                ChatConnectedUserModel chatConnectedUserModel = new ChatConnectedUserModel();
                TokenModel tokenModel = CommonMethods.GetTokenDataModel(Context.GetHttpContext());
                chatConnectedUserModel.ConnectionId = Context.ConnectionId;
                chatConnectedUserModel.UserId = userId;
                await _chatService.ChatConnectedUser(chatConnectedUserModel, tokenModel);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// he SendMessage method can be called by any connected client.
        /// It sends the received message to "all clients".
        /// SignalR code is asynchronous to provide maximum scalability.
        /// </summary>        
        /// <param name="message"></param>        
        /// <param name="fromUserId"></param>
        /// <param name="toUserId"></param>
        /// <returns></returns>
        public async Task SendMessage(string message, int fromUserId, int toUserId)
        {
            try
            {
                ChatModel chatModel = new ChatModel();
                TokenModel tokenModel = CommonMethods.GetTokenDataModel(Context.GetHttpContext());
                string connectionId = _chatService.GetConnectionId(toUserId);
                chatModel.ChatDate = DateTime.UtcNow;
                chatModel.FromUserId = fromUserId;
                chatModel.ToUserId = toUserId;
                chatModel.IsSeen = false;
                chatModel.Message = message;
                await _chatService.SaveChat(chatModel, tokenModel);

                if (connectionId != null)
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", message, fromUserId);
                //else
                //    await Clients.All.SendAsync("ReceiveMessage", message, fromUserId);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// this is use for message count notification by signalR for real time dynamic count for new message recieved
        /// </summary>
        /// <param name="forStaff"></param>
        /// <returns></returns>
        public async Task MessageCountRequest(bool forStaff)
        {
            HttpContext httpContext = Context.GetHttpContext();
            MessagesInfoFromSignalRModel messagesInfoFromSignalRModel = _messageService.ExecuteFunctions<MessagesInfoFromSignalRModel>(() => _messageService.GetMessagesInfoFromSignalR(forStaff, httpContext));
            await Clients.All.SendAsync("MessageCountResponse", messagesInfoFromSignalRModel);
        }

        public async Task NotificationRequest(bool forStaff)
        {
            HttpContext httpContext = Context.GetHttpContext();
            TokenModel token = CommonMethods.GetTokenDataModel(httpContext);
            NotificationModel notificationModel = _tokenService.GetLoginNotification(token);
            await Clients.All.SendAsync("NotificationResponse", notificationModel);
        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }
    }
}
