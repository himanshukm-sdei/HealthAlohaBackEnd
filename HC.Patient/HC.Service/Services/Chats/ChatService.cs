using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Chat;
using HC.Patient.Repositories.IRepositories.Chats;
using HC.Patient.Service.IServices.Chats;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HC.Patient.Service.Services.Chats
{
    public class ChatService : BaseService, IChatService
    {
        private JsonModel response;
        private readonly IChatRepository _chatRepository;
        private readonly IChatConnectedUserRepository _chatConnectedUserRepository;

        public ChatService(IChatRepository chatRepository, IChatConnectedUserRepository chatConnectedUserRepository)
        {
            response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
            _chatRepository = chatRepository;
            _chatConnectedUserRepository = chatConnectedUserRepository;
        }
        public async Task<JsonModel> ChatConnectedUser(ChatConnectedUserModel chatConnectedUserModel, TokenModel tokenModel)
        {
            ChatConnectedUser chatConnectedUser = _chatConnectedUserRepository.Get(a => a.UserId == chatConnectedUserModel.UserId);
            if (chatConnectedUser != null)
            {
                chatConnectedUser.ConnectionId = chatConnectedUserModel.ConnectionId;
                chatConnectedUser.UpdatedDate = DateTime.UtcNow;
                _chatConnectedUserRepository.Update(chatConnectedUser);
            }
            else
            {
                chatConnectedUser = new ChatConnectedUser();
                chatConnectedUser.ConnectionId = chatConnectedUserModel.ConnectionId;
                chatConnectedUser.UserId = chatConnectedUserModel.UserId;
                chatConnectedUser.CreatedDate = DateTime.UtcNow;
                _chatConnectedUserRepository.Create(chatConnectedUser);
            }

            await _chatConnectedUserRepository.SaveChangesAsync();
            return response = new JsonModel(null, StatusMessage.ChatConnectedEstablished, (int)HttpStatusCode.OK);
        }
        public async Task<JsonModel> SaveChat(ChatModel chatModel, TokenModel tokenModel)
        {
            Chat chat = null;
            if (chatModel.Id == 0)
            {
                chat = new Chat();
                chatModel.Message = CommonMethods.Encrypt(chatModel.Message);

                AutoMapper.Mapper.Map(chatModel, chat);
                chat.OrganizationID = tokenModel.OrganizationID;
                chat.CreatedBy = tokenModel.UserID;
                chat.CreatedDate = DateTime.UtcNow;
                chat.IsDeleted = false;
                chat.IsActive = true;
                chat.ChatDate = DateTime.UtcNow;
                _chatRepository.Create(chat);
                await _chatRepository.SaveChangesAsync();
                response = new JsonModel(null, StatusMessage.LocationSaved, (int)HttpStatusCode.OK);
            }
            return response;
        }
        public JsonModel GetChatHistory(ChatParmModel chatParmModel, TokenModel tokenModel)
        {
            List<ChatModel> chatModels = _chatRepository.GetChatHistory<ChatModel>(chatParmModel, tokenModel).ToList();
            if (chatModels != null && chatModels.Count() > 0)
            {
                chatModels.ForEach(a => { a.Message = CommonMethods.Decrypt(a.Message); });

                response = new JsonModel(chatModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(chatModels, chatModels);
            }
            return response;
        }
        public string GetConnectionId(int UserID)
        {
            return _chatConnectedUserRepository.Get(a => a.UserId == UserID).ConnectionId;
        }
        public bool SignOutChat(int UserID)
        {
            bool isConnected = true;
            ChatConnectedUser chatConnectedUser = _chatConnectedUserRepository.Get(a => a.UserId == UserID);
            if (chatConnectedUser != null)
            {
                chatConnectedUser.ConnectionId = string.Empty;
                chatConnectedUser.UpdatedDate = DateTime.UtcNow;
                _chatConnectedUserRepository.Update(chatConnectedUser);
                _chatConnectedUserRepository.SaveChanges();
                isConnected = false;
            }
            return isConnected;
        }
    }
}
