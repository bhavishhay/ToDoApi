﻿namespace ToDoApi.Models.DTOs
{
    public class UpdateToDoDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}
