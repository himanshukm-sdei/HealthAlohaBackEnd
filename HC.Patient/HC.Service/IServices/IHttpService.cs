using Application.Dtos;
using HC.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstractions.Services
{
    public interface IHttpService
    {
        JsonModel HttpWebRequest(string Url, object classObject, bool isAuthenticationRequired, string token, string methodType);

        bool PushNotifications(PushNotificationsModel pushNotificationsModel, bool isAuthenticationRequired, string methodType);
    }
}
