﻿namespace SimpleBank.AcctManage.UI.Blazor.Server.v1.Data.Responses
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
