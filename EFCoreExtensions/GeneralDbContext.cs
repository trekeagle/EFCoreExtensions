using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace CodeTreker.Utils.DB
{
    public class GeneralDbContext : DbContext
    {

        private string _connectonStrName;

        public GeneralDbContext(string connectonStrName = null)
        {
            _connectonStrName = connectonStrName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (string.IsNullOrEmpty(_connectonStrName))
            {
                _connectonStrName = "DefaultConnection";
            }
            options.UseSqlServer(ConfigManager.GetInstance().Configuration.GetConnectionString(_connectonStrName));
        }

    }
}
