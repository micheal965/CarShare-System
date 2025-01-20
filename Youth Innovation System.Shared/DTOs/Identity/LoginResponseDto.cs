﻿using System.ComponentModel.DataAnnotations;

namespace Youth_Innovation_System.DTOs.Identity
{
    public class LoginResponseDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
