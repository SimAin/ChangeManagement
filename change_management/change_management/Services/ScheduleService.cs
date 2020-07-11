using change_management.Models;
using System;
using System.Linq;
using System.Collections.Generic;

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

            foreach (var item in changeList)
            {
                item.deadlineStatus = 99;

                if(item.status == "In progress"){
                    //It is in progress therefore it wil have a start date. 
                    int daysRemaining = item.processingTime - ((int) (item.startedDate ?? DateTime.Now).Subtract(DateTime.Now).TotalDays);
                    int twentyP = (int) Math.Ceiling(item.processingTime * 0.2);
                    
                    if(DateTime.Now.AddDays(daysRemaining) < item.deadline.AddDays(- twentyP)){
                        item.deadlineStatus = 1;
                    } 
                    if((DateTime.Now.AddDays(daysRemaining) >= item.deadline.AddDays(- twentyP)) && (DateTime.Now.AddDays(daysRemaining) < item.deadline)) {
                        item.deadlineStatus = 2;
                    }
                    if(DateTime.Now.AddDays(daysRemaining) >= item.deadline) {
                        item.deadlineStatus = 4;
                    }
                }
                if ((item.status != "In Progress") && (item.laxity < item.processingTime)) {
                    item.deadlineStatus = 5;
                }
            }

            return changeList;
        }
    }
}