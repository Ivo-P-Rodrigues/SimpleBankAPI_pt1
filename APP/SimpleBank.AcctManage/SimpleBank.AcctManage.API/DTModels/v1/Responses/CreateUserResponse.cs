﻿namespace SimpleBank.AcctManage.API.DTModels.v1.Responses
{
    public class CreateUserResponse
    {
        public Guid UserId { get; set; }
        public string CreatedAt { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string PasswordChangedAt { get; set; }
        public string Username { get; set; }


    }
}
