﻿namespace SaaSOvation.IdentityAccess.Domain.Model.Identity
{
    using System;
    using SaaSOvation.Common.Domain.Model;

    public class RegistrationInvitation
    {
        public RegistrationInvitation(
            Identity<Tenant> tenantId,
            string invitationId,
            string description,
            DateTime startingOn,
            DateTime until)
        {
            this.Description = description;
            this.InvitationId = invitationId;
            this.StartingOn = startingOn;
            this.TenantId = tenantId;
            this.Until = until;
        }

        public RegistrationInvitation(Identity<Tenant> tenantId, string invitationId, string description)
            : this(tenantId, invitationId, description, DateTime.MinValue, DateTime.MinValue)
        {
        }

        public string Description { get; private set; }

        public string InvitationId { get; private set; }

        public DateTime StartingOn { get; private set; }

        public Identity<Tenant> TenantId { get; private set; }

        public DateTime Until { get; private set; }

        public bool IsAvailable()
        {
            bool isAvailable = false;

            if (this.StartingOn == DateTime.MinValue && this.Until == DateTime.MinValue)
            {
                isAvailable = true;
            }
            else
            {
                long time = (new DateTime()).Ticks;
                if (time >= this.StartingOn.Ticks && time <= this.Until.Ticks)
                {
                    isAvailable = true;
                }
            }

            return isAvailable;
        }

        public bool IsIdentifiedBy(String invitationIdentifier)
        {
            bool isIdentified = this.InvitationId.Equals(invitationIdentifier);

            if (!isIdentified && this.Description != null)
            {
                isIdentified = this.Description.Equals(invitationIdentifier);
            }

            return isIdentified;
        }

        public RegistrationInvitation OpenEnded()
        {
            this.StartingOn = DateTime.MinValue;
            this.Until = DateTime.MinValue;
            return this;
        }

        public RegistrationInvitation RedefineAs()
        {
            this.StartingOn = DateTime.MinValue;
            this.Until = DateTime.MinValue;
            return this;
        }

        public InvitationDescriptor ToDescriptor()
        {
            return new InvitationDescriptor(
                this.TenantId,
                this.InvitationId,
                this.Description,
                this.StartingOn,
                this.Until);
        }

        public RegistrationInvitation WillStartOn(DateTime date)
        {
            if (this.Until != DateTime.MinValue)
            {
                throw new InvalidOperationException("Cannot set starting-on date after until date.");
            }

            this.StartingOn = date;

            // temporary if Until set properly follows, but
            // prevents illegal state if Until set doesn't follow
            this.Until = new DateTime(date.Ticks + 86400000);

            return this;
        }

        public RegistrationInvitation LastingUntil(DateTime date)
        {
            if (this.StartingOn == DateTime.MinValue)
            {
                throw new InvalidOperationException("Cannot set until date before setting starting-on date.");
            }

            this.Until = date;

            return this;
        }

        public override bool Equals(object anotherObject)
        {
            bool equalObjects = false;

            if (anotherObject != null && this.GetType() == anotherObject.GetType())
            {
                RegistrationInvitation typedObject = (RegistrationInvitation) anotherObject;
                equalObjects =
                    this.TenantId.Equals(typedObject.TenantId) &&
                    this.InvitationId.Equals(typedObject.InvitationId);
            }

            return equalObjects;
        }

        public override int GetHashCode()
        {
            int hashCodeValue =
                + (6325 * 233)
                + this.TenantId.GetHashCode()
                + this.InvitationId.GetHashCode();

            return hashCodeValue;
        }

        public override string ToString()
        {
            return "RegistrationInvitation ["
                    + "tenantId=" + TenantId
                    + ", description=" + Description
                    + ", invitationId=" + InvitationId
                    + ", startingOn=" + StartingOn
                    + ", until=" + Until + "]";
        }
    }
}
