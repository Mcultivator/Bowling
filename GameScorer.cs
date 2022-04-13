namespace GameScorer
{

    public abstract class GameScorer
    {
        private const int Frames = 10;
        private static int remainingPins = 0;
        private static int lineAsNumber = 0;
        private static List<List<int>>? scores;
        private static List<int>? frameScores;
        private static bool tenthFrameThirdRollFlag = false;
        private static int runningScore = 0;
        protected static bool sanitizeInput(int frame, int roll, string line)
        {
            try
            {
                if(line == "" || line == null)
                {
                    throw new Exception("Not a positive integer, please try again.");
                }
                foreach(char c in line)
                {
                    if(!char.IsDigit(c))
                    {
                        throw new Exception("Not a positive integer, please try again.");
                    }
                }

                int.TryParse(line, out lineAsNumber);

                if(lineAsNumber >= 11 || lineAsNumber < 0)
                {
                    throw new Exception("Not a valid integer (must be less than 11), please try again.");
                }

                if(remainingPins - lineAsNumber < 0)
                {
                    throw new Exception("Not enough pins remaining for this score, please input a different score.");
                }
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        protected static void readSanitizedLine(int frame, int roll)
        {
            var line = Console.ReadLine();

            while(!sanitizeInput(frame, roll, line))
            {
                System.Console.WriteLine($"Please input score for frame {frame + 1}, roll {roll + 1}:");
                line = Console.ReadLine();
            }
        }

        protected static void rollFirstNineFrames()
        {
            scores = new List<List<int>>();

            for(var frame = 0;frame < Frames - 1;frame++)
            {
                remainingPins = 10;
                frameScores = new List<int>();

                for(var roll = 0;roll < 2;roll++)
                {
                    System.Console.WriteLine($"Please input score for frame {frame + 1}, roll {roll + 1}:");
                    readSanitizedLine(frame, roll);

                    remainingPins -= lineAsNumber;

                    frameScores.Add(lineAsNumber);

                    if(roll == 0 && remainingPins == 0)
                    {
                        System.Console.WriteLine("Strike!!!");
                        break;
                    }
                    else if (roll == 1)
                    {
                        if(remainingPins == 0)
                        {
                            System.Console.WriteLine("Spare.");
                        }
                    }
                }

                scores.Add(frameScores);
            }
        }

        protected static void rollTenthFrame()
        {
            remainingPins = 10;
            frameScores = new List<int>();
            System.Console.WriteLine("Please input score for frame 10, roll 1:");
            readSanitizedLine(10, 0);
            remainingPins -= lineAsNumber;
            frameScores.Add(lineAsNumber);
            if(remainingPins == 0)
            {
                remainingPins = 10;
                tenthFrameThirdRollFlag = true;
            }
            System.Console.WriteLine("Please input score for frame 10, roll 2:");
            readSanitizedLine(10, 1);
            remainingPins -= lineAsNumber;
            frameScores.Add(lineAsNumber);
            if(remainingPins == 0)
            {
                remainingPins = 10;
                tenthFrameThirdRollFlag = true;
            }
            if(tenthFrameThirdRollFlag)
            {
                System.Console.WriteLine("Please input score for frame 10, roll 3:");
                readSanitizedLine(10, 2);
                remainingPins -= lineAsNumber;
                frameScores.Add(lineAsNumber);
            }
            scores.Add(frameScores);
        }

        public static void scoreGame()
        {
            rollFirstNineFrames();

            rollTenthFrame();
            
            for(int frame = 0;frame < Frames;frame++)
            {
                if(frame < Frames - 1)
                {
                    if(scores[frame].Count == 1)
                    {
                        runningScore += 10;
                        runningScore += scores[frame + 1][0];
                        if(scores[frame + 1].Count == 1)
                            runningScore += scores[frame + 2][0];
                        else
                            runningScore += scores[frame + 1][1];
                    }
                    else if(scores[frame][0] + scores[frame][1] == 10)
                    {
                        runningScore += 10;
                        runningScore += scores[frame + 1][0];
                    }
                    else
                    {
                        runningScore += scores[frame][0] + scores[frame][1];
                    }
                }
                else
                {
                    for(int roll = 0;roll < scores[frame].Count;roll++)
                        runningScore += scores[frame][roll];
                }
            }

            System.Console.WriteLine($"The total score is: {runningScore}");
        }
    }
}