﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Tauchbolde.Common.Model
{
    public class Participant
    {
        public Guid Id { get; set; }

        [Display(Name = "Anlass ID")]
        [Required]
        public Guid EventId { get; set; }
        [Display(Name = "Anlass")]
        [Required]
        public Event Event { get; set; }

        [Display(Name = "Teilnehmer")]
        [Required]
        public ApplicationUser User { get; set; }

        [Display(Name = "Anzahl Personen")]
        [Required]
        public int CountPeople { get; set; }

        [Display(Name ="Notiz")]
        public string Note { get; set; }

        [Display(Name ="Status")]
        [Required]
        public ParticipantStatus Status { get; set; }

        [Display(Name="Buddy Team")]
        public string BuddyTeamName { get; set; }
    }
}