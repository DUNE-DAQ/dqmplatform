using DuneDaqMonitoringPlatform.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Data
{
    public class UserDbContext : IdentityDbContext
    {

        public UserDbContext(IConfiguration configuration) : base(new DbContextOptionsBuilder<UserDbContext>().UseNpgsql(   configuration["UserDbConnectionParameters:Server"] +
                                                                                                                            configuration["UserDbConnectionParameters:Port"] +
                                                                                                                            configuration["UserDbConnectionParameters:Database"] +
                                                                                                                            configuration["UserDbConnectionParameters:User"] +
                                                                                                                            configuration["UserDbConnectionParameters:Password"] +
                                                                                                                            configuration["UserDbConnectionParameters:Security"] +
                                                                                                                            configuration["UserDbConnectionParameters:Pooling"]
                                                                                                                        ).Options) { }

    }
}
