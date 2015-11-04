namespace PhotoContest.App.Jobs
{
    using PhotoContest.App.Controllers;
    using PhotoContest.Data.Interfaces;
    using Quartz;
    using Quartz.Impl;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;

    public class FinilizeContestJob : IJob
    {
        public IPhotoContestData Data { get; set; }
        public FinilizeContestJob(IPhotoContestData data)
        {
            this.Data = data;
        }

        public void Execute(IJobExecutionContext context)
        {
            var contests = this.Data.Contests.All().Where(c => c.Deadline <= DateTime.Now);

            if (contests.Count() > 0)
            {
                var contestController = new ContestsController(this.Data);
                foreach (var contest in contests)
                {
                    contestController.FinalizeContest(contest.Id);
                }
            }
        }
    }
}