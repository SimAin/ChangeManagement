using change_management.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using change_management.Services;

namespace change_management.Services
{
    public class ScheduleService
    {
        public ScheduleService(){}

        // TODO: Boost up C0 if deadline has done etc

        public List<Change> scheduleChanges(List<Change> changeList) {

            List<Change> orderedCritical =  new List<Change>();
            List<Change> orderedNonCritical =  new List<Change>();

            var critical = changeList.Where(c => c.criticality == true).ToList();
            if (critical.Count() > 1){
                orderedCritical =  SplitList(critical);
            } else {
                orderedCritical =  critical;
            }
            var nonCritical = changeList.Where(c => c.criticality == false).ToList();
            if (nonCritical.Count() > 1){
                orderedNonCritical =  SplitList(nonCritical);
            } else {
                orderedNonCritical =  nonCritical;
            }

            orderedCritical.AddRange(orderedNonCritical);
            List<Change> completeOrder = orderedCritical;
            return orderedCritical.ToList();
        }


        private List<Change> SplitList(List<Change> unOrdered)
        {
            if (unOrdered.Count <= 1)
                return unOrdered;

            List<Change> listOne = new List<Change>();
            List<Change> listTwo = new List<Change>();

            for (int i = 0; i < (unOrdered.Count / 2); i++)  
            {
                var di = (int) DateTime.Now.Subtract(unOrdered[i].deadline).TotalDays;
                unOrdered[i].laxity = di - unOrdered[i].processingTime;
                listOne.Add(unOrdered[i]);
            }
            for (int i = (unOrdered.Count / 2); i < unOrdered.Count; i++)
            {
                var di = (int) DateTime.Now.Subtract(unOrdered[i].deadline).TotalDays;
                unOrdered[i].laxity = di - unOrdered[i].processingTime;
                listTwo.Add(unOrdered[i]);
            }

            listOne = SplitList(listOne);
            listTwo = SplitList(listTwo);
            return Merge(listOne, listTwo);
        }

        private List<Change> Merge(List<Change> listOne, List<Change> listTwo)
        {
            List<Change> result = new List<Change>();

            while(listOne.Count > 0 || listTwo.Count>0)
            {
                if (listOne.Count > 0 && listTwo.Count > 0)
                {
                    if (listOne.First().laxity > listTwo.First().laxity) {

                        result.Add(listOne.First());
                        listOne.Remove(listOne.First());

                    } else if(listOne.First().laxity < listTwo.First().laxity) {

                        result.Add(listTwo.First());
                        listTwo.Remove(listTwo.First());

                    } else {
                        if (listOne.First().priority < listTwo.First().priority) {

                            result.Add(listOne.First());
                            listOne.Remove(listOne.First());

                        } else if (listOne.First().priority > listTwo.First().priority) {

                            result.Add(listTwo.First());
                            listTwo.Remove(listTwo.First());
                        } else {
                            if (listOne.First().createdDate > listTwo.First().createdDate) {

                                result.Add(listOne.First());
                                listOne.Remove(listOne.First());

                            } else if (listOne.First().createdDate <= listTwo.First().createdDate) {

                                result.Add(listTwo.First());
                                listTwo.Remove(listTwo.First());
                            }
                        }
                    }
                }
                else if(listOne.Count>0)
                {
                    result.Add(listOne.First());
                    listOne.Remove(listOne.First());
                }
                else if (listTwo.Count > 0)
                {
                    result.Add(listTwo.First());
                    listTwo.Remove(listTwo.First());    
                }    
            }
            return result;
        }

