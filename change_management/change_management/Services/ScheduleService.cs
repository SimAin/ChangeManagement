using System;
using System.Collections.Generic;
using System.Linq;
using change_management.Models;
using change_management.Services;

namespace change_management.Services {
    public class ScheduleService {
        public ScheduleService () { }

        #region Ordering change

        public List<Change> scheduleChanges (List<Change> changeList) {

            List<Change> orderedCritical = new List<Change> ();
            List<Change> orderedNonCritical = new List<Change> ();

            var critical = changeList.Where (c => c.criticality == true).ToList ();
            if (critical.Count () > 1) {
                orderedCritical = SplitList (critical);
            } else {
                orderedCritical = critical;
            }
            var nonCritical = changeList.Where (c => c.criticality == false).ToList ();
            if (nonCritical.Count () > 1) {
                orderedNonCritical = SplitList (nonCritical);
            } else {
                orderedNonCritical = nonCritical;
            }

            orderedCritical.AddRange (orderedNonCritical);
            List<Change> completeOrder = orderedCritical;
            return orderedCritical.ToList ();
        }

        private List<Change> SplitList (List<Change> unOrdered) {
            if (unOrdered.Count <= 1)
                return unOrdered;

            List<Change> listOne = new List<Change> ();
            List<Change> listTwo = new List<Change> ();

            for (int i = 0; i < (unOrdered.Count / 2); i++) {
                var di = (int) DateTime.Now.Subtract (unOrdered[i].deadline).TotalDays;
                unOrdered[i].laxity = di - unOrdered[i].processingTime;
                listOne.Add (unOrdered[i]);
            }
            for (int i = (unOrdered.Count / 2); i < unOrdered.Count; i++) {
                var di = (int) DateTime.Now.Subtract (unOrdered[i].deadline).TotalDays;
                unOrdered[i].laxity = di - unOrdered[i].processingTime;
                listTwo.Add (unOrdered[i]);
            }

            listOne = SplitList (listOne);
            listTwo = SplitList (listTwo);
            return Merge (listOne, listTwo);
        }

        private List<Change> Merge (List<Change> listOne, List<Change> listTwo) {
            List<Change> result = new List<Change> ();

            while (listOne.Count > 0 || listTwo.Count > 0) {
                if (listOne.Count > 0 && listTwo.Count > 0) {
                    if (listOne.First ().laxity > listTwo.First ().laxity) {

                        result.Add (listOne.First ());
                        listOne.Remove (listOne.First ());

                    } else if (listOne.First ().laxity < listTwo.First ().laxity) {

                        result.Add (listTwo.First ());
                        listTwo.Remove (listTwo.First ());

                    } else {
                        if (listOne.First ().priority < listTwo.First ().priority) {

                            result.Add (listOne.First ());
                            listOne.Remove (listOne.First ());

                        } else if (listOne.First ().priority > listTwo.First ().priority) {

                            result.Add (listTwo.First ());
                            listTwo.Remove (listTwo.First ());
                        } else {
                            if (listOne.First ().createdDate > listTwo.First ().createdDate) {

                                result.Add (listOne.First ());
                                listOne.Remove (listOne.First ());

                            } else if (listOne.First ().createdDate <= listTwo.First ().createdDate) {

                                result.Add (listTwo.First ());
                                listTwo.Remove (listTwo.First ());
                            }
                        }
                    }
                } else if (listOne.Count > 0) {
                    result.Add (listOne.First ());
                    listOne.Remove (listOne.First ());
                } else if (listTwo.Count > 0) {
                    result.Add (listTwo.First ());
                    listTwo.Remove (listTwo.First ());
                }
            }
            return result;
        }

        #endregion

