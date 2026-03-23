using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;

namespace prace
{
    internal class Job
    {
        public string name { get; }
        public int length { get; }
        public int reward { get; }

        public Job(string name, int length, int reward)
        {
            this.name = name;
            this.length = length;
            this.reward = reward;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            int time = 48;
            string csvPath = "MilionarZaVikend.csv";
            List<Job> jobs = new List<Job>();
            using(StreamReader sr =  new StreamReader(csvPath))
            {
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var values = line.Split(',');
                    string name = values[0];
                    int length = int.Parse(values[1]);
                    int reward = int.Parse(values[2]);
                    jobs.Add(new Job(name, length, reward));
                }
            }
            int[,] array = new int[jobs.Count, time+1];
            for (int jobID = 0; jobID < jobs.Count; jobID++)
            {
                for (int currentTime = 0; currentTime <= time; currentTime++)
                {
                    Job job = jobs[jobID];
                    // check if we can even add the job
                    if (job.length > currentTime)
                    {
                        // cannot add job, copy the field above
                        if (jobID == 0)
                        {
                            array[jobID, currentTime] = 0;
                        }
                        else
                        {
                            array[jobID, currentTime] = array[jobID - 1, currentTime];
                        }
                        continue;
                    }
                    // We can add the job, now we neeed to decide whether it's worth it
                    if (jobID == 0)
                    {
                        // It's always worth it to add it
                        array[jobID, currentTime] = job.reward;
                        continue;
                    }
                    int bestReward = int.MinValue;
                    for (int i = 0; i <= currentTime - job.length; i++)
                    {
                        if (array[jobID - 1, i] > bestReward)
                        {
                            bestReward = array[jobID - 1, i];
                        }
                    }
                    int rewardValue = bestReward + job.reward;
                    if (rewardValue > array[jobID - 1, currentTime])
                    {
                        array[jobID, currentTime] = rewardValue;
                    }
                    else
                    {
                        array[jobID, currentTime] = array[jobID - 1, currentTime];
                    }
                    if (currentTime != 0)
                    {
                        if (array[jobID, currentTime] < array[jobID, currentTime - 1])
                        {
                            throw new Exception();
                        }
                    }
                }
            }
            // reconstruct it
            List<string> jobNames = new List<string>();
            int currentReward = array[jobs.Count - 1, time];
            int currentJobID = jobs.Count - 1;
            int currentTimeS = time;
            Console.WriteLine(currentReward);
            while (currentReward > 0)
            {
                // are we using this job?
                if(currentJobID == 0)
                {
                    jobNames.Add(jobs[0].name);
                    break;
                }
                if (array[currentJobID, currentTimeS] == array[currentJobID-1, currentTimeS])
                {
                    // we are not using this job
                    currentJobID--;
                    continue;
                }
                // we are using this job
                jobNames.Add(jobs[currentJobID].name);
                // move it up
                currentTimeS -= jobs[currentJobID].length;
                currentReward -= jobs[currentJobID].reward;
                currentJobID--;
            }
            foreach(string jobName in jobNames)
            {
                Console.WriteLine(jobName);
            }
        }
    }
}
