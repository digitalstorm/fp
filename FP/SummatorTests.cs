using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FP
{
	[TestFixture]
	public class SummatorTests
	{
	    private HexSumFormatter formatter;
	    private const string LargeInputFilename = "process-large-file.txt";
		private const string OutputFilename = "process-result.txt";
		private const string ExpectedOutputFilename = "expected-process-result.txt";

	    [SetUp]
	    public void SetUp()
	    {
	        formatter = new HexSumFormatter();
	    }

	    [Test]
		public void Process_GeneratesCorrectOutputFile()
		{
			var actualResultFile = new FileInfo(OutputFilename);
			if (actualResultFile.Exists) actualResultFile.Delete();

	        DoTest();

	        CollectionAssert.AreEqual(
				File.ReadAllLines(ExpectedOutputFilename),
				File.ReadAllLines(actualResultFile.FullName));
		}

	    [Test]
		public void Process_ShowProgressOnConsole()
		{
			var stdOut = Console.Out;
			try
			{
				var consoleOutput = new StringWriter();
				Console.SetOut(consoleOutput);

                DoTest();

                var actualOutput = consoleOutput.ToString()
					.TrimEnd()
					.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
				Assert.AreEqual("processed 100 items", actualOutput.First());
				Assert.AreEqual("processed 1000 items", actualOutput.Last());
				Assert.AreEqual(10, actualOutput.Length);
			}
			finally
			{
				Console.SetOut(stdOut);
			}
		}

	    private void DoTest()
	    {
	        Action<int> logProgress = index =>
	        {
	            if (index%100 == 0)
	                Console.WriteLine("processed {0} items", index);
	        };

            using (var input = new DataSource(LargeInputFilename))
            {
                SummatorExtensions
                    .RepeatUnitNull(input.NextParsedRecord)
                    .Select(formatter.SumAndFormat)
                    .Notify(logProgress)
                    .WriteAllLines(OutputFilename);
            }
	    }

	    [Test]
		[Explicit("Генератор данных. Не нужен для выполнения задания")]
		public void GenerateInput()
		{
			var r = new Random();
			File.WriteAllLines(LargeInputFilename,
				Enumerable.Range(0, 1000).Select(i =>
					string.Join(
						" ",
						Enumerable.Range(0, 4).Select(j => Convert.ToString(r.Next(1000), 16))
						)));
		}
	}
}