using HC.Model;
using HC.Patient.Model.Chat;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HC.Patient.Service.IServices.Chats
{
    public interface IChatService: IBaseService
    {
        Task<JsonModel> ChatConnectedUser(ChatConnectedUserModel chatConnectedUserModel, TokenModel tokenModel);
        string GetConnectionId(int UserID);
        bool SignOutChat(int UserID);
        Task<JsonModel> SaveChat(ChatModel chatModel, TokenModel tokenModel);
        JsonModel GetChatHistory(ChatParmModel chatParmModel, TokenModel tokenModel);
    }
}
