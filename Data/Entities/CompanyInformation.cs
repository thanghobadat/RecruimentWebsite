﻿using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class CompanyInformation
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int WorkerNumber { get; set; }
        public string ContactName { get; set; }
        public AppUser AppUser { get; set; }
        public List<CompanyImage> CompanyImages { get; set; }
        public List<CompanyBranch> CompanyBranches { get; set; }
        public List<Notification> Notifications { get; set; }
        public CompanyAvatar CompanyAvatar { get; set; }
        public CompanyCoverImage CompanyCoverImage { get; set; }
        public Recruitment Recruitment { get; set; }
        public List<Follow> Follows { get; set; }
        public List<Chat> Chats { get; set; }

    }
}