        public List<Change> calculateDeadlineStatus (List<Change> changeList) {

            calculateUserBookedDays (changeList);
            foreach (var member in SessionService.loggedInTeam.teamMembers) {
                var userWaitTime = member.userBookedDays;
                var userChanges = changeList.Where (c => c.userResponsible.userID == member.user.userID).ToList ();
                foreach (var item in userChanges) {
                    item.deadlineStatus = 99;
                    calculateDeadlineStatus (item, userWaitTime);
                }
            }


            calculateUserPlannedDays(changeList);

            //var shortestPickUpTime = calculateTeamMemberWithLeastPlanned (changeList);
            var unassignedChanges = changeList.Where (c => c.userResponsible.userID == 0).ToList ();

            foreach (var item in unassignedChanges) {
                item.deadlineStatus = 99;
                calculateDeadlineStatus (item, calculateTeamMemberWithLeastPlanned(item).userLowestDays);
            }

            return changeList;
        }

        public void calculateDeadlineStatus (Change change, int userWaitTime) {

            if (change.status == "In progress") {
                //It is in progress therefore it wil have a start date. 
                var d = change.processingTime - Math.Abs (((int) (change.startedDate ?? DateTime.Now).Subtract (DateTime.Now).TotalDays));
                int daysRemaining = (d < 0) ? 0 : d;

                deadlineStatusSetter (change, daysRemaining);
            }

            if (change.status == "Not Started") {

                deadlineStatusSetter (change, (userWaitTime + change.processingTime));
                userWaitTime = userWaitTime + change.processingTime;
            }
        }

        public Change deadlineStatusSetter (Change change, int days) {

            int twentyP = (int) Math.Ceiling (change.processingTime * 0.2);

            if (DateTime.Now.AddDays (days).Date < change.deadline.AddDays (-twentyP).Date) {
                change.deadlineStatus = 1;
            }
            if ((DateTime.Now.AddDays (days).Date >= change.deadline.AddDays (-twentyP).Date) && (DateTime.Now.AddDays (days).Date < change.deadline.Date)) {
                change.deadlineStatus = 2;
            }
            if (DateTime.Now.AddDays (days).Date == change.deadline.Date) {
                change.deadlineStatus = 3;
            }
            if ((DateTime.Now.AddDays (days).Date <= change.deadline.AddDays (twentyP).Date) && (DateTime.Now.AddDays (days).Date > change.deadline.Date)) {
                change.deadlineStatus = 4;
            }
            if ((DateTime.Now.AddDays (days).Date >= change.deadline.AddDays (twentyP).Date) && (DateTime.Now.AddDays (days).Date > change.deadline.Date)) {
                change.deadlineStatus = 5;
            }

            return change;
        }

        public void calculateUserBookedDays (List<Change> changeList) {
            foreach (var member in SessionService.loggedInTeam.teamMembers) {
                member.userBookedDays = 0;
                var usersInProgChanges = changeList.Where (c => c.userResponsible.userID == member.user.userID && c.status == "In progress").ToList ();
                foreach (var change in usersInProgChanges) {
                    int daysRemaining = 0;
                    int daysCheck =  -((int) (change.startedDate ?? DateTime.Now).Subtract (DateTime.Now).TotalDays);
                    if(daysCheck < change.processingTime){
                        daysRemaining = change.processingTime - daysCheck;
                    }
                    
                    member.userBookedDays = member.userBookedDays + daysRemaining;
                }
            }
        }

        public void calculateUserPlannedDays (List<Change> changeList) {
            foreach (var member in SessionService.loggedInTeam.teamMembers) {
                member.userPlannedDays = member.userBookedDays;
                
                var usersInProgChanges = changeList.Where (c => c.userResponsible.userID == member.user.userID && c.status == "Not Started").ToList ();
                foreach (var change in usersInProgChanges) {
                    int daysRemaining = change.processingTime - ((int) (change.startedDate ?? DateTime.Now).Subtract (DateTime.Now).TotalDays);
                    member.userPlannedDays = member.userPlannedDays + daysRemaining;
                }

                member.userLowestDays = member.userPlannedDays;
            }
        }

        public TeamMember calculateTeamMemberWithLeastPlanned (Change change) {
            
            var member = SessionService.loggedInTeam.teamMembers.Where (m => m.userLowestDays == (SessionService.loggedInTeam.teamMembers.Min (l => l.userLowestDays))).FirstOrDefault ();
            member.userLowestDays = member.userLowestDays + change.processingTime;

            return member;
        }
    }
}