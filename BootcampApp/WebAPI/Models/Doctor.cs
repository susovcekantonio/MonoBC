﻿namespace WebAPI.Models
{
    public class Doctor
    {
        public Guid Id {  get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Specialty { get; set; }
    }
}
