using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTest.Helper
{
    public class MQConnection
    {
        private string vhost = string.Empty;
        private IConnection connection = null;
        private MQConfig config = null;
        public static UTF8Encoding UTF8 { get; set; } = new UTF8Encoding(false);

        public MQConnection(MQConfig config, string vhost)
        {
            this.config = config;
            this.vhost = vhost;
        }

        public IConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    ConnectionFactory factory = new ConnectionFactory
                    {
                        AutomaticRecoveryEnabled = true,
                        UserName = this.config.UserName,
                        Password = this.config.Password,
                        HostName = this.config.HostName,
                        VirtualHost = this.vhost,
                        Port = this.config.Port
                    };
                    connection = factory.CreateConnection();
                }
                return connection;
            }
        }
    }
}
