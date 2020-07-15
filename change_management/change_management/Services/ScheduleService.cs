using System;
using System.Collections.Generic;
using System.Linq;
using change_management.Models;
using change_management.Services;

namespace change_management.Services {
    public class ScheduleService {
        private int userWaitTime = 0;
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

            userWaitTime = calculateUserBookedDays (changeList, SessionService.loggedInUser.userID);
            foreach (var item in changeList) {
                item.deadlineStatus = 99;
                calculateDeadlineStatus (item);
            }

            return changeList;
        }

        public void calculateDeadlineStatus (Change change) {

            if (change.status == "In progress") {
                //It is in progress therefore it wil have a start date. 
                int daysRemaining = change.processingTime - Math.Abs(((int) (change.startedDate ?? DateTime.Now).Subtract (DateTime.Now).TotalDays));

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

        public int calculateUserBookedDays (List<Change> changeList, int userID) {
            var userPlannedDays = 0;
            foreach (var item in changeList) {
                if (item.status == "In progress") {
                    //It is in progress therefore it wil have a start date. 
                    int daysRemaining = item.processingTime - ((int) (item.startedDate ?? DateTime.Now).Subtract (DateTime.Now).TotalDays);

                    if (item.userResponsible.userID == userID) {
                        userPlannedDays = userPlannedDays + daysRemaining;
                    }
                }
            }

            return userPlannedDays;
        }
    }
}