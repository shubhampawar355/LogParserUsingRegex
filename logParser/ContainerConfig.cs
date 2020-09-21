using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace logParser
{
    public static class ContainerConfig
    {
        public static IContainer Configure(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Application>().As<IApplication>();

            builder.RegisterType<UserInput>().As<IUserInput>()
                .WithParameter(new TypedParameter(typeof(string[]),args)).SingleInstance();
            builder.RegisterType<LogParser>().As<ILogParser>();
            builder.RegisterType<FileReader>().As<IFileReader>();
            builder.RegisterType<FileWriter>().As<IFileWriter>();
            builder.RegisterType<Log>().As<ILog>();

            return builder.Build();
        }
    }
}
