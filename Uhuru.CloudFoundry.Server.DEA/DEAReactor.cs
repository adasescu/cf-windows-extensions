﻿using System;
using System.Globalization;
using Uhuru.NatsClient;
using Uhuru.Utilities;


namespace Uhuru.CloudFoundry.DEA
{
    public class DeaReactor : VcapReactor
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event SubscribeCallback OnRouterStart;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event SubscribeCallback OnHealthManagerStart;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event SubscribeCallback OnDeaStart;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event SubscribeCallback OnDeaStop;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event SubscribeCallback OnDeaStatus;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event SubscribeCallback OnDropletStatus;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event SubscribeCallback OnDeaDiscover;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event SubscribeCallback OnDeaFindDroplet;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event SubscribeCallback OnDeaUpdate;

        

        public DeaReactor()
        {

        }

        public string Uuid
        {
            get;
            set;
        }

        //Runs the DeaReactor in a blocking mode
        public override void Start()
        {
            base.Start();
            

            NatsClient.Subscribe("dea.status", OnDeaStatus);
            NatsClient.Subscribe("droplet.status", OnDropletStatus);
            NatsClient.Subscribe("dea.discover", OnDeaDiscover);
            NatsClient.Subscribe("dea.find.droplet", OnDeaFindDroplet);
            NatsClient.Subscribe("dea.update", OnDeaUpdate);

            NatsClient.Subscribe("dea.stop", OnDeaStop);
            NatsClient.Subscribe(String.Format(CultureInfo.InvariantCulture, Strings.NatsMessageDeaStart, Uuid), OnDeaStart);

            NatsClient.Subscribe("router.start", OnRouterStart);
            NatsClient.Subscribe("healthmanager.start", OnHealthManagerStart);
        }

        public void SendDeaHeartbeat(string message)
        {
            NatsClient.Publish("dea.heartbeat", null, message);
        }

        public void SendDeaStart(string message)
        {
            NatsClient.Publish("dea.start", null,  message);
        }

        public void SendDropletExited(string message)
        {
            NatsClient.Publish("droplet.exited", null, message);
            Logger.Debug(Strings.SentDropletExited, message);
        }

        public void SendRouterRegister(string message)
        {
            NatsClient.Publish("router.register", null, message);
        }

        public void SendRouterUnregister(string message)
        {
            NatsClient.Publish("router.unregister", null, message);
        }
    }
}
