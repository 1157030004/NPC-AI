namespace Shadee.NPC_Villagers
{
    public class ScheduleEventArgs
    {
            public int Hour;
            public int Minute;
            public int Day;
            public int Month;
            public int Year;
            public BaseInteraction Interaction;
            public SmartObject Target;

            public ScheduleEventArgs()
            {
                
            }

            public ScheduleEventArgs(int hour, int minute, int day, int month, int year, BaseInteraction interaction, SmartObject target)
            {
                Hour = hour;
                Minute = minute;
                Day = day;
                Month = month;
                Year = year;
                Interaction = interaction;
                Target = target;
            }
    }
}