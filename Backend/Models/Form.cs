using System;
using System.Collections.Generic;

namespace Backend.Models
{
    public class Form
    {
        public Guid Id { get; set; }
        public bool IsPassword { get; set; }
        public string? Password { get; set; }
        public string? Description { get; set; }
        public int ExpiredDays { get; set; }
        public List<string>? FileNames { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class FormResponse
    {
        public bool IsPassword { get; set; }
        public string Description { get; set; }
    }

    public class CreateFormRequest
    {
        public bool IsPassword { get; set; }
        public string? Password { get; set; }
        public string? Description { get; set; }
        public int ExpiredDays { get; set; }
        public List<string> FileNames { get; set; }
    }

    public class ValidatePasswordRequest
    {
        public string? Password { get; set; }
    }
} 