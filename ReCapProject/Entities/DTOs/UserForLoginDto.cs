﻿
using Core;

namespace Entities
{
    public class UserForLoginDto:IDto
    {
        public string Email {  get; set; }
        public string Password { get; set; }
    }
}
