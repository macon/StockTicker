﻿//-------------------------------------------------------------------------------
// <copyright file="AuthenticationViewModel.cs" company="bbv Software Services AG">
//   Copyright (c) 2012
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace StockTicker.Authentication
{
    using System.Linq;

    using Caliburn.Micro;

    using StockTicker.Actions;

    // NOTE: Holds the whole authentication wizard. Only one step can be active at the time. Each step which is finished is deactivated and removed from the conductors items collection
    internal sealed class AuthenticationViewModel : Conductor<IAuthenticationStep>.Collection.OneActive, IAuthenticationViewModel
    {
        private readonly IAuthenticationStepFactory authenticationStepFactory;

        public AuthenticationViewModel(IAuthenticationStepFactory authenticationStepFactory, IBusyIndicationViewModel busyIndication)
        {
            this.authenticationStepFactory = authenticationStepFactory;
            this.BusyIndication = busyIndication;

            this.DisplayName = Authentication.AuthenticationTitle;
        }

        public IBusyIndicationViewModel BusyIndication { get; private set; }

        // NOTE: The next method is hooked up by the Next button and simply takes the next item from the items collection and activates it. Previously activated item is deactivated.
        public void Next()
        {
            this.DeactivateItem(this.ActiveItem, true);

            if (this.ActiveItem == null)
            {
                this.TryClose();
            }
        }

        // NOTE: Upon initialization all steps are loaded into the conductor. But only the first is activated
        protected override void OnInitialize()
        {
            base.OnInitialize();

            this.Items.AddRange(this.authenticationStepFactory.CreateSteps());

            this.ActivateItem(this.Items.First());
        }
    }
}