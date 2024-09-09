namespace AluraRPA.Infrastructure.Services;
public class ProcessManagerService : IProcessManagerService
{
    public const int MAX_PROCESS_INACTIVE_COUNT = 5;
    public const int WAIT_LIMIT_SECONDS = 30;
    public const int CPU_PERCENTAGE = 2;

    public void TaskKillCmd(string imageName)
    {
        try
        {
            var command = @$"/C taskkill /F /FI ""USERNAME eq {Environment.UserName}"" /IM {imageName}";
            using var process = Process.Start("cmd.exe", command);
            process.WaitForExit((int)TimeSpan.FromSeconds(20).TotalSeconds);
        }
        catch (Exception e)
        {
            Console.WriteLine($"{nameof(ProcessManagerService)}: " + e.Message);
        }
    }

    public async Task WaitProcessCpuIdle(string processName, int waitForProcessDelay = 1000)
    {
        try
        {
            Process process = default;
            var waitLimit = 0;
            while (process is null)
            {
                if (++waitLimit > WAIT_LIMIT_SECONDS) return;
                process = Process.GetProcessesByName(processName).FirstOrDefault();
                await Task.Delay(waitForProcessDelay);
            }

            var cpu = new PerformanceCounter("Process", "% Processor Time", process.ProcessName, true);
            var inactivityCount = 1;

            while (inactivityCount < MAX_PROCESS_INACTIVE_COUNT)
            {
                var CPU = Math.Round(cpu.NextValue(), 2);
                if ((int)CPU > CPU_PERCENTAGE)
                    inactivityCount = 0;
                else
                    inactivityCount++;

                Console.WriteLine(@$"\t Processo {processName} encontrado - CPU: {CPU} -- PID: {process.Id} | Inactivity Count: {inactivityCount}");
                await Task.Delay(1000);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"{nameof(ProcessManagerService)}: " + e.Message);
        }
    }
}