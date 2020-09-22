namespace logParser {
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System;
    using Autofac;

    public class Program {
        public static void Main (string[] args) {
            
            var container = ContainerConfig.Configure(args);

            using (var scope=container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IApplication>();
                app.Run();
            }
        }
    }
}