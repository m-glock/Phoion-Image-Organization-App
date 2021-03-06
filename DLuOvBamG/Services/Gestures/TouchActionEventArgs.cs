﻿using System;
using Xamarin.Forms;

/**
 from https://docs.microsoft.com/en-us/samples/xamarin/xamarin-forms-samples/effects-touchtrackingeffect/
 */
namespace DLuOvBamG.Services.Gestures
{
    public class TouchActionEventArgs : EventArgs
    {
        public TouchActionEventArgs(long id, TouchActionType type, Point location, bool isInContact)
        {
            Id = id;
            Type = type;
            Location = location;
            IsInContact = isInContact;
        }

        public long Id { private set; get; }

        public TouchActionType Type { private set; get; }

        public Point Location { private set; get; }

        public bool IsInContact { private set; get; }
    }
}
