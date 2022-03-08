﻿using System;

namespace Saas.LandingSignup.Web.Models.StateMachine
{
   
    public class OnboardingWorkflowState
    {
        public enum States
        {
            UserNameEntry,
            OrganizationNameEntry,
            OrganizationCategoryEntry,
            ServicePlanEntry,
            TenantDeploymentRequested,
            TenantDeploymentConfirmation,
            UsernameExistsError,
            Error
        };
        public enum Triggers
        {
            OnUserNamePosted,
            OnUserNameExists,
            OnOrganizationNamePosted,
            OnOrganizationCategoryPosted,
            OnServicePlanPosted,
            OnTenantDeploymentSuccessful,
            OnError,
            OnConfirmation
        }; 
        
        public States CurrentState { get; internal set; }

        public OnboardingWorkflowState(States state = States.UserNameEntry)
        {
            CurrentState = state;
        }

        public States Transition(Triggers trigger)
        {
            ChangeState(CurrentState, trigger);
            return CurrentState;
        }

        States ChangeState(States current, Triggers trigger) =>
            (current, trigger) switch
            {
                (States.UserNameEntry, Triggers.OnUserNamePosted) => States.OrganizationNameEntry,
                (States.UserNameEntry, Triggers.OnUserNameExists) => States.UsernameExistsError,
                (States.UserNameEntry, Triggers.OnError) => States.Error,
                (States.OrganizationNameEntry, Triggers.OnOrganizationNamePosted) => States.OrganizationCategoryEntry,
                (States.OrganizationNameEntry, Triggers.OnError) => States.Error,
                (States.OrganizationCategoryEntry, Triggers.OnOrganizationCategoryPosted) => States.ServicePlanEntry,
                (States.OrganizationCategoryEntry, Triggers.OnError) => States.Error,
                (States.ServicePlanEntry, Triggers.OnServicePlanPosted) => States.TenantDeploymentRequested,
                (States.ServicePlanEntry, Triggers.OnError) => States.Error,
                (States.TenantDeploymentRequested, Triggers.OnTenantDeploymentSuccessful) => States.TenantDeploymentConfirmation,
                (States.TenantDeploymentRequested, Triggers.OnError) => States.Error,
                _ => throw new NotSupportedException($"{current} has no transition on {trigger}")
            };
    }
}
