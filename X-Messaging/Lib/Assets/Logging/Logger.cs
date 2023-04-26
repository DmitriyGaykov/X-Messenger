using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Assets.Logging;
public class Logger
{
	private static Logger logger = null;
	private static string logfile;
	private static StreamWriter file;
	private static bool toConsole = false;
	private static readonly object locker = new object();
	private Logger(string logfile)
	{
		Logger.logfile = logfile;
		this.Clear();
		this.Start();
	}

	public static bool ToConsole
	{
		get => toConsole;
		set => toConsole = value;
	}

	public static Logger GetLogger(string logfile = "program.log")
	{
		if(logger is null)
		{
			logger = new Logger(logfile);
		}

		return logger;
	}

	public void Clear()
	{
		using(file = new StreamWriter(logfile))
		{
			return;
		}
	}


	public void Start()
	{
		var now = DateTime.Now;

		string start =
        $"""
		[START]
		{now.Date}		|			{now.ToLongTimeString()}
		Запуск приложения: {AppDomain.CurrentDomain.FriendlyName}

		""";

		WriteLine(start);
	}

	public async void MicroInfo(string msg)
	{
		await Task.Run(() =>
		{
            var now = DateTime.Now;

            string info =
            $"""
				
				[MICROINFO] {now.ToLongTimeString()}	||			{msg}
			""";

            Write(info);
        });
	}

	public async void IInfo(string msg)
	{
		await Task.Run(() =>
		{
            var now = DateTime.Now;

            string info =
            $"""

			[IMPORTANT INFO]
			{now.Date}		|			{now.ToLongTimeString()}
			Message:
			{msg}

			""";

            Write(info);
        });
	}
	public async void Info(string msg)
	{
		await Task.Run(() =>
		{
            var now = DateTime.Now;

            string info =
                $"""

			[INFO]
			{now.Date}		|			{now.ToLongTimeString()}
			Message:
			{msg}

			""";

            Write(info);
        });
	}

	public async void MicroWarning(string msg)
	{
        await Task.Run(() =>
        {
            var now = DateTime.Now;

            string warn =
            $"""
				[MICROWARNING] {now.ToLongTimeString()} | {msg}
			""";

            WriteLine(warn);
        });
    }

	public async void Warning(string msg)
	{
		await Task.Run(() =>
		{
            var now = DateTime.Now;

            string warn =
                $"""

			[WARNING]
			{now.Date}		|			{now.ToLongTimeString()}

			Warning:
			{msg}

			""";

            Write(warn);
        });
	}

	public void End()
	{

		var now = DateTime.Now;

		string end =
			$"""
			[END]
			{now.Date}		|			{now.ToLongTimeString()}
			Конец приложения: {AppDomain.CurrentDomain.FriendlyName}

			""";

		WriteLine(end);
	}

	public void Dispose()
	{
		file.Close();
	}

    private const string line = "\n-------------------------------------------------------------------------------------------------------------------------------------------\n";

    public void Write(string msg)
	{
        if (toConsole)
		{
			Console.Write(msg);
		}
		lock (locker)
		{
			using(file = new(logfile, true))
				file.Write(msg);
		}
	}

	public void WriteLine(string msg)
	{
        if (toConsole)
		{
			Console.WriteLine(msg);
		}

		lock (locker)
		{
            using (file = new(logfile, true))
                file.WriteLine(msg);
		}
	}
}
