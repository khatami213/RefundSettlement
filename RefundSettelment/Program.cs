using System;
using System.ServiceProcess;
using System.Threading;

namespace RefundSettelment
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new RefundSettelment()
            //};
            //ServiceBase.Run(ServicesToRun);

            if (!Environment.UserInteractive)
                // running as service
                using (var service = new RefundSettelment())
                    ServiceBase.Run(service);
            else
            {
                // running as console app

                var a = new RefundSettelment();
                a.RefundSettelmentService();
                Thread.Sleep(1000);
                a.ProviderRefundSettlementService();
                Thread.Sleep(1000);
            }

        }
    }
}
