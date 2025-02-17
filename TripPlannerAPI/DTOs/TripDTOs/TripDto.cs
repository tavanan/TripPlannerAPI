﻿using TripPlannerAPI.Controllers;
using TripPlannerAPI.DTOs.AccountDTOs;
using TripPlannerAPI.Models;

namespace TripPlannerAPI.DTOs.TripDTOs
{
    public class TripDto
    {
        public int tripId { get; set; }
        public DateTime date { get; set; }

        public DateTime creationDateTime { get; set; }

        public bool? isFavoriteForCurrentUser { get; set; }
        public bool? isJoinedByCurrentUser { get; set; }

        public bool? isCreatedByCurrentUser { get; set; }
        public string? type { get; set; }

        public string startTime
        {
            get
            {
                if (date == DateTime.MinValue) //For normal DateTimes, if you don't initialize them at all then they will match DateTime.MinValue, because it is a value type rather than a reference type.
                    return null;
                string prefixHrs = "";
                if (date.TimeOfDay.Hours < 10)
                    prefixHrs = "0";
                string prefixMins = "";
                if (date.TimeOfDay.Minutes < 10)
                    prefixMins = "0";
                return prefixHrs + date.TimeOfDay.Hours.ToString() + ":" + prefixMins + date.TimeOfDay.Minutes.ToString();
            }
        }
        public float totalTime { get; set; }
        public string description { get; set; }
        public float distance { get; set; }
        public List<Preference> preferences { get; set; }
        public List<Location> waypoints { get; set; }
        public List<UserDto> members { get; set; }
        public UserDto creator { get; set; }
        public bool isRecommended { get; set; }
        public List<Pin> pins { get; set; }

        public TripDto(Trip trip, User usr)
        {
            tripId = trip.tripId;
            date = trip.date;
            creationDateTime = trip.creationDateTime;
            type = trip.type;
            totalTime = trip.totalTime;
            distance = trip.distance;
            description = trip.description;
            waypoints = trip.waypoints;
            preferences = trip.preferences;
            isRecommended = trip.isRecommended;
            pins = trip.Pins;

            creator = new UserDto(trip.creator);
            members = trip.members.Select(u => new UserDto(u)).ToList();

            isJoinedByCurrentUser = trip.members.Any(u => u.Id == usr.Id);
            isCreatedByCurrentUser = trip.creator.Id == usr.Id;
            isFavoriteForCurrentUser = trip.FavoritedBy?.Any(u => u.Id == usr.Id);
        }

        public TripDto(Trip trip, User usr, bool _isFavoriteForCurrentUser)
        {
            tripId = trip.tripId;
            date = trip.date;
            creationDateTime = trip.creationDateTime;
            type = trip.type;
            totalTime = trip.totalTime;
            description = trip.description;
            waypoints = trip.waypoints;
            preferences = trip.preferences;
            isRecommended = trip.isRecommended;
            pins = trip.Pins;

            creator = new UserDto(usr);
            members = trip.members.Select(u => new UserDto(u)).ToList();

            isJoinedByCurrentUser = trip.members.Any(u => u.Id == usr.Id);
            isCreatedByCurrentUser = trip.creator.Id == usr.Id;
            isFavoriteForCurrentUser = _isFavoriteForCurrentUser;
        }
    }
}
