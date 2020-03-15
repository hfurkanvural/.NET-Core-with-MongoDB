using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace docker_api
{
    public class ServerConfig
    {
       public MongoDBConfig MongoDB { get; set; } = new MongoDBConfig();
    }
}