        public List<Change> calculateDeadlineStatus(List<Change> changeList) {
            var inProgressPlannedDays = 0;
            //var spentDays = 0;

            var userWaitTime = calculateUserBookedDays(changeList);
            foreach (var item in changeList)
            {
                item.deadlineStatus = 99;
                int twentyP = (int) Math.Ceiling(item.processingTime * 0.2);

                if(item.status == "In progress"){
                    //It is in progress therefore it wil have a start date. 
                    int daysRemaining = item.processingTime - ((int) (item.startedDate ?? DateTime.Now).Subtract(DateTime.Now).TotalDays);
                    
                    
                    if(DateTime.Now.AddDays(daysRemaining).Date < item.deadline.AddDays(- twentyP).Date){
                        item.deadlineStatus = 1;
                    } 
                    if((DateTime.Now.AddDays(daysRemaining).Date >= item.deadline.AddDays(- twentyP).Date) && (DateTime.Now.AddDays(daysRemaining).Date < item.deadline.Date)) {
                        item.deadlineStatus = 2;
                    }
                    if(DateTime.Now.AddDays(daysRemaining).Date == item.deadline.Date) {
                        item.deadlineStatus = 3;
                    }
                    if(DateTime.Now.AddDays(daysRemaining).Date > item.deadline.Date) {
                        Console.WriteLine(DateTime.Now.AddDays(daysRemaining));
                        item.deadlineStatus = 4;
                    }

                    inProgressPlannedDays = inProgressPlannedDays + daysRemaining;
                }
                if ((item.status != "In progress")) {
                    
                    if(item.userResponsible.userID == SessionService.loggedInUser.userID)
                    {
                        if (DateTime.Now.AddDays(userWaitTime + item.processingTime).Date < item.deadline.AddDays(- twentyP).Date){
                            item.deadlineStatus = 1;
                            userWaitTime = userWaitTime + item.processingTime;
                        }
                        if((DateTime.Now.AddDays(userWaitTime + item.processingTime).Date >= item.deadline.AddDays(- twentyP).Date) && (DateTime.Now.AddDays(userWaitTime + item.processingTime).Date < item.deadline.Date)) {
                            item.deadlineStatus = 2;
                            userWaitTime = userWaitTime + item.processingTime;
                        }
                        if(DateTime.Now.AddDays(userWaitTime + item.processingTime).Date >= item.deadline.Date) {
                            item.deadlineStatus = 4;
                            userWaitTime = userWaitTime + item.processingTime;
                        }
                        if(DateTime.Now.Date > item.deadline.Date) {
                            item.deadlineStatus = 5;
                            userWaitTime = userWaitTime + item.processingTime;
                        }
                    }

                    // if (item.laxity < item.processingTime) {
                    //     Console.WriteLine(item.laxity + " -  " + item.processingTime);
                    //     item.deadlineStatus = 5;
                    // }
                }
            }
            
            // var remainingDays = SessionService.loggedInTeam.throughput - inProgressPlannedDays;
            // int daysUntilFriday = ((int) DayOfWeek.Friday - (int) DateTime.Now.DayOfWeek + 7) % 7;

            return changeList;
        }

        public int calculateDeadlineStatus(Change change) {
            int conf = 99;
            

            if(change.status == "In progress"){
                //It is in progress therefore it wil have a start date. 
                int daysRemaining = change.processingTime - ((int) (change.startedDate ?? DateTime.Now).Subtract(DateTime.Now).TotalDays);
                int twentyP = (int) Math.Ceiling(change.processingTime * 0.2);
                
                if(DateTime.Now.AddDays(daysRemaining) < change.deadline.AddDays(- twentyP)){
                    conf = 1;
                } 
                if((DateTime.Now.AddDays(daysRemaining) >= change.deadline.AddDays(- twentyP)) && (DateTime.Now.AddDays(daysRemaining) < change.deadline)) {
                    conf = 2;
                }
                if(DateTime.Now.AddDays(daysRemaining) >= change.deadline) {
                    conf = 4;
                }
                if(DateTime.Now > change.deadline) {
                    conf = 5;
                }
            }

            if (change.status == "Not Started") {
                if (change.laxity < change.processingTime) {
                    change.deadlineStatus = 5;
                }
            }

            return conf;
        }

        public int calculateUserBookedDays(List<Change> changeList) {
            var userPlannedDays = 0;
            foreach (var item in changeList)
            {
                if(item.status == "In progress"){
                    //It is in progress therefore it wil have a start date. 
                    int daysRemaining = item.processingTime - ((int) (item.startedDate ?? DateTime.Now).Subtract(DateTime.Now).TotalDays);

                    if(item.userResponsible.userID == SessionService.loggedInUser.userID) {
                        userPlannedDays = userPlannedDays + daysRemaining;
                    }
                }
            }

            return userPlannedDays;
        }
    }
}