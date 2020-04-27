using Application.Abstractions.Services;
using HC.Patient.Model.Message;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.User;
using HC.Patient.Service.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace HC.Patient.Service.Services.Login
{
    public class SmsService : ISmsService
    {
        // private readonly ITwilioRestClient _client;
        private readonly IPatientInviteRepository _patientInviteRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _usersRepository;
        public SmsService( IConfiguration configuration, IUserRepository usersRepository, IPatientInviteRepository patientInviteRepository)
        {
            //_client = client;
            _patientInviteRepository = patientInviteRepository;
            _configuration = configuration;
            _usersRepository = usersRepository;
        }
        public string SendSms(MessageModel messageModel)
        {
            try
            {
                string messageSID = string.Empty;
                string TwililoAuthToken = _configuration["TwilioAuthTextSender:AUTH_TOKEN"];
                string TwililoAccoutSid = _configuration["TwilioAuthTextSender:ACCOUNT_SID"];
                string PhoneNumber = _configuration["TwilioAuthTextSender:PhoneNumber"];
                messageModel.From = PhoneNumber;
                if (!string.IsNullOrEmpty(TwililoAuthToken))
                {
                    TwilioClient.Init(TwililoAccoutSid, TwililoAuthToken);
                    var SendTo = new PhoneNumber(messageModel.To);
                    var message = MessageResource.Create(SendTo, from: new PhoneNumber(messageModel.From), body: messageModel.Message);
                    messageSID = message.Sid;
                }

                return messageSID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Generate and save otp in db.
        public string GenerateSMSPin(int userId)
        {
            #region Declaration
            string pin = string.Empty;
            string smsPin = string.Empty;
            bool isUpdated = false;
            #endregion Declaration

            #region Body
            try
            {
                //Generate SMS pin.
                pin = this.GeneratePIN();
                if (!string.IsNullOrEmpty(pin))
                {
                    // Save OTP in DB.
                     isUpdated = _patientInviteRepository.SaveOtp(userId, pin);
                    if (isUpdated)
                    {
                        smsPin = pin;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return smsPin;
            #endregion Body
        }

        /// <summary>
        /// Generate PIN
        /// </summary>
        /// <returns></returns>
        private string GeneratePIN()
        {
            #region Body
            string exactPin = GenerateNewPIN();

            //If PIN contains 0 at starting
            if (exactPin.IndexOf("0") == 0 || exactPin.Trim().StartsWith("0"))
            {
                //generate PIN again
                exactPin = GeneratePIN();
            }
            //Return PIN
            return exactPin;

            #endregion Body
        }
        /// <summary>
        /// Generate New PIN
        /// </summary>
        /// <returns></returns>
        private string GenerateNewPIN()
        {
            #region Declaration          
            var bytes = new byte[4];
            string pin = string.Empty;
            #endregion Declaration

            #region Body            
            //Create new random number
            var rng = RandomNumberGenerator.Create();

            ////Get bytes
            rng.GetBytes(bytes);

            //Convert number 
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000;

            //Get PIN with 5 digit
            string exactPin = String.Format("{0:D5}", random);

            //Return PIN
            return exactPin;

            #endregion Body
        }

    }
}